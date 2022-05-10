using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Units;


namespace TbsFramework.Players.AI.Evaluators
{
    public class DamageUnitEvaluator : UnitEvaluator
    {
        public override float Evaluate(Unit unitToEvaluate, Unit evaluatingUnit, Player currentPlayer, CellGrid cellGrid)
        {
            var enemyUnits = cellGrid.GetEnemyUnits(currentPlayer);
            var enemiesInRange = enemyUnits.Where(u => evaluatingUnit.Cell.GetDistance(u.Cell) <= evaluatingUnit.AttackRange);
            var topDamage = enemiesInRange.Select(u => evaluatingUnit.DryAttack(u))
                                          .DefaultIfEmpty()
                                          .Max();

            var score = (float)evaluatingUnit.DryAttack(unitToEvaluate) / (float)topDamage;
            return score;
        }
    }
}
