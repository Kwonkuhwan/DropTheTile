using TbsFramework.Cells;
using UnityEngine;

namespace TbsFramework.Example1
{
    class MyHexagon : Hexagon
    {
        private Vector3 dimensions = new Vector3(2.2f, 1.1f, 1.9f);

        public override Vector3 GetCellDimensions()
        {
            return dimensions;
        }

        public override void SetColor(Color color)
        {
            GetComponent<Renderer>().material.color = color;
        }
    }
}
