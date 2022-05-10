using System;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid.GameResolvers;
using TbsFramework.Grid.GridStates;
using TbsFramework.Grid.TurnResolvers;
using TbsFramework.Grid.UnitGenerators;
using TbsFramework.Players;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine;
using Photon.Pun;

namespace TbsFramework.Grid
{
    /// <summary>
    /// CellGrid class keeps track of the game, stores cells, units and players objects. It starts the game and makes turn transitions. 
    /// It reacts to user interacting with units or cells, and raises events related to game progress. 
    /// 
    /// ellGrid 클래스는 게임을 추적하고 셀, 유닛 및 플레이어 객체를 저장합니다. 게임을 시작하고 턴 전환을 합니다.
    /// 유닛이나 셀과의 상호작용에 반응하여 게임 진행과 관련된 이벤트를 발생시킵니다.
    /// </summary>

    [RequireComponent(typeof(PhotonView))]
    public class CellGrid : MonoBehaviour
    {
        public static CellGrid instance;
        /// <summary>
        /// LevelLoading event is invoked before Initialize method is run.
        /// 초기화 메서드가 실행되기 전에 LevelLoading 이벤트가 호출됩니다.
        /// </summary>
        public event EventHandler LevelLoading;
        /// <summary>
        /// LevelLoadingDone event is invoked after Initialize method has finished running.
        /// Level Loading Done 이벤트는 초기화 메서드의 실행이 완료된 후에 호출됩니다.
        /// </summary>
        public event EventHandler LevelLoadingDone;
        /// <summary>
        /// GameStarted event is invoked at the beggining of StartGame method.
        /// </summary>
        public event EventHandler GameStarted;
        /// <summary>
        /// GameEnded event is invoked when there is a single player left in the game.
        /// GameStarted 이벤트는 StartGame 메서드의 시작 시 실행됩니다.
        /// </summary>
        public event EventHandler<GameEndedArgs> GameEnded;
        /// <summary>
        /// Turn ended event is invoked at the end of each turn.
        /// 턴 종료 이벤트는 각 턴의 마지막에 호출됩니다.
        /// </summary>
        public event EventHandler TurnEnded;

        /// <summary>
        /// UnitAdded event is invoked each time AddUnit method is called.
        /// UnitAdded 이벤트는 AddUnit 메서드가 호출될 때마다 호출됩니다.
        /// </summary>
        public event EventHandler<UnitCreatedEventArgs> UnitAdded;



        private PhotonView photonView;




        private CellGridState _cellGridState; //The grid delegates some of its behaviours to cellGridState object. 그리드는 일부 동작을 cellGridState 객체에 위임합니다.
        public CellGridState CellGridState
        {
            get
            {
                return _cellGridState;
            }
            set
            {
                CellGridState nextState;
                if (_cellGridState != null)
                {
                    _cellGridState.OnStateExit();
                    nextState = _cellGridState.MakeTransition(value);
                }
                else
                {
                    nextState = value;
                }

                _cellGridState = nextState;
                _cellGridState.OnStateEnter(); // cellGrid를 전체 unmark해주는 함수  foreach로 일일히 지정해준다. 
            }
        }

        public int NumberOfPlayers { get { return Players.Count; } }

