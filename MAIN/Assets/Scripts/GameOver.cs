using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOver;
    public static bool isOver;
    // Start is called before the first frame update
    void Start()
    {
        gameOver.SetActive(false);
        // sets game over screen to inactive on default
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
