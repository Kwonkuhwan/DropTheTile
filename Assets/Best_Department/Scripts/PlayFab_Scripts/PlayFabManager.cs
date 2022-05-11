using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using GooglePlayGames;

public class PlayFabManager : MonoBehaviour
{
    public InputField EmailInput, PasswordInput, UsernameInput;
    public Text Log_Text;

    /******************************************************************************************************************************************/
    // ȸ������ �κ�

    /// <summary>
    /// PlayFab �̸��� ȸ������
    /// </summary>
    public void OnPlayFabSignUpClick()
    {
        var request = new RegisterPlayFabUserRequest { Email = EmailInput.text, Password = PasswordInput.text, Username = UsernameInput.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => Log_Text.text = "PlayFab ȸ������ ����", (error) => Log_Text.text = "PlayFab ȸ������ ����");
    }

    /******************************************************************************************************************************************/

    /******************************************************************************************************************************************/
    // �α��� �κ�

    /// <summary>
    /// PlayFab �̸��� ȸ������
    /// </summary>
    public void OnPlayFabLoginClick()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "144";

        var request = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => Log_Text.text = "PlayFab �α��� ����", (error) => Log_Text.text = "PlayFab �α��� ����");
    }
    /******************************************************************************************************************************************/

    /******************************************************************************************************************************************/
    // Google �α��� �κ�

    private void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        OnPlayFabGoogleLoginClick();
    }

    /// <summary>
    /// (Google)PlayFab �α��� ��ư Ŭ��
    /// </summary>
    public void OnPlayFabGoogleLoginClick()
    {
        Social.localUser.Authenticate((success) =>
        {
            if (success) { Log_Text.text = "(Google)PlayFab �α��� ����"; PlayFabGoogleLogin(); }
            else Log_Text.text = "(Google)PlayFab �α��� ����";
        });   
    }


    /// <summary>
    /// (Google)PlayFab �α׾ƿ� ��ư Ŭ��
    /// </summary>
    public void OnPlayFabGoogleLogoutClick()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        Log_Text.text = "(Google)PlayFab �α׾ƿ�";
    }

    /// <summary>
    /// (Google)PlayFab �α���
    /// </summary>
    public void PlayFabGoogleLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = Social.localUser.id + "@Dropthtile.com", Password = Social.localUser.id };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => Log_Text.text = "(Google)PlayFab �α��� ����", (error) => PlayFabGoogleSingUp());
    }

    /// <summary>
    /// (Google)PlayFab ȸ������
    /// </summary>
    public void PlayFabGoogleSingUp()
    {
        int num = Random.Range(0, 10);
        string[] str_random = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
        string str_UN = str_random[num] + Random.Range(0, 2147483647).ToString();
        var request = new RegisterPlayFabUserRequest { Email = Social.localUser.id + "@Dropthtile.com", Password = Social.localUser.id, Username = str_UN };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { Log_Text.text = "(Google)PlayFab ȸ������ ����"; PlayFabGoogleLogin(); }, (error) => Log_Text.text = "(Google)PlayFab ȸ������ ����" + str_UN + " | " + error);
    }
    /******************************************************************************************************************************************/

}
