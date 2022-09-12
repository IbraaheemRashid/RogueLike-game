/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabManager : MonoBehaviour
{ }
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }

    // Update is called once per frame
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Points",
                    Value = score
                }
            }
            };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        }
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
        {
        Debug.Log("Leaderboard Sent");
         }
    }
*/

