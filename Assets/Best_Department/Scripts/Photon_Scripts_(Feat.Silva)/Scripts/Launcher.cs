using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;//포톤 기능 사용
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks//다른 포톤 반응 받아들이기
{
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();//설정한 포톤 서버에 따라 마스터 서버 연결

    }
    public override void OnConnectedToMaster()//마스터서버에 연결시 작동
    {
        Debug.Log("connected to Master");
        //base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();//마스터서버 연결시 로비로 연결
    }
    public override void OnJoinedLobby()//로비 연결시 작동
    {
        MenuManager.Instance.OpenMenu("TitleMenu");//로비에 들어오면 타이틀 메뉴 키기
        Debug.Log("Joined lobby");
        //base.OnJoinedLobby();
    }
    public void CreateRoom()//방만들기
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;//방 이름이 빈값이면 방 안만들어짐
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);//포톤 네트워크기능으로 roomNameInputField.text의 이름으로 방을 만든다.
        MenuManager.Instance.OpenMenu("LoadingMenu");//로딩창 열기
        Debug.Log("created room");
    }
    public override void OnJoinedRoom()//방에 들어갔을때 작동
    {
        MenuManager.Instance.OpenMenu("Room");//룸 메뉴 열기
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;//들어간 방 이름 표시
        Debug.Log("on joined room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)//방 만들기 실패시 작동
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");//에러 메뉴 열기
        Debug.Log("joined room failed");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();//방떠나기 포톤 네트워크 기능
        MenuManager.Instance.OpenMenu("LoadingMenu");//로딩창 열기
    }
    public override void OnLeftRoom()
    {
        //base.OnLeftRoom();
        MenuManager.Instance.OpenMenu("TitleMenu");//방 떠나기 성공시 타이틀 메뉴 호출
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
