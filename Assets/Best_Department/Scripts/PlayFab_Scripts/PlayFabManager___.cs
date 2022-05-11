using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


public class PlayFabManager : MonoBehaviour
{
    ddressRequset { Email = EmailInput.text, Password = PasswordInput.text
};
PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    void OnLoginSuccess(LoginResult result)
{
    print("로그인 성공");
}

void OnLoginFailure(PlatFabError error)
{
    print("로그인 실패");
}
}
