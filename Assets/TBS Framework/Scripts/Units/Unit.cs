using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using TbsFramework.Cells;
using TbsFramework.Pathfinding.Algorithms;
using TbsFramework.Units.Highlighters;
using TbsFramework.Units.UnitStates;
using TbsFramework.Grid;
using TbsFramework.Players.AI.Actions;
using TbsFramework.Players.AI.Evaluators;
using Photon.Pun;

namespace TbsFramework.Units
{
    /// <summary>
    /// Base class for all units in the game.
    /// 게임 내 모든 유닛의 베이스 클래스.
    /// </summary>
    [ExecuteInEditMode]
    public class Unit : MonoBehaviourPun
    {
        public static Unit instance;

        Dictionary<Cell, List<Cell>> cachedPaths = null;
        /// <summary>
        /// UnitClicked event is invoked when user clicks the unit. 
        /// It requires a collider on the unit game object to work.
        /// Unit Clicked 이벤트는 사용자가 장치를 클릭하면 호출됩니다.
        /// 작동하려면 유닛 게임 오브젝트에 충돌기가 필요합니다.
        /// </summary>
        public event EventHandler UnitClicked;
        /// <summary>
        /// UnitSelected event is invoked when user clicks on unit that belongs to him. 
        /// It requires a collider on the unit game object to work.
        /// Unit Selected 이벤트는 사용자가 자신의 유닛을 클릭하면 호출됩니다.
        /// 작동하려면 유닛 게임 오브젝트에 충돌기가 필요합니다.
        /// </summary>
        public event EventHandler UnitSelected;

  
        /// <summary>
        /// UnitDeselected event is invoked when user click outside of currently selected unit's collider.
        /// It requires a collider on the unit game object to work.
        /// UnitDeselected 이벤트는 사용자가 현재 선택한 장치의 충돌기 외부에서 클릭하면 호출됩니다.
        /// 작동하려면 유닛 게임 오브젝트에 충돌기가 필요합니다.
        /// </summary>
        public event EventHandler UnitDeselected;
        /// <summary>
        /// UnitHighlighted event is invoked when user moves cursor over the unit. 
        /// It requires a collider on the unit game object to work.
        /// UnitHighlighted 이벤트는 사용자가 장치 위로 커서를 이동할 때 호출됩니다.
        /// 작동하려면 유닛 게임 오브젝트에 충돌기가 필요합니다.
        /// </summary>
        public event EventHandler UnitHighlighted;
        /// <summary>
        /// UnitDehighlighted event is invoked when cursor exits unit's collider. 
        /// It requires a collider on the unit game object to work.
        /// UnitDehighlighted 이벤트는 커서가 유닛의 충돌기에서 벗어날 때 호출됩니다.
        /// 작동하려면 유닛 게임 오브젝트에 충돌기가 필요합니다.
        /// </summary>
        public event EventHandler UnitDehighlighted;
        /// <summary>
        /// UnitAttacked event is invoked when the unit is attacked.
        /// 유닛이 공격받았을 때 UnitAttacked 이벤트가 호출됩니다.
        /// </summary>
        public event EventHandler<AttackEventArgs> UnitAttacked;
        /// <summary>
        /// UnitDestroyed event is invoked when unit's hitpoints drop below 0.
        /// UnitDestroyed 이벤트는 유닛의 체력이 0 아래로 떨어지면 호출됩니다.
        /// </summary>
        public event EventHandler<AttackEventArgs> UnitDestroyed;
        /// <summary>
        /// UnitMoved event is invoked when unit moves from one cell to another.
        /// UnitMoved 이벤트는 유닛이 셀 간에 이동할 때 호출됩니다.
        /// </summary>
        public event EventHandler<MovementEventArgs> UnitMoved;

        public UnitHighlighterAggregator UnitHighlighterAggregator;

        public bool Obstructable = true;
        public bool canpush = true;

        public UnitState UnitState { get; set; }
        public void SetState(UnitState state)
        {
            UnitState.MakeTransition(state);
            //Debug.Log(string.Format("{0} - {1}", gameObject, UnitState));
        }
        /// <summary>
        /// A list of buffs that are applied to the unit.
        /// 장치에 적용되는 버프 목록입니다.
        /// </summary>
        private List<(Buff buff, int timeLeft)> Buffs;
        public void AddBuff(Buff buff)
        {
            buff.Apply(this);
            Buffs.Add((buff, buff.Duration));
        }

