using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Header : MonoBehaviour {
    public void Load(string scenename)
    {
        Debug.Log("sceneName to load: " + scenename);
        SceneManager.LoadScene(scenename);
    }
    public void Menu()
    {
        Load("Menu");

    }
    public void Logout()
    {
        Load("Login");

    }
}
