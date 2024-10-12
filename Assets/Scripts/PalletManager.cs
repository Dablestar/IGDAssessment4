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

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag.Equals("Player"))
        {
            source.clip = sound;
            source.Play();
            if (gameObject.tag.Equals("normalPallet"))
            {
                PacStudentController.AddScore(500);
            }

            if (gameObject.tag.Equals("specialPallet"))
            {
                StartCoroutine(EnemyController.WeakenEnemy());
            }

            if (gameObject.tag.Equals("bonusPallet"))
            {
                PacStudentController.AddScore(5000);
            }
            Destroy(this);
        }
    }
}