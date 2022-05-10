using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "v1.0";
    private string userId = "Ojui";

    private void Awake()
    {
        // 방장이 혼자 씬을 로딩하면, 나머지 사람들은 자동으로 싱크가 됨
        PhotonNetwork.AutomaticallySyncScene = true;

        // 게임 버전 지정
        PhotonNetwork.GameVersion = gameVersion;

        // 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    void Start()
    {
        Debug.Log("00. 포톤 매니저 시작");
        PhotonNetwork.NickName = userId;
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("01. 포톤 서버에 접속");
        PhotonNetwork.JoinLobby();//마스터서버 연결시 로비로 연결
      

    }
    public override void OnJoinedLobby()//로비 연결시 작동
    {
        MenuManager.Instance.OpenMenu("MainMenu");//로비에 들어오면 타이틀 메뉴 키기
        Debug.Log("Joined lobby");
        //base.OnJoinedLobby();
    }
    public void JoinRandomRoom() 
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Join lobby");
    }
    

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        

        // 룸 속성 설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 5;


        // 룸을 생성 > 자동 입장됨
        //PhotonNetwork.CreateRoom("null", ro);
        PhotonNetwork.CreateRoom(roomName: null, new RoomOptions { MaxPlayers = 2 });
        Debug.Log("02. 랜덤 룸 접속 실패");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("03. 방 생성 완료");
    }

    public override void OnJoinedRoom()
    {
       /*if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }*/
        Debug.Log("04. 방 입장 완료");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("JK_PlayRoom_2");
           
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();//방떠나기 포톤 네트워크 기능
        //MenuManager.Instance.OpenMenu("LoadingMenu");//로딩창 열기
    }
    public override void OnLeftRoom()
    {
        //base.OnLeftRoom();
        PhotonNetwork.LoadLevel("Menu_1");
    }
}