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
    private Time time;
    
    // Start is called before the first frame update
    void Start()
    {
        time = new Time();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void MoveToMainScene()
    {
        Debug.Log("clicked");
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("MainScene");
    }

    public void MoveToDesignIteration()
    {
        DontDestroyOnLoad(this);
        Debug.Log("Not Implemented");
    }

    public void MoveToStartScene()
    {
        
    }

    
    
}
