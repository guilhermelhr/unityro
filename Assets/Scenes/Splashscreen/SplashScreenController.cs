using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.OnGrfLoaded += OnGrfLoaded;
    }

    private void OnGrfLoaded() {
        SceneManager.LoadScene("LoginScene");
    }

    // Update is called once per frame
    void Destroy()
    {
        GameManager.OnGrfLoaded -= OnGrfLoaded;
    }
}
