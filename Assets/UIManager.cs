using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] Menu[] menus;



    [Header("Scene Dependencies")]
    [SerializeField] private NetworkManager networkManager;

    // 버튼은 랜덤으로  어느팀을 할건지 고르는 Btn.

    //내가 만들지 않은 함수 Onsingle, DisableAllScreen

    [Header("Texts")]
    [SerializeField] private Text connectionStatusText;

    //[Header("Screen GameObject")]
    //[SerializeField] private GameObject connectScreen;
    //[SerializeField] private GameObject endGameScreen;

    //한 사이클이 끝나면 다른 학과친구 만나러 가기!! 로 다시 
    //메인메뉴로 들어가서 랜덤매칭하도록 유도!

    void Awake()
    {
        Instance = this;
        OnGameLaunched();
    }

    //게임을 실행시켰을때
    public void OnGameLaunched()
    {
        //    endGameScreen.SetActive(false);
        //    connectScreen.SetActive(false);
        //}
    }

    //온라인 게임 모드를 고를때
    public void OnMultiplayerModeSelected()
    {
        //connectionStatusText.gameObject.SetActive(true);
        //connectScreen.SetActive(true);
    }

    //게임이 연결되었을대 / 방에 들어갔을때
    public void OnConnect()
    {
        networkManager.OnJoinedRoom();
    }

    public void SetConnectionStatus(string status)
    {
        connectionStatusText.text = status;
    }

    //메뉴를 여는 스크립트
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)//string을 받아서 해당이름 가진 메뉴를 여는 스크립트
            {
                OpenMenu(menus[i]);
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
