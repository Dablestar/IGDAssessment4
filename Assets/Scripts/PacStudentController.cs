using System;
using System.Collections;
using System.Collections.Generic;
using dir;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UIElements;

public class PacStudentController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private AudioSource source;
    [SerializeField] private List<AudioClip> footstepSoundEffects;
    [SerializeField] private AudioClip onPlayerHitSound;
    [SerializeField] private AudioClip onWallHitSound;
    [SerializeField] private Animator studentAnim;
    [SerializeField] private ParticleSystem movementParticle;
    private static int playerLife = 3;
    public static int PlayerLife
    {
        get { return playerLife;}
        set { playerLife = value; }
    }
    
    
    [SerializeField]private int posX, posY;
    private static int score;
    public static int Score
    {
        get { return score; }
        set { score = value; }
    }

    private AudioSource studentSound;

    private bool isWalking;

    private static Tweener studentTweener;
    public static Tweener StudentTweener
    {
        get { return studentTweener; }
    }

    private Direction lastInput;
    private Direction currentInput;

    // Start is called before the first frame update
    void Awake()
    {
        moveSpeed = 0.5f;
        score = 0;
        studentSound = gameObject.GetComponent<AudioSource>();
        studentTweener = gameObject.GetComponent<Tweener>();
        lastInput = Direction.None;
        StartCoroutine(PlayFootworkAudio());
    }

    private void Start()
    {
        Stop();
        posX = 1;
        posY = 1;
    }

    // Update is called once per frame
    void Update()
    {
        studentAnim.SetFloat("moveSpeed", moveSpeed);
        if (Input.GetKeyDown(KeyCode.W))
        {
            lastInput = Direction.Up;
            moveSpeed = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            lastInput = Direction.Left;
            moveSpeed = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            lastInput = Direction.Down;
            moveSpeed = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            lastInput = Direction.Right;
            moveSpeed = 0.5f;
        }

        if (studentTweener.Move(transform, transform.position, lastInput, moveSpeed, posX, posY) && lastInput != Direction.None)
        {
            movementParticle.Play();
            currentInput = lastInput;
            switch (lastInput)
            {
                case Direction.Up:
                    posY--;
                    break;
                case Direction.Down:
                    posY++;
                    break;
                case Direction.Left:
                    posX--;
                    break;
                case Direction.Right:
                    posX++;
                    break;
            }
        }
        
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

    

    private void Stop()
    {
        moveSpeed = 0f;
        studentAnim.SetInteger("movingDirection", 1);
        isWalking = false;
        currentInput = Direction.None;
        lastInput = Direction.None;
        studentSound.Stop();
        studentTweener.AbortTween();
    }

    

    private IEnumerator KillPlayer()
    {
        Stop();
        studentAnim.SetTrigger("isDead");
        studentSound.clip = onPlayerHitSound;
        studentSound.Play();
        if (ObjectSpawner.Respawn(gameObject.transform))
        {
            yield return new WaitForSeconds(1f);
            PlayerLife--;
        }
        else
        {
            Application.Quit();
        }
        posX = 1;
        posY = 1;
        yield return new WaitForSeconds(3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            StartCoroutine(KillPlayer());
        }
    }

    public static void AddScore(int addScore)
    {
        score += addScore;
    }
}