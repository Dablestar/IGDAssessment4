using System;
using System.Collections;
using System.Collections.Generic;
using dir;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

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
    [SerializeField] private int posX, posY;

    private UIManager _manager;

    public static int PlayerLife
    {
        get { return playerLife; }
        set { playerLife = value; }
    }

    public static int PalletCount
    {
        get { return palletCount; }
        set { palletCount = value; }
    }

    private static int score;

    public static int Score
    {
        get { return score; }
        set { score = value; }
    }

    private bool isWalking;
    private bool isDead;

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
        score = 0;
        studentSound = gameObject.GetComponent<AudioSource>();
        effectSource = GameObject.Find("FXPlayer").GetComponent<AudioSource>();
        _manager = GameObject.Find("Managers").GetComponent<UIManager>();
        studentTweener = gameObject.GetComponent<Tweener>();
        lastInput = Direction.None;
        StartCoroutine(PlayFootworkAudio());
    }

    private void Start()
    {
        isDead = false;
        posX = 1;
        posY = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.IsPlaying)
        {
            if (!isDead)
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
            }
        }
        if (!studentTweener.TweenExists(transform) && lastInput != Direction.None)
        {
            if (studentTweener.IsWalkable(lastInput, posX, posY))
            {
                isWalking = true;
                studentAnim.SetInteger("movingDirection", (int)lastInput + 1);
                currentInput = lastInput;
                studentTweener.Move(transform, transform.position, lastInput, moveSpeed, posX, posY);
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
            else if (studentTweener.IsWalkable(currentInput, posX, posY))
            {
                isWalking = true;
                studentTweener.Move(transform, transform.position, currentInput, moveSpeed, posX, posY);
                switch (currentInput)
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
            else
            {
                Stop();
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
            yield return new WaitForSeconds(moveSpeed);
        }
    }


    private void Stop()
    {
        Debug.Log("Stop()");
        isWalking = false;
        moveSpeed = 0f;
        currentInput = Direction.None;
        lastInput = Direction.None;
        studentTweener.AbortTween();
        effectSource.clip = onWallHitSound;
        effectSource.Play();
    }
    private void StopForTP()
    {
        Debug.Log("StopForTP()");
        isWalking = false;
        moveSpeed = 0f;
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
        StopForTP();
        gameObject.GetComponent<Collider2D>().enabled = false;
        studentAnim.SetTrigger("isDead");
        studentSound.clip = onPlayerHitSound;
        studentSound.Play();
        _manager.DeleteIcon(PlayerLife);
        PlayerLife--;
        if (ObjectSpawner.Respawn(gameObject.transform))
        {
            yield return new WaitUntil(() => !studentTweener.TweenExists(transform));
        }
        else
        {
            StartCoroutine(_manager.GameOver());
            gameObject.SetActive(false);
        }
        posX = 1;
        posY = 1;
        studentAnim.SetTrigger("isRespawned");
        Debug.Log("isRespawned");
        gameObject.GetComponent<Collider2D>().enabled = true;
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            EnemyController hitEnemy = other.gameObject.GetComponent<EnemyController>();
            if (hitEnemy.CurrentStatus.Equals(EnemyController.EnemyStatus.Weaken) || hitEnemy.CurrentStatus.Equals(EnemyController.EnemyStatus.Recovering))
            {
                StartCoroutine(hitEnemy.KillEnemy());
                AddScore(300);
            }
            else if(hitEnemy.CurrentStatus.Equals(EnemyController.EnemyStatus.Walking))
            {
                StartCoroutine(KillPlayer());
            }
            else
            {
                Debug.Log("Dead Enemy");
            }
        }

        if (other.gameObject.tag.Equals("Special"))
        {
            GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in allEnemy)
            {
                EnemyController temp = enemy.GetComponent<EnemyController>();
                StartCoroutine(temp.WeakenEnemy());
            }
            StartCoroutine(_manager.OnGhostScared());
        }

        if (other.gameObject.tag.Equals("TP"))
        {
            Debug.Log("Teleporter Triggered");
            Teleport();
        }
    }
    
    public static void AddScore(int addScore)
    {
        score += addScore;
    }

    private void Teleport()
    {
        Direction tempInput = lastInput;
        StopForTP();
        if (posX <= -1)
        {
            posX = 27;
            gameObject.transform.localPosition = new Vector3(27f, -14.75f, -1);
            Walk();
            if (studentTweener.Move(transform, transform.position, tempInput, moveSpeed, posX, posY))
            {
                posX--;
                lastInput = tempInput;
            }
            else
            {
                Debug.Log("Glitch");
                Stop();
            }
        }
        else if (posX >= 28)
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