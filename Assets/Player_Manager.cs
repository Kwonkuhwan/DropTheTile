using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.IO;//Path������� ���

public class Player_Manager : MonoBehaviourPunCallbacks
{ 

//{
//    public GameObject playerPrefab;
//    PhotonView PV;//����� ����

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
//       /* if (PV.IsMine)//�� ���� ��Ʈ��ũ�̸�
//        {
//            CreateController();//�÷��̾� ��Ʈ�ѷ� �ٿ��ش�. 
//        }*/
//    }
//   /* void CreateController()//�÷��̾� ��Ʈ�ѷ� �����
//    {
//        Debug.Log("Instantiated Player");
//        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
//        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
//    }*/
    
    

}