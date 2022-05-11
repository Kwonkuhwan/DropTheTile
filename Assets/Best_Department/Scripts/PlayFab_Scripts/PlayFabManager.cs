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
    // 회원가입 부분

    /// <summary>
    /// PlayFab 이메일 회원가입
    /// </summary>
    public void OnPlayFabSignUpClick()
    {
        var request = new RegisterPlayFabUserRequest { Email = EmailInput.text, Password = PasswordInput.text, Username = UsernameInput.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => Log_Text.text = "PlayFab 회원가입 성공", (error) => Log_Text.text = "PlayFab 회원가입 실패");
    }

    /******************************************************************************************************************************************/

    /******************************************************************************************************************************************/
    // 로그인 부분

    /// <summary>
    /// PlayFab 이메일 회원가입
    /// </summary>
    public void OnPlayFabLoginClick()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "144";

        var request = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => Log_Text.text = "PlayFab 로그인 성공", (error) => Log_Text.text = "PlayFab 로그인 실패");
    }
    /******************************************************************************************************************************************/

    /******************************************************************************************************************************************/
    // Google 로그인 부분

    private void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        OnPlayFabGoogleLoginClick();
    }

    /// <summary>
    /// (Google)PlayFab 로그인 버튼 클릭
    /// </summary>
    public void OnPlayFabGoogleLoginClick()
    {
        Social.localUser.Authenticate((success) =>
        {
            if (success) { Log_Text.text = "(Google)PlayFab 로그인 성공"; PlayFabGoogleLogin(); }
            else Log_Text.text = "(Google)PlayFab 로그인 실패";
        });   
    }


    /// <summary>
    /// (Google)PlayFab 로그아웃 버튼 클릭
    /// </summary>
    public void OnPlayFabGoogleLogoutClick()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        Log_Text.text = "(Google)PlayFab 로그아웃";
    }

    /// <summary>
    /// (Google)PlayFab 로그인
    /// </summary>
    public void PlayFabGoogleLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = Social.localUser.id + "@Dropthtile.com", Password = Social.localUser.id };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => Log_Text.text = "(Google)PlayFab 로그인 성공", (error) => PlayFabGoogleSingUp());
    }

    /// <summary>
    /// (Google)PlayFab 회원가입
    /// </summary>
    public void PlayFabGoogleSingUp()
    {
        int num = Random.Range(0, 10);
        string[] str_random = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
        string str_UN = str_random[num] + Random.Range(0, 2147483647).ToString();
        var request = new RegisterPlayFabUserRequest { Email = Social.localUser.id + "@Dropthtile.com", Password = Social.localUser.id, Username = str_UN };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { Log_Text.text = "(Google)PlayFab 회원가입 성공"; PlayFabGoogleLogin(); }, (error) => Log_Text.text = "(Google)PlayFab 회원가입 실패" + str_UN + " | " + error);
    }
    /******************************************************************************************************************************************/

}
