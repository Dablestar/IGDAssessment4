using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    private double time;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private static TMP_Text scoreText;
    
    [SerializeField] private bool isPlaying;
    
    private Button levelChangeBtn;
    
    private static UIManager _manager;
    // Start is called before the first frame update

    private void Awake()
    {
        if (_manager != null && _manager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _manager = this;
            DontDestroyOnLoad(gameObject);
        }

        levelChangeBtn = GameObject.Find("Level1Btn").GetComponent<Button>();
        levelChangeBtn.onClick.AddListener(MoveToMainScene);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            time += Time.deltaTime;
            TimeSpan timer = TimeSpan.FromSeconds(time);
            timeText.text = timer.ToString(@"mm\:ss\:fff");
        }
    }
    
    public void MoveToMainScene()
    {
        Debug.Log("clicked");
        SceneManager.LoadSceneAsync("MainScene");
        SceneManager.sceneLoaded += OnMainSceneLoaded;
    }

    public void MoveToDesignIteration()
    {
        Debug.Log("Not Implemented");
    }

    public void MoveToStartScene()
    {
        Init();
        Debug.Log("BackBtn Clicked");
        SceneManager.LoadSceneAsync("StartScene");
    }

    private void Init()
    {
        time = 0f;
        Time.timeScale = 0;
        isPlaying = false;
        timeText = null;
        levelChangeBtn = null;
    }

    public void OnMainSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        isPlaying = true;
        timeText = GameObject.Find("Timer").transform.Find("Time").GetComponentInChildren<TMP_Text>();
        scoreText = GameObject.Find("Score").transform.Find("ScoreValue").GetComponentInChildren<TMP_Text>();

        Button btn = GameObject.Find("Button").GetComponent<Button>();
        btn.onClick.AddListener(_manager.MoveToStartScene);
        
        SceneManager.sceneLoaded -= OnMainSceneLoaded;
    }

    public void OnGhostScared()
    {
        
    }

    public static void SetScoreText(int score)
    {
        scoreText.text = score.ToString();
    }
    
}
