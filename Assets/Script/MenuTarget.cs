using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTarget : MonoBehaviour {
    public static string target;
	// Use this for initialization
    void Awake() {
        DontDestroyOnLoad(this);
    }
}
