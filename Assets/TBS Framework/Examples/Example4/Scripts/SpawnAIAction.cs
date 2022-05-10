using System.Collections;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Players.AI.Actions;
using TbsFramework.Units;
using UnityEngine;

namespace TbsFramework.Example4
{
    public class SpawnAIAction : AIAction
    {
        public override bool ShouldExecute(Player player, Unit unit, CellGrid cellGrid)
        {
            return unit.GetComponent<SpawnAbility>() != null;
        }

        public override void CleanUp(Player player, Unit unit, CellGrid cellGrid)
        {
        }

        public override IEnumerator Execute(Player player, Unit unit, CellGrid cellGrid)
        {
            var availableUnits = unit.GetComponent<SpawnAbility>().Prefabs;
            foreach (var _unit in availableUnits)
            {
                var shouldSpawn = _unit.GetComponentsInChildren<SpawnCondition>()
                                       .Select(c => c.ShouldSpawn(cellGrid, _unit.GetComponent<Unit>(), player))
                                       .Aggregate((result, next) => result || next);
                if (shouldSpawn)
                {
                    unit.GetComponent<SpawnAbility>().SelectedPrefab = _unit;
                    unit.GetComponent<SpawnAbility>().AIExecute(cellGrid);
                    break;
                }
            }
            yield return new WaitForSeconds(0.075f);
        }

        public override void Precalculate(Player player, Unit unit, CellGrid cellGrid)
        {

        }

        public override void ShowDebugInfo(Player player, Unit unit, CellGrid cellGrid)
        {
        }
    }
}