        public int TotalHitPoints { get; private set; }
        public float TotalMovementPoints { get; private set; }
        public float TotalActionPoints { get; private set; }

        /// <summary>
        /// Cell that the unit is currently occupying.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        private Cell cell;
        public Cell Cell
        {
            get
            {
                return cell;
            }
            set
            {
                cell = value;
            }
        }

        public int HitPoints;
        public int AttackRange;
        public int AttackFactor;
        public int DefenceFactor;
        /// <summary>
        /// Determines how far on the grid the unit can move.
        /// </summary>
        [SerializeField]
        private float movementPoints;
        public virtual float MovementPoints
        {
            get
            {
                return movementPoints;
            }
            protected set
            {
                movementPoints = value;
            }
        }
        /// <summary>
        /// Determines speed of movement animation.
        /// 애니메이션 이동 속도를 결정합니다.
        /// </summary>
        public float MovementAnimationSpeed;
        /// <summary>
        /// Determines how many attacks unit can perform in one turn.
        /// 한 번에 실행할 수 있는 공격 수를 결정합니다.
        /// </summary>
        [SerializeField]
        private float actionPoints = 1;
        public float ActionPoints
        {
            get
            {
                return actionPoints;
            }
            set
            {
                actionPoints = value;
            }
        }

        /// <summary>
        /// Indicates the player that the unit belongs to. 
        /// Should correspoond with PlayerNumber variable on Player script.
        /// 장치가 속한 플레이어를 나타냅니다.
        /// 플레이어 스크립트의 PlayerNumber 변수와 상호 응답해야 합니다.
        /// </summary>
        public int PlayerNumber;

        /// <summary>
        /// Indicates if movement animation is playing.
        /// 이동 애니메이션이 재생 중인지 여부를 나타냅니다.
        /// </summary>
        public bool IsMoving { get; set; }

        private static DijkstraPathfinding _pathfinder = new DijkstraPathfinding();
        private static IPathfinding _fallbackPathfinder = new AStarPathfinding();

        /// <summary>
        /// Method called after object instantiation to initialize fields etc. 
        /// 필드 등을 초기화하기 위해 객체 인스턴스화 후에 호출되는 메서드입니다.
        /// </summary>
        public virtual void Initialize()
        {
            Buffs = new List<(Buff, int)>();

            UnitState = new UnitStateNormal(this);

            TotalHitPoints = HitPoints;
            TotalMovementPoints = MovementPoints;
            TotalActionPoints = ActionPoints;
        }

