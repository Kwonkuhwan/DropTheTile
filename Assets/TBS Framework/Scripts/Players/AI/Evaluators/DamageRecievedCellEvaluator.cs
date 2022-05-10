using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Units;

namespace TbsFramework.Players.AI.Evaluators
{
    public class DamageRecievedCellEvaluator : CellEvaluator
    {
        public override float Evaluate(Cell cellToEvaluate, Unit evaluatingUnit, Player currentPlayer, CellGrid cellGrid)
        {
            var enemyUnits = cellGrid.GetEnemyUnits(currentPlayer);
            var totalDamage = enemyUnits.Select(u =>
            {
                var isAttackable = 0f;
                var distanceModifier = 0f;
                var damage = u.DryAttack(evaluatingUnit);

                var cellsInMovementRange = cellGrid.Cells.Where(c => u.Cell.GetDistance(c) <= u.MovementPoints && u.IsCellMovableTo(c));
                foreach (var c in cellsInMovementRange)
                {
                    if (u.IsUnitAttackable(evaluatingUnit, cellToEvaluate, c))
                    {
                        isAttackable = 1f;
                        distanceModifier = UnityEngine.Mathf.Pow(1f / (float)u.Cell.GetDistance(cellToEvaluate), 2);
                        break;
                    }
                }
                return isAttackable * damage * distanceModifier;
            }).Aggregate((result, next) => result + next);

            return (float)evaluatingUnit.HitPoints - totalDamage <= 0 ? -1 : (float)totalDamage / (float)evaluatingUnit.HitPoints * (-1);
        }
    }
}
