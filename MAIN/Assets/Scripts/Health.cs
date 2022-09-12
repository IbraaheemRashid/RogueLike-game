using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float playHealth;
    public Image healthBar;

    public GameObject gameOver;
    public static bool isOver;
    // Start is called before the first frame update
    void Start()
    {
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    
    void Update()
    {
        if (playHealth <= 0)
        {
            isOver = true;
            gameOver.SetActive(true);
            Time.timeScale = 0f;
        }
        healthBar.fillAmount = playHealth / 4;
       // GameOver();
    }
    public void OverGoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        isOver = false;
    }
    public void QuitGame()
    {
       // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    //  static public void Game()
    // {

    //PlayFabManager PlayFabManager = GameObject.Find("PlayerFabManager").GetComponent<PlayFabManager>();

    //PlayFabManager.SendLeaderboard(GameController.scoreValue);
    // }
}