        public virtual void OnMouseDown()
        {
            if (UnitClicked != null)
            {
                UnitClicked.Invoke(this, new EventArgs());
            }
        }
        protected virtual void OnMouseEnter()
        {
            if (UnitHighlighted != null)
            {
                UnitHighlighted.Invoke(this, new EventArgs());
            }
        }
        protected virtual void OnMouseExit()
        {
            if (UnitDehighlighted != null)
            {
                UnitDehighlighted.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Method is called at the start of each turn.
        /// 각 턴이 시작될 때마다 메서드가 호출됩니다.
        /// </summary>
        public virtual void OnTurnStart()
        {
            cachedPaths = null;

            Buffs.FindAll(b => b.timeLeft == 0).ForEach(b => { b.buff.Undo(this); });
            Buffs.RemoveAll(b => b.timeLeft == 0);
            var name = this.name;
            var state = UnitState;
            SetState(new UnitStateMarkedAsFriendly(this));
        }
        /// <summary>
        /// Method is called at the end of each turn.
        /// 각 턴의 마지막에 메서드가 호출됩니다.
        /// 
        /// </summary>
        public virtual void OnTurnEnd()
        {
            for (int i = 0; i < Buffs.Count; i++)
            {
                (Buff buff, int timeLeft) = Buffs[i];
                Buffs[i] = (buff, timeLeft - 1);
            }

            MovementPoints = TotalMovementPoints;
            ActionPoints = TotalActionPoints;

            SetState(new UnitStateNormal(this));
        }
        /// <summary>
        /// Method is called when units HP drops below 1.
        /// 단위 HP가 1 미만으로 떨어지면 메서드가 호출됩니다.
        /// </summary>
        protected virtual void OnDestroyed()
        {
            Cell.IsTaken = false;
            Cell.CurrentUnits.Remove(this);
            MarkAsDestroyed();
            Destroy(gameObject);
        }

        /// <summary>
        /// Method is called when unit is selected.
        /// </summary>
        public virtual void OnUnitSelected()
        {
            if (FindObjectOfType<CellGrid>().GetCurrentPlayerUnits().Contains(this))
            {
                SetState(new UnitStateMarkedAsSelected(this));
            }
            if (UnitSelected != null)
            {
                UnitSelected.Invoke(this, new EventArgs());
            }
        }
        /// <summary>
        /// Method is called when unit is deselected.
        /// </summary>
        public virtual void OnUnitDeselected()
        {
            if (FindObjectOfType<CellGrid>().GetCurrentPlayerUnits().Contains(this))
            {
                SetState(new UnitStateMarkedAsFriendly(this));
            }
            if (UnitDeselected != null)
            {
                UnitDeselected.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Method indicates if it is possible to attack a unit from given cell.
        /// </summary>
        /// <param name="other">Unit to attack</param>
        /// <param name="sourceCell">Cell to perform an attack from</param>
        /// <returns>Boolean value whether unit can be attacked or not</returns>
        /// 
        public virtual bool IsUnitAttackable(Unit other, Cell sourceCell)
        {
            return IsUnitAttackable(other, other.Cell, sourceCell);
        }
        public virtual bool IsUnitAttackable(Unit other, Cell otherCell, Cell sourceCell)
        {
            return sourceCell.GetDistance(otherCell) <= AttackRange
                && other.PlayerNumber != PlayerNumber
                && ActionPoints >= 1;
        }


        /// <summary>
        /// Method performs an attack on given unit.
        /// </summary>
        public void AttackHandler(Unit unitToAttack)
        {
            AttackAction attackAction = DealDamage(unitToAttack);
            MarkAsAttacking(unitToAttack);
            unitToAttack.DefendHandler(this, attackAction.Damage);
            AttackActionPerformed(attackAction.ActionCost);
        }
        /// <summary>
        /// Method for calculating damage and action points cost of attacking given unit
        /// </summary>
        /// <returns></returns>
        protected virtual AttackAction DealDamage(Unit unitToAttack)
        {
            return new AttackAction(AttackFactor, 1f);
        }
        /// <summary>
        /// Method called after unit performed an attack.
        /// </summary>
        /// <param name="actionCost">Action point cost of performed attack</param>
        protected virtual void AttackActionPerformed(float actionCost)
        {
            ActionPoints -= actionCost;
        }

        /// <summary>
        /// Handler method for defending against an attack.
        /// </summary>
        /// <param name="aggressor">Unit that performed the attack</param>
        /// <param name="damage">Amount of damge that the attack caused</param>
        public void DefendHandler(Unit aggressor, int damage)
        {
            MarkAsDefending(aggressor);
            int damageTaken = Defend(aggressor, damage);
            HitPoints -= damageTaken;
            DefenceActionPerformed();

            if (UnitAttacked != null)
            {
                UnitAttacked.Invoke(this, new AttackEventArgs(aggressor, this, damage));
            }
            if (HitPoints <= 0)
            {
                if (UnitDestroyed != null)
                {
                    UnitDestroyed.Invoke(this, new AttackEventArgs(aggressor, this, damage));
                }
                OnDestroyed();
            }
        }
        /// <summary>
        /// Method for calculating actual damage taken by the unit.
        /// </summary>
        /// <param name="aggressor">Unit that performed the attack</param>
        /// <param name="damage">Amount of damge that the attack caused</param>
        /// <returns>Amount of damage that the unit has taken</returns>        
        protected virtual int Defend(Unit aggressor, int damage)
        {
            return Mathf.Clamp(damage - DefenceFactor, 1, damage);
        }
        /// <summary>
        /// Method callef after unit performed defence.
        /// </summary>
        protected virtual void DefenceActionPerformed() { }

        public int DryAttack(Unit other)
        {
            int damage = DealDamage(other).Damage;
            int realDamage = other.Defend(this, damage);

            return realDamage;
        }

        /// <summary>
        /// Handler method for moving the unit.
        /// </summary>
        /// <param name="destinationCell">Cell to move the unit to</param>
        /// <param name="path">A list of cells, path from source to destination cell</param>
        public virtual void Move(Cell destinationCell, List<Cell> path)
        {
            var totalMovementCost = path.Sum(h => h.MovementCost);
            MovementPoints -= totalMovementCost;

            Cell.IsTaken = false;
            Cell.CurrentUnits.Remove(this);
            Cell = destinationCell;
            destinationCell.IsTaken = true;
            destinationCell.CurrentUnits.Add(this);

            if (MovementAnimationSpeed > 0)
            {
                StartCoroutine(MovementAnimation(path));
            }
            else
            {
                transform.position = Cell.transform.position;
                OnMoveFinished();
            }

            if (UnitMoved != null)
            {
                UnitMoved.Invoke(this, new MovementEventArgs(Cell, destinationCell, path, this));
            }
        }
        protected virtual IEnumerator MovementAnimation(List<Cell> path)
        {
            IsMoving = true;
            for (int i = path.Count - 1; i >= 0; i--)
            {
                var currentCell = path[i];
                Vector3 destination_pos = FindObjectOfType<CellGrid>().Is2D ? new Vector3(currentCell.transform.localPosition.x, currentCell.transform.localPosition.y, transform.localPosition.z) : new Vector3(currentCell.transform.localPosition.x, transform.localPosition.y, currentCell.transform.localPosition.z);
                while (transform.localPosition != destination_pos)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination_pos, Time.deltaTime * MovementAnimationSpeed);
                    yield return 0;
                }
            }

            IsMoving = false;
            OnMoveFinished();
        }
        /// <summary>
        /// Method called after movement animation has finished.
        /// </summary>
        protected virtual void OnMoveFinished() { }

        ///<summary>
        /// Method indicates if unit is capable of moving to cell given as parameter.
        /// </summary>
        public virtual bool IsCellMovableTo(Cell cell)
        {
            return !cell.IsTaken;
        }
        /// <summary>
        /// Method indicates if unit is capable of moving through cell given as parameter.
        /// </summary>
        public virtual bool IsCellTraversable(Cell cell)
        {
            return !cell.IsTaken;
        }
        /// <summary>
        /// Method returns all cells that the unit is capable of moving to.
        /// </summary>
        public HashSet<Cell> GetAvailableDestinations(List<Cell> cells)
        {
            cachedPaths = new Dictionary<Cell, List<Cell>>();

            var paths = CachePaths(cells);
            foreach (var key in paths.Keys)
            {
                if (!IsCellMovableTo(key))
                {
                    continue;
                }
                var path = paths[key];

                var pathCost = path.Sum(c => c.MovementCost);
                if (pathCost <= MovementPoints)
                {
                    cachedPaths.Add(key, path);
                }
            }
            return new HashSet<Cell>(cachedPaths.Keys);
        }

        private Dictionary<Cell, List<Cell>> CachePaths(List<Cell> cells)
        {
            var edges = GetGraphEdges(cells);
            var paths = _pathfinder.findAllPaths(edges, Cell);
            return paths;
        }

        public List<Cell> FindPath(List<Cell> cells, Cell destination)
        {
            if (cachedPaths != null && cachedPaths.ContainsKey(destination))
            {
                return cachedPaths[destination];
            }
            else
            {
                return _fallbackPathfinder.FindPath(GetGraphEdges(cells), Cell, destination);
            }
        }
        /// <summary>
        /// Method returns graph representation of cell grid for pathfinding.
        /// </summary>
        protected virtual Dictionary<Cell, Dictionary<Cell, float>> GetGraphEdges(List<Cell> cells)
        {
            Dictionary<Cell, Dictionary<Cell, float>> ret = new Dictionary<Cell, Dictionary<Cell, float>>();
            foreach (var cell in cells)
            {
                if (IsCellTraversable(cell) || cell.Equals(Cell))
                {
                    ret[cell] = new Dictionary<Cell, float>();
                    foreach (var neighbour in cell.GetNeighbours(cells).FindAll(IsCellTraversable))
                    {
                        ret[cell][neighbour] = neighbour.MovementCost;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Gives visual indication that the unit is under attack.
        /// </summary>
        /// <param name="aggressor">
        /// Unit that is attacking.
        /// </param>
        public virtual void MarkAsDefending(Unit aggressor)
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsDefendingFn?.ForEach(o => o.Apply(this, aggressor));
            }
        }
        /// <summary>
        /// Gives visual indication that the unit is attacking.
        /// </summary>
        /// <param name="target">
        /// Unit that is under attack.
        /// </param>
        public virtual void MarkAsAttacking(Unit target)
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsAttackingFn?.ForEach(o => o.Apply(this, target));
            }
        }
        /// <summary>
        /// Gives visual indication that the unit is destroyed. It gets called right before the unit game object is
        /// destroyed.
        /// </summary>
        public virtual void MarkAsDestroyed()
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsDestroyedFn?.ForEach(o => o.Apply(this, null));
            }
        }

