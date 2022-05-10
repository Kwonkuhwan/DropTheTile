using System;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells.Highlighters;
using TbsFramework.Pathfinding.DataStructs;
using TbsFramework.Units;
using UnityEngine;
using Photon.Pun;



namespace TbsFramework.Cells
{
    /// <summary>
    /// Class representing a single field (cell) on the grid.
    /// 그리드의 단일 필드(셀)를 나타내는 클래스입니다.
    /// </summary>
    ///
    [Serializable]
    public abstract class Cell : MonoBehaviourPun , IGraphNode, IEquatable<Cell>
    {
        [HideInInspector]
        [SerializeField]
        private Vector2 _offsetCoord;
        /// <summary>
        /// Position of the cell on the grid.
        /// 그리드에서 셀의 위치입니다.
        /// </summary>
        public Vector2 OffsetCoord { get { return _offsetCoord; } set { _offsetCoord = value; } }

        public List<CellHighlighter> MarkAsReachableFn;
        public List<CellHighlighter> MarkAsPathFn;
        public List<CellHighlighter> MarkAsHighlightedFn;
        public List<CellHighlighter> UnMarkFn;

        /// <summary>
        /// Indicates if something is occupying the cell.셀을 점유하고 있는지 여부를 나타냅니다.
        /// </summary>
        public bool IsTaken;
        /// <summary>
        /// Cost of moving through the cell. 셀을 통과하는 비용입니다.
        /// </summary>
        public float MovementCost = 1;

        public List<Unit> CurrentUnits { get; private set; } = new List<Unit>();

        /// <summary>
        /// CellClicked event is invoked when user clicks on the cell. 
        /// It requires a collider on the cell game object to work.
        /// 셀 클릭 이벤트는 사용자가 셀을 클릭할 때 호출됩니다.
        /// 셀 게임 오브젝트에 충돌기가 있어야 작동합니다.
        /// </summary>
        public event EventHandler CellClicked;
        /// <summary>
        /// CellHighlighed event is invoked when cursor enters the cell's collider. 
        /// It requires a collider on the cell game object to work.
        /// 셀 강조 표시 이벤트는 커서가 셀의 충돌기에 들어갈 때 호출됩니다.
        /// 셀 게임 오브젝트에 충돌기가 있어야 작동합니다.
        /// </summary>
        public event EventHandler CellHighlighted;
        /// <summary>
        /// CellDehighlighted event is invoked when cursor exits the cell's collider. 
        /// It requires a collider on the cell game object to work.
        /// CellDe-highlighted 이벤트는 커서가 셀의 충돌기를 벗어날 때 호출됩니다.
        /// 셀 게임 오브젝트에 충돌기가 있어야 작동합니다.
        /// </summary>
        public event EventHandler CellDehighlighted;

        protected virtual void OnMouseEnter()
        {
            if (CellHighlighted != null)
                CellHighlighted.Invoke(this, new EventArgs());
        }
        protected virtual void OnMouseExit()
        {
            if (CellDehighlighted != null)
                CellDehighlighted.Invoke(this, new EventArgs());
        }
        protected virtual void OnMouseDown()
        {
            if (CellClicked != null)
                CellClicked.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Method returns distance to a cell that is given as parameter. 
        /// </summary>
        public abstract int GetDistance(Cell other);

        /// <summary>
        /// Method returns cells adjacent to current cell, from list of cells given as parameter.
        /// </summary>
        public abstract List<Cell> GetNeighbours(List<Cell> cells);
        /// <summary>
        /// Method returns cell's physical dimensions It is used in grid generators.
        /// </summary>
        public abstract Vector3 GetCellDimensions();

        /// <summary>
        ///  Method marks the cell to give user an indication that selected unit can reach it.
        /// </summary>
        public virtual void MarkAsReachable()
        {
            MarkAsReachableFn?.ForEach(o => o.Apply(this));
        }
        /// <summary>
        /// Method marks the cell as a part of a path.
        /// </summary>
        public virtual void MarkAsPath()
        {
            MarkAsPathFn?.ForEach(o => o.Apply(this));
        }
        /// <summary>
        /// Method marks the cell as highlighted. It gets called when the mouse is over the cell.
        /// </summary>
        public virtual void MarkAsHighlighted()
        {
            MarkAsHighlightedFn?.ForEach(o => o.Apply(this));
        }
        /// <summary>
        /// Method returns the cell to its base appearance.
        /// </summary>
        public virtual void UnMark()
        {
            UnMarkFn?.ForEach(o => o.Apply(this));
        }
        public virtual void SetColor(Color color) { }

        public int GetDistance(IGraphNode other)
        {
            return GetDistance(other as Cell);
        }

        public virtual bool Equals(Cell other)
        {
            return (OffsetCoord.x == other.OffsetCoord.x && OffsetCoord.y == other.OffsetCoord.y);
        }

        public override bool Equals(object other)
        {
            if (!(other is Cell))
                return false;

            return Equals(other as Cell);
        }

        public override int GetHashCode()
        {
            int hash = 23;

            hash = (hash * 37) + (int)OffsetCoord.x;
            hash = (hash * 37) + (int)OffsetCoord.y;
            return hash;
        }

        /// <summary>
        /// Method for cloning field values into a new cell. Used in Tile Painter in Grid Helper
        /// </summary>
        /// <param name="newCell">Cell to copy field values to</param>
        public abstract void CopyFields(Cell newCell);

        public override string ToString()
        {
            return OffsetCoord.ToString();
        }
    }
}