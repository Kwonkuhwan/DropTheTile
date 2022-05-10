using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Players.AI.Evaluators;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine;

namespace TbsFramework.Players.AI.Actions
{
    public class MoveToPositionAIAction : AIAction
    {
        public bool ShouldMoveAllTheWay = true;
        private Cell TopDestination = null;

        private Dictionary<Cell, string> cellMetadata;
        private IEnumerable<(Cell cell, float value)> cellScores;
        private Dictionary<Cell, float> cellScoresDict;

        private Gradient DebugGradient;

        private void Awake()
        {
            var colorKeys = new GradientColorKey[3];

            colorKeys[0] = new GradientColorKey(Color.red, 0.2f);
            colorKeys[1] = new GradientColorKey(Color.yellow, 0.5f);
            colorKeys[2] = new GradientColorKey(Color.green, 0.8f);

            DebugGradient = new Gradient();
            DebugGradient.SetKeys(colorKeys, new GradientAlphaKey[0]);
        }

        public override bool ShouldExecute(Player player, Unit unit, CellGrid cellGrid)
        {
            if (unit.GetComponent<MoveAbility>() == null)
            {
                return false;
            }

            cellMetadata = new Dictionary<Cell, string>();
            cellScoresDict = new Dictionary<Cell, float>();
            cellGrid.Cells.ForEach(c => cellMetadata[c] = "");


            var evaluators = GetComponents<CellEvaluator>();
            foreach (var e in evaluators)
            {
                e.Precalculate(unit, player, cellGrid);
            }
            cellScores = cellGrid.Cells.Select(c => (cell: c, value: evaluators.Select(e =>
            {
                var score = e.Evaluate(c, unit, player, cellGrid);
                var weightedScore = score * e.Weight;
                cellMetadata[c] += string.Format("{0:+0.00;-0.00} * {1:+0.00;-0.00} = {2:+0.00;-0.00} : {3}\n", e.Weight, score, weightedScore, e.GetType().ToString());

                return weightedScore;
            }).DefaultIfEmpty(0f).Aggregate((result, next) => result + next))).OrderByDescending(x => x.value).ToList();

            cellScores.ToList().ForEach(s =>
            {
                cellMetadata[s.cell] += string.Format("Total: {0:0.00}", s.value);
                cellScoresDict[s.cell] = s.value;
            });

            var (topCell, maxValue) = cellScores.Where(o => unit.IsCellMovableTo(o.cell))
                                                .OrderByDescending(o => o.value)
                                                .First();

            var currentCellVal = evaluators.Select(e => e.Weight * e.Evaluate(unit.Cell, unit, player, cellGrid))
                                           .DefaultIfEmpty(0f)
                                           .Aggregate((result, next) => result += next);

            if (maxValue > currentCellVal)
            {
                TopDestination = topCell;
                return true;
            }

            TopDestination = unit.Cell;
            return false;
        }
        public override void Precalculate(Player player, Unit unit, CellGrid cellGrid)
        {
            var path = unit.FindPath(cellGrid.Cells, TopDestination);
            List<Cell> selectedPath = new List<Cell>();
            float cost = 0;

            for (int i = path.Count - 1; i >= 0; i--)
            {
                var cell = path[i];
                cost += cell.MovementCost;
                if (cost <= unit.MovementPoints)
                {
                    selectedPath.Add(cell);
                }
                else
                {
                    for (int j = selectedPath.Count - 1; j >= 0; j--)
                    {
                        if (!unit.IsCellMovableTo(selectedPath[j]))
                        {
                            selectedPath.RemoveAt(j);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                }
            }
            selectedPath.Reverse();

            if (selectedPath.Count != 0)
            {
                TopDestination = ShouldMoveAllTheWay ? selectedPath[0] : selectedPath.OrderByDescending(c => cellScoresDict[c]).First();
            }
        }
        public override IEnumerator Execute(Player player, Unit unit, CellGrid cellGrid)
        {
            unit.GetComponent<MoveAbility>().Destination = TopDestination;
            unit.GetComponent<MoveAbility>().AIExecute(cellGrid);
            while (unit.IsMoving)
            {
                yield return 0;
            }
        }
        public override void CleanUp(Player player, Unit unit, CellGrid cellGrid)
        {
            foreach (var cell in cellGrid.Cells)
            {
                cell.UnMark();
            }
            TopDestination = null;
            (cellGrid.CellGridState as CellGridStateAITurn).CellDebugInfo = null;
        }
        public override void ShowDebugInfo(Player player, Unit unit, CellGrid cellGrid)
        {
            Dictionary<Cell, DebugInfo> cellDebugInfo = new Dictionary<Cell, DebugInfo>();

            var maxScore = cellScores.Max(x => x.value);
            var minScore = cellScores.Min(x => x.value);

            for (int i = 0; i < cellScores.Count(); i++)
            {
                var (cell, value) = cellScores.ElementAt(i);
                var color = i == 0 ? Color.blue : DebugGradient.Evaluate((value - minScore) / (Mathf.Abs(maxScore - minScore) + float.Epsilon));
                cellDebugInfo[cell] = new DebugInfo(cellMetadata[cell], color);
            }

            cellDebugInfo[TopDestination].Color = Color.magenta;
            (cellGrid.CellGridState as CellGridStateAITurn).CellDebugInfo = cellDebugInfo;
        }
    }
}
