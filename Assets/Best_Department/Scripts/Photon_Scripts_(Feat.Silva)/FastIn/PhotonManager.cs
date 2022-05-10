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
    public override void OnConnectedToMaster()
    {
        Debug.Log("01. ���� ������ ����");
        PhotonNetwork.JoinLobby();//�����ͼ��� ����� �κ�� ����
      

    }
    public override void OnJoinedLobby()//�κ� ����� �۵�
    {
        MenuManager.Instance.OpenMenu("MainMenu");//�κ� ������ Ÿ��Ʋ �޴� Ű��
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

    public override void OnCreatedRoom()
    {
        Debug.Log("03. �� ���� �Ϸ�");
    }

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