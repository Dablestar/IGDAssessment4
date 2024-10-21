using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    private int score;
    private float time;
    private Button button;
    
    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void MoveToMainScene()
    {
        Debug.Log("clicked");
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
    }

    public void MoveToDesignIteration()
    {
        Debug.Log("Not Implemented");
    }

    public void MoveToStartScene()
    {
        score = 0;
        Time.timeScale = 0;
        time = 0f;
        
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("StartScene");
    }

    
    
}