        /// <summary>
        /// Method marks unit as current players unit.
        /// </summary>
        public virtual void MarkAsFriendly()
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsFriendlyFn?.ForEach(o => o.Apply(this, null));
            }
        }
        /// <summary>
        /// Method mark units to indicate user that the unit is in range and can be attacked.
        /// </summary>
        public virtual void MarkAsReachableEnemy()
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsReachableEnemyFn?.ForEach(o => o.Apply(this, null));
            }
        }
        /// <summary>
        /// Method marks unit as currently selected, to distinguish it from other units.
        /// </summary>
        public virtual void MarkAsSelected()
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsSelectedFn?.ForEach(o => o.Apply(this, null));
            }
        }
        /// <summary>
        /// Method marks unit to indicate user that he can't do anything more with it this turn.
        /// </summary>
        public virtual void MarkAsFinished()
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsFinishedFn?.ForEach(o => o.Apply(this, null));
            }
        }
        /// <summary>
        /// Method returns the unit to its base appearance
        /// </summary>
        public virtual void UnMark()
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.UnMarkFn?.ForEach(o => o.Apply(this, null));
            }
        }
        public virtual void SetColor(Color color) { }

        [ExecuteInEditMode]
        public void OnDestroy()
        {
            if (Cell != null)
            {
                Cell.IsTaken = false;
            }
        }

        private void Reset()
        {
            if (GetComponent<Abilities.AttackAbility>() == null)
            {
                gameObject.AddComponent<Abilities.AttackAbility>();
            }
            if (GetComponent<Abilities.MoveAbility>() == null)
            {
                gameObject.AddComponent<Abilities.MoveAbility>();
            }

            GameObject brain = new GameObject("Brain");
            brain.transform.parent = transform;

            brain.AddComponent<MoveToPositionAIAction>();
            brain.AddComponent<AttackAIAction>();

            brain.AddComponent<DamageCellEvaluator>();
            brain.AddComponent<DamageUnitEvaluator>();
        }
    }

    public class AttackAction
    {
        public readonly int Damage;
        public readonly float ActionCost;

        public AttackAction(int damage, float actionCost)
        {
            Damage = damage;
            ActionCost = actionCost;
        }
    }

    public class MovementEventArgs : EventArgs
    {
        public Cell OriginCell;
        public Cell DestinationCell;
        public List<Cell> Path;
        public Unit Unit;

        public MovementEventArgs(Cell sourceCell, Cell destinationCell, List<Cell> path, Unit unit)
        {
            OriginCell = sourceCell;
            DestinationCell = destinationCell;
            Path = path;
            Unit = unit;
        }
    }
    public class AttackEventArgs : EventArgs
    {
        public Unit Attacker;
        public Unit Defender;

        public int Damage;

        public AttackEventArgs(Unit attacker, Unit defender, int damage)
        {
            Attacker = attacker;
            Defender = defender;

            Damage = damage;
        }
    }
    public class UnitCreatedEventArgs : EventArgs
    {
        public Transform unit;

        public UnitCreatedEventArgs(Transform unit)
        {
            this.unit = unit;
        }
    }
}
