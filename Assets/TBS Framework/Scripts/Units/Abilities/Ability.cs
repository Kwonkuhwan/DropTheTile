using System;
using System.Collections;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using UnityEngine;
using Photon.Pun;

namespace TbsFramework.Units.Abilities
{
    public abstract class Ability : MonoBehaviourPun
    {
        //Reference to the unit that the ability is attached to
        public Unit UnitReference { get; internal set; }

        //일어나자마자 UnitReference는 Unit클래스를 참조한다.
        protected virtual void Awake()
        {
            UnitReference = GetComponent<Unit>();
        }

        //클래스를 실행하다 
        public void Execute(CellGrid cellGrid, Action<CellGrid> preAction, Action<CellGrid> postAction)
        {
            StartCoroutine(Act(cellGrid, preAction, postAction));
        }

        public void HumanExecute(CellGrid cellGrid)
        {
            Execute(cellGrid,
                _ => cellGrid.CellGridState = new CellGridStateBlockInput(cellGrid),
                _ => cellGrid.CellGridState = new CellGridStateAbilitySelected(cellGrid, UnitReference, UnitReference.GetComponents<Ability>().ToList()));
        }

        public void AIExecute(CellGrid cellGrid)
        {
            Execute(cellGrid, _ => { }, _ => { });
        }


        public virtual IEnumerator Act(CellGrid cellGrid) { yield return 0; }

        private IEnumerator Act(CellGrid cellGrid, Action<CellGrid> preAction, Action<CellGrid> postAction)
        {
            preAction(cellGrid);
            yield return StartCoroutine(Act(cellGrid));
            postAction(cellGrid);

            yield return 0;
        }

        public virtual void OnUnitClicked(Unit unit, CellGrid cellGrid) { }
        public virtual void OnUnitHighlighted(Unit unit, CellGrid cellGrid) { }
        public virtual void OnUnitDehighlighted(Unit unit, CellGrid cellGrid) { }
        public virtual void OnUnitDestroyed(CellGrid cellGrid) { }
        public virtual void OnCellClicked(Cell cell, CellGrid cellGrid) { }
        public virtual void OnCellSelected(Cell cell, CellGrid cellGrid) { }
        public virtual void OnCellDeselected(Cell cell, CellGrid cellGrid) { }
        public virtual void Display(CellGrid cellGrid) { }
        public virtual void CleanUp(CellGrid cellGrid) { }

        public virtual void OnAbilitySelected(CellGrid cellGrid) { }
        public virtual void OnAbilityDeselected(CellGrid cellGrid) { }
        public virtual void OnTurnStart(CellGrid cellGrid) { }
        public virtual void OnTurnEnd(CellGrid cellGrid) { }

        public virtual bool CanPerform(CellGrid cellGrid) { return false; }
    }
}
