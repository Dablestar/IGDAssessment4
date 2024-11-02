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
    [SerializeField] private TMP_Text ghostWeakenText;
    [SerializeField] private Transform lifeDonutImages;
    [SerializeField] private TMP_Text bestRecord;
    private TMP_Text gameStartText;
    private BGAudioPlayer bg;

    public static bool IsPlaying { get; set; }
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

        bestRecord = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        levelChangeBtn = GameObject.Find("Level1Btn").GetComponent<Button>();
        

        TimeSpan bestTime = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Time"));
        int bestScore = PlayerPrefs.GetInt("Score");
        bestRecord.text = $"Best Record\n {bestTime.ToString(@"mm\:ss\:fff")}\n {bestScore}";
        levelChangeBtn.onClick.AddListener(MoveToMainScene);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlaying)
        {
            time += Time.deltaTime;
            TimeSpan timer = TimeSpan.FromSeconds(time);
            timeText.text = timer.ToString(@"mm\:ss\:fff");
            if (PacStudentController.PalletCount == 0)
            {
                StartCoroutine(GameOver());
            }
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
        SceneManager.sceneLoaded += OnStartSceneLoaded;
    }

    private void Init()
    {
        time = 0f;
        Time.timeScale = 0;
        IsPlaying = false;
        timeText = null;
        levelChangeBtn = null;
    }

    private void OnMainSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        timeText = GameObject.Find("Timer").transform.Find("Time").GetComponentInChildren<TMP_Text>();
        scoreText = GameObject.Find("Score").transform.Find("ScoreValue").GetComponentInChildren<TMP_Text>();
        lifeDonutImages = GameObject.Find("Lives").transform;
        ghostWeakenText = GameObject.Find("ScaredTimer").transform.Find("ScaredTime")
            .GetComponentInChildren<TMP_Text>();
        ghostWeakenText.gameObject.SetActive(false);
        
        bg = GameObject.Find("BGPlayer").GetComponent<BGAudioPlayer>();
        bg.PlayNormalBackground();

        Button btn = GameObject.Find("Button").GetComponent<Button>();
        btn.onClick.AddListener(_manager.MoveToStartScene);

        gameStartText = GameObject.Find("GameStartText").GetComponent<TMP_Text>();
        StartCoroutine(StartGame());

        SceneManager.sceneLoaded -= OnMainSceneLoaded;
    }

    private void OnStartSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bestRecord = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        levelChangeBtn = GameObject.Find("Level1Btn").GetComponent<Button>();
        
        TimeSpan bestTime = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Time"));
        int bestScore = PlayerPrefs.GetInt("Score");
        bestRecord.text = $"Best Record\n {bestTime.ToString(@"mm\:ss\:fff")}\n {bestScore}";
        levelChangeBtn.onClick.AddListener(MoveToMainScene);
        SceneManager.sceneLoaded -= OnStartSceneLoaded;
    }

    IEnumerator StartGame()
    {
        int count = 3;
        while (count > 0)
        {
            //3, 2, 1
            Debug.Log($"{count}");
            gameStartText.text = $"{count}";
            count--;
            yield return new WaitForSeconds(1f);
        }
        //go
        gameStartText.text = "GO!";
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
        IsPlaying = true;
        gameStartText.gameObject.SetActive(false);
    }
    
    public IEnumerator OnGhostScared()
    {
        bg.PlayWeakenBackground();
        Debug.Log($"Coroutine : {nameof(OnGhostScared)}TimeScale : {Time.timeScale}");
        ghostWeakenText.gameObject.SetActive(true);
        ghostWeakenText.alignment = TextAlignmentOptions.Center;
        int count = 10;
        while (count > 0)
        {
            Debug.Log(count);
            ghostWeakenText.text = $"Arrest Them! \n {count}";
            count--;
            yield return new WaitForSeconds(1f);
        }
        ghostWeakenText.gameObject.SetActive(false);
    }

    public void SetScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public IEnumerator GameOver()
    {
        bg.Source.Stop();
        PlayerPrefs.SetInt("Score", PacStudentController.Score);
        PlayerPrefs.SetFloat("Time", (float)time);
        IsPlaying = false;
        gameStartText.gameObject.SetActive(true);
        gameStartText.text = "Game Over!";
        PlayerPrefs.Save();
        yield return new WaitForSecondsRealtime(3f);
        MoveToMainScene();
    }

    public void DeleteIcon(int idx)
    {
        Transform icon = lifeDonutImages.Find("LifeIcons").Find($"Icon{idx}");
        if (icon is not null)
        {
            Destroy(icon.gameObject);
        }
    }
}