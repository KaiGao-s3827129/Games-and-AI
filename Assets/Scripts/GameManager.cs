using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    static GameManager gameManager;

    private void Awake()
    {
        if (gameManager != null)
        {
            Destroy(gameObject);
            return;
        }

        gameManager = this;
        
        DontDestroyOnLoad(this);
    }

    public static void PlayerDied()
    {
        gameManager.Invoke("RestrartScene", 1.5f);
    }

    void RestrartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
