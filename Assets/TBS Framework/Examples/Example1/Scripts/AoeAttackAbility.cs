using System.Collections;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Units.Abilities;

namespace TbsFramework.Example1
{
    public class AoeAttackAbility : Ability
    {
        public int Range;
        public Cell Target { get; set; }

        public override IEnumerator Act(CellGrid cellGrid)
        {
            var cellsInRange = cellGrid.Cells.Where(c => Target.GetDistance(c) <= Range).ToList();
            var enemiesInRange = cellsInRange.Where(c => c.CurrentUnits.Count > 0).SelectMany(c => c.CurrentUnits).ToList();

            enemiesInRange.ForEach(u =>
            {
                UnitReference.AttackHandler(u);
            });

            yield return 0;
        }

        public override void OnCellClicked(Cell cell, CellGrid cellGrid)
        {
            Target = cell;
            HumanExecute(cellGrid);
        }
        public override void OnCellSelected(Cell cell, CellGrid cellGrid)
        {
            var cellsInRange = cellGrid.Cells.Where(c => cell.GetDistance(c) <= Range).ToList();
            cellsInRange.ForEach(c => c.MarkAsHighlighted());
        }
        public override void OnCellDeselected(Cell cell, CellGrid cellGrid)
        {
            var cellsInRange = cellGrid.Cells.Where(c => cell.GetDistance(c) <= Range).ToList();
            cellsInRange.ForEach(c => c.UnMark());
        }

    }
}