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
        // ������ ȥ�� ���� �ε��ϸ�, ������ ������� �ڵ����� ��ũ�� ��
        PhotonNetwork.AutomaticallySyncScene = true;
        // ���� ���� ����
        PhotonNetwork.GameVersion = gameVersion;
        // ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    void Start()
    {
        Debug.Log("00. ���� �Ŵ��� ����");
        PhotonNetwork.NickName = userId;
    }

    private void Update()
    {
        UIManager.Instance.SetConnectionStatus(PhotonNetwork.NetworkClientState.ToString());
    }

    //�����ͼ��� ����� �κ�� ����
    public override void OnConnectedToMaster()
    {
        Debug.Log("01. ���� ������ ����");
        PhotonNetwork.JoinLobby();
    }

    //�κ� ����� �۵� , �κ� ������ Ÿ��Ʋ �޴� Ű��
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
        // �� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 5;

        // ���� ���� > �ڵ� �����
        //PhotonNetwork.CreateRoom("null", ro);
        PhotonNetwork.CreateRoom(roomName: null, new RoomOptions { MaxPlayers = 2 });
        Debug.Log("02. ���� �� ���� ����");
    }

    //�� ���� �Ϸ�
    public override void OnCreatedRoom()
    {
        Debug.Log("03. �� ���� �Ϸ�");
    }

    //�� ���� �Ϸ�_PlayRoom���� �Ѿ
    public override void OnJoinedRoom()
    {
        /*if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
         {
             PhotonNetwork.CurrentRoom.IsOpen = false;
             PhotonNetwork.CurrentRoom.IsVisible = false;
         }*/
        Debug.Log("04. �� ���� �Ϸ�");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("JK_PlayRoom_2");
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();//�涰���� ���� ��Ʈ��ũ ���
        //MenuManager.Instance.OpenMenu("LoadingMenu");//�ε�â ����
    }

    public override void OnLeftRoom()
    {
        //base.OnLeftRoom();
        PhotonNetwork.LoadLevel("Menu_1");
    }
}


//    //�����ϴ� �Լ�
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




//    //������ �����ϴ� ��...���� ã������ �Լ�
//    public override void OnConnectedToMaster()
//    {
//        Debug.LogError($"Connected to server..Looking for random room ");
//        PhotonNetwork.JoinRandomRoom();
//    }
//    //������ ���� ã�µ� �����ߴ�, �׷��� ���� ���� �����.
//    public override void OnJoinRandomFailed(short returnCode, string message)
//    {
//        Debug.LogError($"Joining random room failed because of {message}.Creating a new one");
//        PhotonNetwork.CreateRoom(null);
//    }

//    // �濡 �����ߴ�.
//    public override void OnJoinedRoom()
//    {
//        Debug.LogError($"Player{PhotonNetwork.LocalPlayer.ActorNumber} joined the room");
//    }

//    //�÷��̾ �濡 ���Դ�.
//    public override void OnPlayerEnteredRoom(Player newPlayer)
//    {
//        Debug.LogError($"Player{newPlayer.ActorNumber} enterd the room ");
//    }

//}
//    #endregion

