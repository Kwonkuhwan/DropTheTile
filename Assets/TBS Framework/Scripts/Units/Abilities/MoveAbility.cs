using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using UnityEngine;

namespace TbsFramework.Units.Abilities
{
    public class MoveAbility : Ability 
    {
        public Cell Destination { get; set; }
        private List<Cell> currentPath;
        private HashSet<Cell> pathsInRange;


        public override IEnumerator Act(CellGrid cellGrid)
        {
            if (UnitReference.ActionPoints > 0 && UnitReference.GetAvailableDestinations(cellGrid.Cells).Contains(Destination))
            {
                var path = UnitReference.FindPath(cellGrid.Cells, Destination);
                UnitReference.Move(Destination, path);
                while (UnitReference.IsMoving)
                {
                    yield return 0;
                }
            }
            yield return 0;
            Debug.Log("move-ACT함수");
        }


        public override void Display(CellGrid cellGrid)
        {
            if (CanPerform(cellGrid))
            {
                pathsInRange = UnitReference.GetAvailableDestinations(cellGrid.Cells);
                foreach (var cell in UnitReference.GetAvailableDestinations(cellGrid.Cells))
                {
                    cell.MarkAsReachable();
                }
            }
            Debug.Log("move-Display함수");
        }

        public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            if (cellGrid.GetCurrentPlayerUnits().Contains(unit))
            {
                cellGrid.CellGridState = new CellGridStateAbilitySelected(cellGrid, unit, unit.GetComponents<Ability>().ToList());
            }
        }
        public override void OnCellClicked(Cell cell, CellGrid cellGrid)
        {
            if (UnitReference.GetAvailableDestinations(cellGrid.Cells).Contains(cell))
            {
                Destination = cell;
                HumanExecute(cellGrid);
            }
            else
            {
                cellGrid.CellGridState = new CellGridStateWaitingForInput(cellGrid);
            }
        }
        public override void OnCellSelected(Cell cell, CellGrid cellGrid)
        {
            if (CanPerform(cellGrid) && UnitReference.GetAvailableDestinations(cellGrid.Cells).Contains(cell))
            {
                currentPath = UnitReference.FindPath(cellGrid.Cells, cell);
                currentPath.ForEach(u => u.MarkAsPath());
            }
        }
        public override void OnCellDeselected(Cell cell, CellGrid cellGrid)
        {
            if (CanPerform(cellGrid) && UnitReference.GetAvailableDestinations(cellGrid.Cells).Contains(cell))
            {
                currentPath?.ForEach(u =>
                {
                    if (UnitReference.IsCellMovableTo(u))
                    {
                        u.MarkAsReachable();
                    }
                    else
                    {
                        u.UnMark();
                    }
                });
            }
        }

        public override void CleanUp(CellGrid cellGrid)
        {
            if (pathsInRange != null)
            {
                foreach (var cell in pathsInRange)
                {
                    cell.UnMark();
                }
            }
        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            return UnitReference.ActionPoints > 0 && UnitReference.GetAvailableDestinations(cellGrid.Cells).Count > 0;
        }
    }
}
