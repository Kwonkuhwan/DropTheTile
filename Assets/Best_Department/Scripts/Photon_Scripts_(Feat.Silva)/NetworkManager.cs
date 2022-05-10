using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "v1.0";
    private string userId = "Ojui";

    public bool Master()
    {
        return PhotonNetwork.LocalPlayer.IsMasterClient;
    }

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

    private void Update()
    {
        UIManager.Instance.SetConnectionStatus(PhotonNetwork.NetworkClientState.ToString());
    }

    //마스터서버 연결시 로비로 연결
    public override void OnConnectedToMaster()
    {
        Debug.Log("01. 포톤 서버에 접속");
        PhotonNetwork.JoinLobby();
    }

    //로비 연결시 작동 , 로비에 진입후 타이틀 메뉴 키기
    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("MainMenu");
        Debug.Log("Joined lobby");
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

    //방 생성 완료
    public override void OnCreatedRoom()
    {
        Debug.Log("03. 방 생성 완료");
    }

    //방 입장 완료_PlayRoom으로 넘어감
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


//    //연결하는 함수
//    public void Connect()
//    {
//        if (PhotonNetwork.IsConnected)
//        {
//            PhotonNetwork.JoinRandomRoom();
//        }
//        else
//        {
//            PhotonNetwork.ConnectUsingSettings();
//        }
//    }

//    #region Photon Callbacks




//    //서버에 연결하는 중...방을 찾는중의 함수
//    public override void OnConnectedToMaster()
//    {
//        Debug.LogError($"Connected to server..Looking for random room ");
//        PhotonNetwork.JoinRandomRoom();
//    }
//    //랜덤의 방을 찾는데 실패했다, 그래서 새로 방을 만든다.
//    public override void OnJoinRandomFailed(short returnCode, string message)
//    {
//        Debug.LogError($"Joining random room failed because of {message}.Creating a new one");
//        PhotonNetwork.CreateRoom(null);
//    }

//    // 방에 참가했다.
//    public override void OnJoinedRoom()
//    {
//        Debug.LogError($"Player{PhotonNetwork.LocalPlayer.ActorNumber} joined the room");
//    }

//    //플레이어가 방에 들어왔다.
//    public override void OnPlayerEnteredRoom(Player newPlayer)
//    {
//        Debug.LogError($"Player{newPlayer.ActorNumber} enterd the room ");
//    }

//}
//    #endregion