        public Player CurrentPlayer
        {
            get { return Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)); }
        }
        public int CurrentPlayerNumber { get; private set; }

        [HideInInspector]
        public bool Is2D;

        /// <summary>
        /// GameObject that holds player objects.
        /// </summary>
        public Transform PlayersParent;

        public bool GameFinished { get; private set; }
        public List<Player> Players { get; private set; }
        public List<Cell> Cells { get; private set; }
        public List<Unit> Units { get; private set; }
        private List<Unit> PlayableUnits = new List<Unit>();

        //Awake함수에서 photonView를 가져온뒤. 
        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }


        private void Start()
        {
            if (LevelLoading != null)
                LevelLoading.Invoke(this, new EventArgs());

            Initialize();

            if (LevelLoadingDone != null)
                LevelLoadingDone.Invoke(this, new EventArgs());

            StartGame();
        }

        private void Initialize()
        {
            GameFinished = false;
            Players = new List<Player>();
            for (int i = 0; i < PlayersParent.childCount; i++)
            {
                var player = PlayersParent.GetChild(i).GetComponent<Player>();
                if (player != null && player.gameObject.activeInHierarchy)
                {
                    player.Initialize(this);
                    Players.Add(player);
                }
            }

            Cells = new List<Cell>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var cell = transform.GetChild(i).gameObject.GetComponent<Cell>();
                if (cell != null)
                {
                    if (cell.gameObject.activeInHierarchy)
                    {
                        Cells.Add(cell);
                    }
                }
                else
                {
                    Debug.LogError("Invalid object in cells paretn game object");
                }
            }

            foreach (var cell in Cells)
            {
                cell.CellClicked += OnCellClicked;
                cell.CellHighlighted += OnCellHighlighted;
                cell.CellDehighlighted += OnCellDehighlighted;
                cell.GetComponent<Cell>().GetNeighbours(Cells);
            }

            Units = new List<Unit>();
            var unitGenerator = GetComponent<IUnitGenerator>();
            if (unitGenerator != null)
            {
                var units = unitGenerator.SpawnUnits(Cells);
                foreach (var unit in units)
                {
                    AddUnit(unit.GetComponent<Transform>());
                }
            }
            else
            {
                Debug.LogError("No IUnitGenerator script attached to cell grid");
            }
        }

        private void OnCellDehighlighted(object sender, EventArgs e)
        {
            CellGridState.OnCellDeselected(sender as Cell);
        }
        private void OnCellHighlighted(object sender, EventArgs e)
        {
            CellGridState.OnCellSelected(sender as Cell);
        }
        private void OnCellClicked(object sender, EventArgs e)
        {
            CellGridState.OnCellClicked(sender as Cell);
        }

        private void OnUnitClicked(object sender, EventArgs e)
        {
            CellGridState.OnUnitClicked(sender as Unit);
        }
        private void OnUnitHighlighted(object sender, EventArgs e)
        {
            CellGridState.OnUnitHighlighted(sender as Unit);
        }
        private void OnUnitDehighlighted(object sender, EventArgs e)
        {
            CellGridState.OnUnitDehighlighted(sender as Unit);
        }

        private void OnUnitDestroyed(object sender, AttackEventArgs e)
        {
            Units.Remove(sender as Unit);
            (sender as Unit).GetComponents<Ability>().ToList().ForEach(a => a.OnUnitDestroyed(this));
            CheckGameFinished();
        }

        /// <summary>
        /// Adds unit to the game
        /// </summary>
        /// <param name="unit">Unit to add</param>
        public void AddUnit(Transform unit)
        {
            Units.Add(unit.GetComponent<Unit>());
            unit.GetComponent<Unit>().UnitClicked += OnUnitClicked;
            unit.GetComponent<Unit>().UnitHighlighted += OnUnitHighlighted;
            unit.GetComponent<Unit>().UnitDehighlighted += OnUnitDehighlighted;
            unit.GetComponent<Unit>().UnitDestroyed += OnUnitDestroyed;
            unit.GetComponent<Unit>().UnitMoved += OnUnitMoved;

            if (UnitAdded != null)
                UnitAdded.Invoke(this, new UnitCreatedEventArgs(unit));
        }

        private void OnUnitMoved(object sender, MovementEventArgs e)
        {
            CheckGameFinished();
            //photonView.RPC(nameof(RPC_OnUnitMoved), RpcTarget.AllBuffered, new object[] { e });
        }

        //분명히 여기는 플레이어가 움직이는 곳인데 어떻게 코드를 짜야하는걸까..........
        //일단 따라해보자.
        [PunRPC]
        //private object RPC_OnUnitMoved(Vector2 e)
        //{
        //    Vector2Int intE = new Vector2Int(Mathf.RoundToInt(e.x), Mathf.RoundToInt(e.y));
        //    //OnUnitMoved(intE);
        //}

        /// <summary>
        /// Method is called once, at the beggining of the game.
        /// </summary>
        public void StartGame()
        {
            if (GameStarted != null)
                GameStarted.Invoke(this, new EventArgs());

            TransitionResult transitionResult = GetComponent<TurnResolver>().ResolveStart(this);
            PlayableUnits = transitionResult.PlayableUnits;
            CurrentPlayerNumber = transitionResult.NextPlayer.PlayerNumber;

            PlayableUnits.ForEach(u => { u.GetComponents<Ability>().ToList().ForEach(a => a.OnTurnStart(this)); u.OnTurnStart(); });
            CurrentPlayer.Play(this);
            Debug.Log("Game started");
        }
        /// <summary>
        /// Method makes turn transitions. It is called by player at the end of his turn.
        /// </summary>
        public void EndTurn()
        {
            CellGridState = new CellGridStateBlockInput(this);
            bool isGameFinished = CheckGameFinished();
            if (isGameFinished)
            {
                return;
            }

            PlayableUnits.ForEach(u => { if (u != null) { u.OnTurnEnd(); u.GetComponents<Ability>().ToList().ForEach(a => a.OnTurnEnd(this)); } });

            TransitionResult transitionResult = GetComponent<TurnResolver>().ResolveTurn(this);
            PlayableUnits = transitionResult.PlayableUnits;
            CurrentPlayerNumber = transitionResult.NextPlayer.PlayerNumber;

            if (TurnEnded != null)
                TurnEnded.Invoke(this, new EventArgs());

            Debug.Log(string.Format("Player {0} turn", CurrentPlayerNumber));
            PlayableUnits.ForEach(u => { u.GetComponents<Ability>().ToList().ForEach(a => a.OnTurnStart(this)); u.OnTurnStart(); });
            CurrentPlayer.Play(this);
        }

        public List<Unit> GetCurrentPlayerUnits()
        {
            return PlayableUnits;
        }
        public List<Unit> GetEnemyUnits(Player player)
        {
            return Units.FindAll(u => u.PlayerNumber != player.PlayerNumber);
        }
        public List<Unit> GetPlayerUnits(Player player)
        {
            return Units.FindAll(u => u.PlayerNumber == player.PlayerNumber);
        }

        public bool CheckGameFinished()
        {
            List<GameResult> gameResults =
                GetComponents<GameEndCondition>()
                .Select(c => c.CheckCondition(this))
                .ToList();

            foreach (var gameResult in gameResults)
            {
                if (gameResult.IsFinished)
                {
                    CellGridState = new CellGridStateGameOver(this);
                    GameFinished = true;
                    if (GameEnded != null)
                    {
                        GameEnded.Invoke(this, new GameEndedArgs(gameResult));
                    }

                    break;
                }
            }
            return GameFinished;
        }
    }
}

