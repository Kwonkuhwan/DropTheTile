using System;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Players;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


namespace TbsFramework.Gui
{
    public class TileValue : MonoBehaviour
    {
        Rigidbody myRigid;
        public GameObject cube;
        public Color[] cubeColors;
        public int tileHealth;
        private bool isStick;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (!isStick)
                {
                    ChangeTileColor();
                    --tileHealth;
                }
                AddRigid();
            }
        }


        //private void ChangeTileColor()
        //{
        //    if (tileHealth == 5)
        //    {
        //        cube.transform.po
        //    }
        //}

        private void ChangeTileColor()
        {
            if (tileHealth == 5)
            {
                cube.GetComponent<MeshRenderer>().material.color = cubeColors[0];
            }
            else if (tileHealth == 4)
            {
                cube.GetComponent<MeshRenderer>().material.color = cubeColors[1];
            }
            else if (tileHealth == 3)
            {
                cube.GetComponent<MeshRenderer>().material.color = cubeColors[2];
            }
            else if (tileHealth == 2)
            {
                cube.GetComponent<MeshRenderer>().material.color = cubeColors[3];
            }
        }

        private void AddRigid()
        {
            if (tileHealth <= 0)
            {
                cube.AddComponent<Rigidbody>();
                Debug.Log("¾È³ç?");
            }
        }
    }
}
