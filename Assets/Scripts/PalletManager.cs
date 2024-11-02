using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletManager : MonoBehaviour
{
    private AudioSource source;

    [SerializeField] private AudioClip sound;
    private UIManager _manager;
    private bool isSpecialEffectRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.Find("FXPlayer").gameObject.GetComponent<AudioSource>();
        _manager = GameObject.Find("Managers").gameObject.GetComponent<UIManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            source.clip = sound;
            source.Play();
            switch (gameObject.tag)
            {
                case "Normal":
                    PacStudentController.AddScore(10);
                    PacStudentController.PalletCount--;
                    Debug.Log($"pallet Left : {PacStudentController.PalletCount}");
                    break;
                case "Bonus":
                    PacStudentController.AddScore(100);
                    break;
                case "Special":
                    PacStudentController.PalletCount--;
                    Debug.Log($"pallet Left : {PacStudentController.PalletCount}");
                    break;
            }
            _manager.SetScoreText(PacStudentController.Score);
            Destroy(gameObject);
        }
    }
    
}