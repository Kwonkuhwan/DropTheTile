using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;//���� ��� ���
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks//�ٸ� ���� ���� �޾Ƶ��̱�
{
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();//������ ���� ������ ���� ������ ���� ����

    }
    public override void OnConnectedToMaster()//�����ͼ����� ����� �۵�
    {
        Debug.Log("connected to Master");
        //base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();//�����ͼ��� ����� �κ�� ����
    }
    public override void OnJoinedLobby()//�κ� ����� �۵�
    {
        MenuManager.Instance.OpenMenu("TitleMenu");//�κ� ������ Ÿ��Ʋ �޴� Ű��
        Debug.Log("Joined lobby");
        //base.OnJoinedLobby();
    }
    public void CreateRoom()//�游���
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;//�� �̸��� ���̸� �� �ȸ������
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);//���� ��Ʈ��ũ������� roomNameInputField.text�� �̸����� ���� �����.
        MenuManager.Instance.OpenMenu("LoadingMenu");//�ε�â ����
        Debug.Log("created room");
    }
    public override void OnJoinedRoom()//�濡 ������ �۵�
    {
        MenuManager.Instance.OpenMenu("Room");//�� �޴� ����
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;//�� �� �̸� ǥ��
        Debug.Log("on joined room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)//�� ����� ���н� �۵�
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");//���� �޴� ����
        Debug.Log("joined room failed");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();//�涰���� ���� ��Ʈ��ũ ���
        MenuManager.Instance.OpenMenu("LoadingMenu");//�ε�â ����
    }
    public override void OnLeftRoom()
    {
        //base.OnLeftRoom();
        MenuManager.Instance.OpenMenu("TitleMenu");//�� ������ ������ Ÿ��Ʋ �޴� ȣ��
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
