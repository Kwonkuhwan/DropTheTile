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

    // ��ư�� ��������  ������� �Ұ��� ���� Btn.

    //���� ������ ���� �Լ� Onsingle, DisableAllScreen

    [Header("Texts")]
    [SerializeField] private Text connectionStatusText;

    //[Header("Screen GameObject")]
    //[SerializeField] private GameObject connectScreen;
    //[SerializeField] private GameObject endGameScreen;

    //�� ����Ŭ�� ������ �ٸ� �а�ģ�� ������ ����!! �� �ٽ� 
    //���θ޴��� ���� ������Ī�ϵ��� ����!

    void Awake()
    {
        Instance = this;
        OnGameLaunched();
    }

    //������ �����������
    public void OnGameLaunched()
    {
        //    endGameScreen.SetActive(false);
        //    connectScreen.SetActive(false);
        //}
    }

    //�¶��� ���� ��带 ����
    public void OnMultiplayerModeSelected()
    {
        //connectionStatusText.gameObject.SetActive(true);
        //connectScreen.SetActive(true);
    }

    //������ ����Ǿ����� / �濡 ������
    public void OnConnect()
    {
        networkManager.OnJoinedRoom();
    }

    public void SetConnectionStatus(string status)
    {
        connectionStatusText.text = status;
    }

    //�޴��� ���� ��ũ��Ʈ
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)//string�� �޾Ƽ� �ش��̸� ���� �޴��� ���� ��ũ��Ʈ
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
