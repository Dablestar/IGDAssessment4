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
    private AudioSource effectSource;
    private AudioSource studentSound;
    [SerializeField] private List<AudioClip> footstepSoundEffects;
    [SerializeField] private AudioClip onPlayerHitSound;
    [SerializeField] private AudioClip onWallHitSound;
    [SerializeField] private Animator studentAnim;
    [SerializeField] private ParticleSystem movementParticle;
    private static int playerLife = 3;
    private static int palletCount = 0;
    public static int PlayerLife
    {
        get { return playerLife;}
        set { playerLife = value; }
    }
    public static int PalletCount
    {
        get { return palletCount;}
        set { palletCount = value; }
    }
    
    
    [SerializeField]private int posX, posY;
    private static int score;
    public static int Score
    {
        get { return score; }
        set { score = value; }
    }

    

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
            isWalking = true;
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
        isWalking = false;
        currentInput = Direction.None;
        lastInput = Direction.None;
        studentTweener.AbortTween();
    }

    private void Walk()
    {
        moveSpeed = 0.5f;
        isWalking = true;
    }

    

    private IEnumerator KillPlayer()
    {
        Stop();
        gameObject.GetComponent<Collider2D>().enabled = false;
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
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            if (!EnemyController.isWeaken)
            {
                StartCoroutine(KillPlayer());    
            }
        }
        if (other.gameObject.tag.Equals("TP"))
        {
            Debug.Log("Teleporter Triggered");
            Teleport();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Walls"))
        {
            Stop();
        }
    }

    public static void AddScore(int addScore)
    {
        score += addScore;
    }

    private void Teleport()
    {
        Direction tempInput = lastInput;
        Stop();   
        if (posX <= -1)
        {
            posX = 27;
            gameObject.transform.localPosition = new Vector3(27f, -14.75f, -1);
            Walk();
            if (studentTweener.Move(transform, transform.position, tempInput, moveSpeed, posX, posY))
            {
                posX--;
                lastInput = tempInput;
            }else
            {
                Debug.Log("Glitch");
                Stop();
            }
        }
        else if(posX >= 28)
        {
            posX = 0;
            gameObject.transform.localPosition = new Vector3(0, -14.75f, -1);
            Walk();
            if (studentTweener.Move(transform, transform.position, tempInput, moveSpeed, posX, posY))
            {
                posX++;
                lastInput = tempInput;
            }
            else
            {
                Debug.Log("Glitch");
                Stop();
            }
        }

        

    }
}