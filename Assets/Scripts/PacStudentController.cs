using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UIElements;

public class PacStudentController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<AudioClip> footstepSoundEffects;
    [SerializeField] private AudioClip onPlayerHitSound;
    [SerializeField] private AudioClip onWallHitSound;
    [SerializeField] private Animator studentAnim;

    static int score { get; set; }

    private AudioSource studentSound;

    private bool isWalking;

    private Tweener tweener;

    private Vector2[] pointList;

    // Start is called before the first frame update
    void Awake()
    {
        moveSpeed = 1.5f;
        score = 0;
        studentSound = gameObject.GetComponent<AudioSource>();
        tweener = gameObject.GetComponent<Tweener>();
        pointList = new Vector2[]
        {
            new Vector2(-5.5f, 4f),
            new Vector2(-5.5f, 0.5f),
            new Vector2(-10.5f, 0.5f),
            new Vector2(-10.5f, 4f)
        };
        StartCoroutine(PlayFootworkAudio());
        StartCoroutine(MovePlayer());
    }

    // Update is called once per frame
    void Update()
    {
        studentAnim.SetFloat("moveSpeed", moveSpeed);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PlayFootworkAudio());
            StartCoroutine(MovePlayer());
        }
    }

    public static void AddScore(int amount)
    {
        score += amount;
    }


    private IEnumerator PlayFootworkAudio()
    {
        while (true)
        {
            if (isWalking)
            {
                if (studentSound.clip == footstepSoundEffects[0])
                {
                    studentSound.clip = footstepSoundEffects[1];
                    studentSound.Play();
                }
                else
                {
                    studentSound.clip = footstepSoundEffects[0];
                    studentSound.Play();
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // private int SetAnimationDirection(Vector2 startPos, Vector2 endPos)
    // {
    //     // W, up
    //     if (startPos.y < endPos.y)
    //     {
    //         return 1;
    //     }
    //
    //     // A, left
    //     if (startPos.x < endPos.x)
    //     {
    //         return 2;
    //     }
    //
    //     // S, down
    //     if (startPos.y > endPos.y)
    //     {
    //         return 3;
    //     }
    //
    //     // D, right
    //     if (startPos.y > endPos.y)
    //     {
    //         return 4;
    //     }
    //
    //     //default, down
    //     return 2;
    // }

    private void Stop()
    {
        moveSpeed = 0f;
        studentAnim.SetInteger("movingDirection", 1);
        isWalking = false;
    }

    private void Move()
    {
        moveSpeed = 3f;
        isWalking = true;
    }

    private IEnumerator MovePlayer()
    {
        for (int i = 0; i < pointList.Length; i++)
        {
            Move();
            studentAnim.SetInteger("movingDirection", (4 - i));
            if (!tweener.isTweenOnGoing())
            {
                tweener.AddTween(gameObject.transform, gameObject.transform.position, pointList[i], moveSpeed);
            }

            yield return new WaitUntil(() => !tweener.isTweenOnGoing());
            Stop();
            if (i >= 3) i = -1;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator KillPlayer()
    {
        Stop();
        studentAnim.SetTrigger("isDead");
        studentSound.clip = onPlayerHitSound;
        studentSound.Play();
        if (ObjectSpawner.Respawn())
        {
            yield return new WaitForSeconds(1f);
            ObjectSpawner.PlayerLife--;
            Destroy(this);
        }
        else
        {
            Destroy(this);
            Application.Quit();
        }

        yield return new WaitForSeconds(1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            Stop();
            studentAnim.SetTrigger(3);
            studentSound.clip = onPlayerHitSound;
            studentSound.Play();
            if (ObjectSpawner.Respawn())
            {
                Destroy(this);
                ObjectSpawner.PlayerLife--;
            }
            else
            {
                Destroy(this);
                Application.Quit();
            }
        }
        else if (other.gameObject.tag.Equals("Walls"))
        {
            transform.Rotate(90f, 0f, 0f);
            studentSound.clip = onWallHitSound;
            studentSound.Play();
        }
    }
}