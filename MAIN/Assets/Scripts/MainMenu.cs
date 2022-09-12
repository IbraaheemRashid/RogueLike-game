using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // will go to the next scene which is level 1
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("Level 1");
        // will start the game again
    }

    

    // Update is called once per frame
    public void QuitGame()
    {
       // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    
}
