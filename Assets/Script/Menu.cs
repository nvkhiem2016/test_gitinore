using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Menu : MonoBehaviour {

    public void Enablegame()
    {
        Load("Page1");

    }
    public void Decoder()
    {
        Load("Page3");

    }
    public void PlayerScore()
    {
        Load("Page4");

    }
    public void GameController()
    {
        Load("Page6");

    }
    public void Logout()
    {
        Load("Login");

    }
    public void Load(string scenename)
    {
        Debug.Log("sceneName to load: " + scenename);
        SceneManager.LoadScene(scenename);
    }
}
