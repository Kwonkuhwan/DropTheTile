using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPGS_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public Text L_log;

    // [add] kkh : 2022.05.10 - Google Login
    public void GoogleLogin()
    {
        Debug.Log("GoogleLogin Click!!!");
        try
        {
            GPGSBinder.Inst.Login((success, localUser) =>
            {
                if (success)
                {
                    //����
                    L_log.text = $"{success}, {localUser.userName}, {localUser.id}, {localUser.state}, {localUser.underage}";
                }
                else
                {
                    // ����
                    L_log.text = "Login Fail...";
                }
            });
        }
        catch
        {
            L_log.text = "Exception Error...";
        }
    }

    // [add] kkh : 2022.05.10 - Google Logout
    public void GoogleLogout()
    {
        Debug.Log("GoogleLogout Click!!!");

        try
        {
            GPGSBinder.Inst.Logout();
        }
        catch
        {
            L_log.text = "GoogleLogout : Exception Error...";
            Debug.LogError("GoogleLogout : Exception Error...");
        }
        L_log.text = "Logout Success";
    }


    // [add] kkh : 2022.05.10 - Google Cloud Save
    public void GoogleCloudSave()
    {
        try
        {
            GPGSBinder.Inst.SaveCloud("mysave", "want data", success => L_log.text = $"{success}");
        }
        catch
        {
            L_log.text = "GoogleCloudSave : Exception Error...";

        }
    }

    // [add] kkh : 2022.05.10 - Google Cloud Load
    public void GoogleCloudLoad()
    {
        try 
        {
            GPGSBinder.Inst.LoadCloud("mysave", (success, data) => L_log.text = $"{success}, {data}");
        }
        catch
        {
            L_log.text = "GoogleCloudLoad : Exception Error...";
        }
    }

    // [add] kkh : 2022.05.10 - Google Cloud Delete
    public void GoogleCloudDelete()
    {
        try
        {
            GPGSBinder.Inst.DeleteCloud("mysave", success => L_log.text = $"{success}");
        }
        catch
        {
            L_log.text = "GoogleCloudLoad : Exception Error...";
        }
    }


    //  ��� ���� UI ����
    //GPGSBinder.Inst.ShowAchievementUI();

    // ���� ���
    //GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_one, success => log = $"{success}");
    //GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_two, success => log = $"{success}");
    //GPGSBinder.Inst.IncrementAchievement(GPGSIds.achievement_three, 1, success => log = $"{success}");

    // ��� �������� UI ����
    //GPGSBinder.Inst.ShowAllLeaderboardUI();

    // Ÿ�� �������� UI ����
    //GPGSBinder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_num);

    // �������忡 ������ �Է�
    //GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_num, 1000, success => log = $"{success}");

    // Ÿ�� ��������� ��� ������ �ҷ��´�
    //GPGSBinder.Inst.LoadAllLeaderboardArray(GPGSIds.leaderboard_num, scores =>
    //{
    //    log = "";
    //    for (int i = 0; i<scores.Length; i++)
    //        log += $"{i}, {scores[i].rank}, {scores[i].value}, {scores[i].userID}, {scores[i].date}\n";
    //});

    // ���������� �� ���� ���� ����
    //GPGSBinder.Inst.LoadCustomLeaderboardArray(GPGSIds.leaderboard_num, 10,
    //GooglePlayGames.BasicApi.LeaderboardStart.PlayerCentered, GooglePlayGames.BasicApi.LeaderboardTimeSpan.Daily, (success, scoreData) =>
    //{
    //  log = $"{success}\n";
    //  var scores = scoreData.Scores;
    //  for (int i = 0; i<scores.Length; i++)
    //    log += $"{i}, {scores[i].rank}, {scores[i].value}, {scores[i].userID}, {scores[i].date}\n";
    //});

    //GPGSBinder.Inst.IncrementEvent(GPGSIds.event_event, 1);

    //GPGSBinder.Inst.LoadEvent(GPGSIds.event_event, (success, iEvent) =>
    //{
    //log = $"{success}, {iEvent.Name}, {iEvent.CurrentCount}";
    //});

    //GPGSBinder.Inst.LoadAllEvent((success, iEvents) =>
    //{
    //log = $"{success}\n";
    //foreach (var iEvent in iEvents)
    //log += $"{iEvent.Name}, {iEvent.CurrentCount}\n";
    //});
}
