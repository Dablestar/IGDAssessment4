using System;
using System.Collections;
using System.Collections.Generic;
using dir;
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
    [SerializeField] private ParticleSystem movementParticle;


    
    [SerializeField]private int posX, posY;
    static int score { get; set; }

    private AudioSource studentSound;

    private bool isWalking;

    private Tweener tweener;

    private Direction lastInput;
    private Direction currentInput;

    // Start is called before the first frame update
    void Awake()
    {
        moveSpeed = 1.5f;
        score = 0;
        studentSound = gameObject.GetComponent<AudioSource>();
        tweener = gameObject.GetComponent<Tweener>();
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
            moveSpeed = 1f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            lastInput = Direction.Left;
            moveSpeed = 1f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            lastInput = Direction.Down;
            moveSpeed = 1f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            lastInput = Direction.Right;
            moveSpeed = 1f;
        }

        if (tweener.Move(transform, transform.position, lastInput, moveSpeed, posX, posY) && lastInput != Direction.None)
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

    

    private void Stop()
    {
        moveSpeed = 0f;
        studentAnim.SetInteger("movingDirection", 1);
        isWalking = false;
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
            Stop();
            studentSound.Play();
        }
    }
}