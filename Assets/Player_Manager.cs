using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.IO;//Path사용위에 사용

public class Player_Manager : MonoBehaviourPunCallbacks
{ 

//{
//    public GameObject playerPrefab;
//    PhotonView PV;//포톤뷰 선언

//    void Awake()
//    {
//       // PV = GetComponent<PhotonView>();
//        Debug.Log("photon awake");
//    }

//    private void Start()
//    {
        
//        if (playerPrefab == null)
//        {
//           Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
//        }
//        else
//        {
//         Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
//         // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
//         PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
//        }
//       /* if (PV.IsMine)//내 포톤 네트워크이면
//        {
//            CreateController();//플레이어 컨트롤러 붙여준다. 
//        }*/
//    }
//   /* void CreateController()//플레이어 컨트롤러 만들기
//    {
//        Debug.Log("Instantiated Player");
//        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
//        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
//    }*/
    
    

}