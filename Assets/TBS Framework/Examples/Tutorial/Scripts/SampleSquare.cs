using System;
using System.Collections.Generic;
using TbsFramework.Cells;
using TbsFramework.Units;
using UnityEngine;
using Photon.Pun;

namespace TbsFramework.Tutorial
{
    class SampleSquare : Square
    {
        //바꿀 색깔의 배열
     
        public override Vector3 GetCellDimensions()
        {
            return GetComponent<Renderer>().bounds.size;
        }
        public override void MarkAsHighlighted()
        {
            GetComponent<Renderer>().material.color = new Color(0.75f, 0.75f, 0.75f);
        }
        //녹색
        public override void MarkAsPath()
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        //노란색
        public override void MarkAsReachable()
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }

        //표시 안함 == 하얀색
        public override void UnMark()
        {
            GetComponent<Renderer>().material.color = Color.white;
        }

    }
}
        //플레이어 태그를 거칠 경우에 포문으로 색깔을 바꿔준다.
        // 그리고 그리고 5번 이상 색깔을 바꿔주고
        // 다 사용했을경우 해당 큐브를 없애고 해당 Unit에 중력값 1000만

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.tag == "Player")  //isTrigger 해제할 경우 잘뜨는 Debug
        //    {//mr의 MeshRender값에 material배열을 넣어준다.
        //        Debug.Log("sample");
                
        //        if (Input.GetKeyDown("k"))
                    
        //            mr.material.color = Color.black; // 검정색
        //        //MeshRenderer mr = this.transform.GetComponent<MeshRenderer>();
        //        //mr.material = Resources.Load("MY_Resources/Material/1") typeof(Material)) as Material;
        //        //Resources.Load("3DModels/Materials/04 - green", typeof(Material)) as Material;
                //for (int i = 0; i < lastCount.Length; i++)
   


            //   square.GetComponent<Material>();

            //    for (int i = 0; i < materials.Count; i++)
            //    {
            //        square[i];

            //    if (color.Length < Max_Count)
            //    {
            //        Destroy(this.gameObject);
            //        blockRD.AddForce(new Vector3(0, -100, 0), ForceMode.Impulse);
            //    }
            //}

