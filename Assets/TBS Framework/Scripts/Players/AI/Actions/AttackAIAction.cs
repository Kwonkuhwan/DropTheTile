using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Players.AI.Evaluators;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine;

namespace TbsFramework.Players.AI.Actions
{
    public class AttackAIAction : AIAction
    {
        private Unit Target;
        private Dictionary<Unit, string> unitDebugInfo;
        private List<(Unit unit, float value)> unitScores;

        public override bool ShouldExecute(Player player, Unit unit, CellGrid cellGrid)
        {
            if (unit.GetComponent<AttackAbility>() == null)
            {
                return false;
            }

            var enemyUnits = cellGrid.GetEnemyUnits(player);
            var isEnemyinRange = enemyUnits.Select(u => unit.IsUnitAttackable(u, unit.Cell))
                                           .Aggregate((result, next) => result || next);

            return isEnemyinRange && unit.ActionPoints > 0;
        }
        public override void Precalculate(Player player, Unit unit, CellGrid cellGrid)
        {
            var enemyUnits = cellGrid.GetEnemyUnits(player);
            var enemiesInRange = enemyUnits.Where(e => unit.IsUnitAttackable(e, unit.Cell))
                                           .ToList();

            unitDebugInfo = new Dictionary<Unit, string>();
            enemyUnits.ForEach(u => unitDebugInfo[u] = "");

            if (enemiesInRange.Count == 0)
            {
                return;
            }

            var evaluators = GetComponents<UnitEvaluator>();
            unitScores = enemiesInRange.Select(u => (unit: u, value: evaluators.Select(e =>
            {
                var score = e.Evaluate(u, unit, player, cellGrid);
                var weightedScore = score * e.Weight;
                unitDebugInfo[u] += string.Format("{0:+0.00;-0.00} * {1:+0.00;-0.00} = {2:+0.00;-0.00} : {3}\n", e.Weight, score, weightedScore, e.GetType().ToString());

                return weightedScore;
            }).DefaultIfEmpty(0f).Aggregate((result, next) => result + next))).ToList();
            unitScores.ToList().ForEach(s => unitDebugInfo[s.unit] += string.Format("Total: {0:0.00}", s.value));

            var (topUnit, maxValue) = unitScores.OrderByDescending(o => o.value)
                                                .First();

            Target = topUnit;
        }
        public override IEnumerator Execute(Player player, Unit unit, CellGrid cellGrid)
        {
            unit.GetComponent<AttackAbility>().UnitToAttack = Target;
            unit.GetComponent<AttackAbility>().AIExecute(cellGrid);
            yield return new WaitForSeconds(0.5f);
        }
        public override void CleanUp(Player player, Unit unit, CellGrid cellGrid)
        {
            foreach (var enemy in cellGrid.GetEnemyUnits(player))
            {
                enemy.UnMark();
            }
            Target = null;
            unitScores = null;
        }
        public override void ShowDebugInfo(Player player, Unit unit, CellGrid cellGrid)
        {
            (cellGrid.CellGridState as CellGridStateAITurn).UnitDebugInfo = unitDebugInfo;

            if (unitScores == null)
            {
                return;
            }

            var minScore = unitScores.DefaultIfEmpty().Min(e => e.value);
            var maxScore = unitScores.DefaultIfEmpty().Max(e => e.value);
            foreach (var (u, value) in unitScores)
            {
                var color = Color.Lerp(Color.red, Color.green, value >= 0 ? value / maxScore : value / minScore * (-1));
                u.SetColor(color);
            }

            if (Target != null)
            {
                Target.SetColor(Color.blue);
            }
        }
    }
}