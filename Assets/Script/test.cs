using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class test : MonoBehaviour {
    public GameObject password;
    private string strpassword;
    public GameObject verifyPopup;
	// Use this for initialization

    public void login()
    {
        strpassword = password.GetComponent<InputField>().text;
        if (!string.IsNullOrEmpty(strpassword))
        {
            StartCoroutine(SendLoginData());
        }
    }
    public void destroyPopup()
    {
        Destroy(verifyPopup);
    }

    public void Load(string scenename)
    {
        Debug.Log("sceneName to load: " + scenename);
        SceneManager.LoadScene(scenename);
    }

    private IEnumerator SendLoginData()
    {
        Dictionary<string, string> passport = new Dictionary<string, string>();
        passport["password"] = strpassword;
        passport["id"] = webservices.userId;
        //Debug.Log(passport["id"]);
       
  

        var request = webservices.Post("login", new JSONObject(passport).ToString());
        yield return request.Send();

        if (request.isNetworkError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            
            Debug.Log(request.downloadHandler.text);
            User user = User.CreateFromJSON(request.downloadHandler.text);
            
            if (user.success==true) //so sanh ket qua tra ve == 0K => correct
            {
                Load(MenuTarget.target);
                Destroy(GameObject.Find("target"));
                Destroy(this);
                Debug.Log(user.id);
            }

        }
    }
}
