using System.Collections.Generic;
using TbsFramework.Grid;
using TbsFramework.Units;

namespace TbsFramework.Players.AI
{
    public class SubsequentUnitSelection : UnitSelection
    {
        public override IEnumerable<Unit> SelectNext(List<Unit> units, CellGrid cellGrid)
        {
            foreach (var unit in units)
            {
                yield return unit;
            }
        }
    }
}

