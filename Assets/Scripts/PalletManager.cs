using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletManager : MonoBehaviour
{
    private AudioSource source;

    [SerializeField] private AudioClip sound;

    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.Find("FXPlayer").gameObject.GetComponent<AudioSource>();
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
                    break;
                case "Bonus":
                    PacStudentController.AddScore(100);
                    break;
                case "SpecialPallet":
                    StartCoroutine(EnemyController.WeakenEnemy());
                    break;
            }
            UIManager.SetScoreText(PacStudentController.Score);
            Destroy(gameObject);
        }
    }
}