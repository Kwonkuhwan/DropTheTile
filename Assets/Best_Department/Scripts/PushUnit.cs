using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using TbsFramework.Cells;
using TbsFramework.Pathfinding.Algorithms;
using TbsFramework.Units.Highlighters;
using TbsFramework.Units.UnitStates;
using TbsFramework.Grid;
using TbsFramework.Players.AI.Actions;
using TbsFramework.Players.AI.Evaluators;
using Photon.Pun;
/// <summary>
/// �б��� �Ŀ�
/// ü���� ��� - ü���� ���� ���Ѵ�� �س���- ���� �̻��Ѱ� ����������
/// 
/// </summary>
namespace TbsFramework.Units
{
    public class PushUnit : MonoBehaviour
    {
        public static PushUnit Inst { get; private set; }
        public Unit unit;
        public Rigidbody pushRigidbody;
        public GameObject cube;

        private void Awake()
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }

        [SerializeField] private float pushPower;
        [SerializeField] private float pushTime;

        private bool isPushed = false;

        private void Start()
        {
            cube.gameObject.GetComponent<Rigidbody>();
        }

        //Method indicates if it is possible to attack a unit from given cell.
        /// </summary>
        /// <param name="other">Unit to attack</param>
        /// <param name="sourceCell">Cell to perform an attack from</param>
        /// <returns>Boolean value whether unit can be attacked or not</returns>

        private void PushPlayer()
        {
            if (!isPushed)
            {
                        
            }
        }


    }
}
