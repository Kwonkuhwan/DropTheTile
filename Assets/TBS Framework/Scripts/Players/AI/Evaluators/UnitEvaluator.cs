using TbsFramework.Grid;
using TbsFramework.Units;
using UnityEngine;

namespace TbsFramework.Players.AI.Evaluators
{
    public abstract class UnitEvaluator : MonoBehaviour
    {
        public float Weight = 1;
        public abstract float Evaluate(Unit unitToEvaluate, Unit evaluatingUnit, Player currentPlayer, CellGrid cellGrid);
    }
}
