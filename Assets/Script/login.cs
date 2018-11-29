using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


[System.Serializable]
public class User
{
    public string id;
    public int per;
    public Boolean success;
    public static User CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<User>(jsonString);
    }
}
public class login : MonoBehaviour {

    public GameObject id;
    public GameObject password;
    private string strid;
    private string strpassword;
    
    // Use this for initialization
    public void RegisterButtonPressed()
    {
        Load("Register");

    }
    public void Load(string scenename)
    {
        Debug.Log("sceneName to load: " + scenename);
        SceneManager.LoadScene(scenename);
    }
    public void LoginButtonPressed()
    {
        strid = id.GetComponent<InputField>().text;
        strpassword = password.GetComponent<InputField>().text;
        if (!string.IsNullOrEmpty(strid) && !string.IsNullOrEmpty(strpassword))
        {
            StartCoroutine(SendLoginData());
        }
    }

    private IEnumerator SendLoginData()
    {
        Dictionary<string, string> passport = new Dictionary<string, string>();
        passport["id"] = strid;
        passport["password"] = strpassword;

        // Delete cookie before requesting a new login
        webservices.CookieString = null;

        var request = webservices.Post("login", new JSONObject(passport).ToString());
        yield return request.Send();

        if (request.isNetworkError)
        {
            Debug.Log("OKE");
            Debug.LogError(request.error);
        }
        else
        {
            webservices.CookieString = request.GetResponseHeader("set-cookie");
            
            Debug.Log(webservices.CookieString);
            Debug.Log(request.downloadHandler.text);
            User user =User.CreateFromJSON(request.downloadHandler.text);// conver json string to Object


            if (user.success == true) //so sanh ket qua : true là login ok
            {
                webservices.userId = user.id;
                webservices.userPer = user.per;
                Debug.Log(webservices.userPer);
                Load("Menu");
            }
            else
            {
                Debug.Log("Lỗi ĐN");
            }

        }
    }
    
}
