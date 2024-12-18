using System;
using System.Collections;
using System.Collections.Generic;
using dir;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public enum EnemyStatus
    {
        Walking,
        Weaken,
        Recovering,
        Dead
    }

    private Transform player;
    private Vector3 currentPlayerPosition;
    [SerializeField] private Direction currentDirection;
    [SerializeField] private Direction lastDirection;
    [SerializeField] private List<Direction> availableDirections;
    private float moveSpeed = 2f;
    private Animator enemyAnim;
    private bool isWeaken { get; set; }
    private bool isDead { get; set; }
    private BGAudioPlayer bg;
    [SerializeField] private int posX;
    [SerializeField] private int posY;
    private Tweener enemyTweener;
    

    public Tweener EnemyTweener
    {
        get { return enemyTweener; }
    }

    private List<GameObject> enemyList;

    public EnemyStatus CurrentStatus { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        isWeaken = false;
        isDead = false;
        enemyAnim = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        bg = GameObject.Find("BGPlayer").GetComponent<BGAudioPlayer>();
        availableDirections = new List<Direction>();
        CurrentStatus = EnemyStatus.Walking;
        enemyAnim.SetFloat("moveSpeed", moveSpeed);
        enemyAnim.SetInteger("movingDirection", 1);
        enemyTweener = gameObject.GetComponent<Tweener>();

        switch (transform.GetChild(0).name)
        {
            case "Ghost1Canvas(Clone)":
                posX = 6;
                posY = 12;
                break;
            case "Ghost2Canvas(Clone)":
                //Ghost2Behaviour
                posX = 14;
                posY = 12;
                break;
            case "Ghost3Canvas(Clone)":
                //Ghost3Behaviour
                posX = 14;
                posY = 16;
                break;
            case "Ghost4Canvas(Clone)":
                posX = 27;
                posY = 27;
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.IsPlaying)
        {
            currentPlayerPosition = player.position;
            if (!isWeaken && !isDead)
            {
                moveSpeed = 0.75f;
                switch (transform.GetChild(0).name)
                {
                    case "Ghost1Canvas(Clone)":
                        //Ghost1Behaviour
                        GetGhost1Behaviours();
                        break;
                    case "Ghost2Canvas(Clone)":
                        //Ghost2Behaviour
                        GetGhost2Behaviours();
                        break;
                    case "Ghost3Canvas(Clone)":
                        //Ghost3Behaviour
                        GetGhost3Behaviours();
                        break;
                    case "Ghost4Canvas(Clone)":
                        //Ghost4Behaviour
                        GetGhost4Behaviours();
                        break;
                    default:
                        Debug.Log("Error");
                        break;
                }
            }
            else if (isWeaken)
            {
                GetGhost1Behaviours();
                
                //Collision Debugging Purpose
                //GetGhost4Behaviours();
            }
        }
    }


    private void GetGhost1Behaviours()
    {
        
        //Create list based on priority
        
        availableDirections.Clear();
        Vector3 temp = currentPlayerPosition - gameObject.transform.position;

        if (temp.x == 0 && temp.y > 0) availableDirections.Add(Direction.Down);
        else if (temp.x == 0 && temp.y < 0) availableDirections.Add(Direction.Up);
        else if (temp.x > 0 && temp.y == 0) availableDirections.Add(Direction.Left);
        else if (temp.x < 0 && temp.y == 0) availableDirections.Add(Direction.Right);
        else if (temp.x > 0 && temp.y > 0)
        {
            availableDirections.Add(Direction.Down);
            availableDirections.Add(Direction.Left);
        }
        else if (temp.x > 0 && temp.y < 0)
        {
            availableDirections.Add(Direction.Up);
            availableDirections.Add(Direction.Left);
        }
        else if (temp.x < 0 && temp.y > 0)
        {
            availableDirections.Add(Direction.Down);
            availableDirections.Add(Direction.Right);
        }
        else if (temp.x < 0 && temp.y < 0)
        {
            availableDirections.Add(Direction.Up);
            availableDirections.Add(Direction.Right);
        }

        availableDirections.RemoveAll(d => d == lastDirection);

        if (availableDirections.Count == 0)
        {
            Walk(lastDirection);
        }
        else
        {
            Direction selectedDirection = availableDirections[Random.Range(0, availableDirections.Count)];
            Walk(selectedDirection);
            lastDirection = currentDirection;
        }
    }

    private void GetGhost2Behaviours()
    {
        availableDirections.Clear();
        Vector3 temp = currentPlayerPosition - gameObject.transform.position;

        if (temp.x == 0 && temp.y > 0) availableDirections.Add(Direction.Up);
        else if (temp.x == 0 && temp.y < 0) availableDirections.Add(Direction.Down);
        else if (temp.x > 0 && temp.y == 0) availableDirections.Add(Direction.Right);
        else if (temp.x < 0 && temp.y == 0) availableDirections.Add(Direction.Left);
        else if (temp.x > 0 && temp.y > 0)
        {
            availableDirections.Add(Direction.Up);
            availableDirections.Add(Direction.Right);
        }
        else if (temp.x > 0 && temp.y < 0)
        {
            availableDirections.Add(Direction.Down);
            availableDirections.Add(Direction.Right);
        }
        else if (temp.x < 0 && temp.y > 0)
        {
            availableDirections.Add(Direction.Up);
            availableDirections.Add(Direction.Left);
        }
        else if (temp.x < 0 && temp.y < 0)
        {
            availableDirections.Add(Direction.Down);
            availableDirections.Add(Direction.Left);
        }

        availableDirections.RemoveAll(d => d == lastDirection);
        availableDirections.RemoveAll(d => !enemyTweener.IsWalkable(d, posX, posY));

        if (availableDirections.Count == 0)
        {
            currentDirection = lastDirection;
            Walk(currentDirection);
        }
        else
        {
            Direction selectedDirection = availableDirections[Random.Range(0, availableDirections.Count)];
            Walk(selectedDirection);
            lastDirection = currentDirection;
        }
    }

    private void GetGhost3Behaviours()
    {
        availableDirections.Clear();

        availableDirections.Add(Direction.Up);
        availableDirections.Add(Direction.Down);
        availableDirections.Add(Direction.Left);
        availableDirections.Add(Direction.Right);

        availableDirections.RemoveAll(d => d == lastDirection);
        Walk(availableDirections[Random.Range(0, availableDirections.Count)]);
    }

    private void GetGhost4Behaviours()
    {
        Direction[] directionsList = new[] { Direction.Up, Direction.Left };
    }

    private void Walk(Direction direction)
    {
        if (!enemyTweener.TweenExists(transform))
        {
            currentDirection = direction;
            enemyAnim.SetInteger("movingDirection", (int)direction + 1);
            if (enemyTweener.Move(transform, transform.position, currentDirection, moveSpeed, posX, posY))
            {
                switch (direction)
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
    }


    public IEnumerator WeakenEnemy()
    {
        bg.PlayWeakenBackground();
        Debug.Log("Enemy Weakened()");
        if (!isWeaken)
        {
            isWeaken = true;
        }
        enemyAnim.SetTrigger("isWeaken");
        CurrentStatus = EnemyStatus.Weaken;
        yield return new WaitForSeconds(7f);
        //set animation recovering
        enemyAnim.SetTrigger("isOnRecover");
        CurrentStatus = EnemyStatus.Recovering;
        yield return new WaitForSeconds(3f);
        enemyAnim.SetTrigger("isRecovered");
        CurrentStatus = EnemyStatus.Walking;
        isWeaken = false;
        Debug.Log("Recovered");
        bg.PlayNormalBackground();
    }

    public IEnumerator KillEnemy()
    {
        bg.PlayKilledBackground();
        gameObject.GetComponent<Collider2D>().enabled = false;
        enemyTweener.AbortTween();
        enemyAnim.SetTrigger("isDead");
        isDead = true;
        CurrentStatus = EnemyStatus.Dead;
        //respawn to spawn point;
        ObjectSpawner.EnemyRespawn(transform);
        yield return new WaitForSeconds(5f);
        enemyAnim.SetTrigger("isRevived");
        switch (transform.GetChild(0).name)
        {
            case "Ghost1Canvas(Clone)":
                posX = 6;
                posY = 12;
                break;
            case "Ghost2Canvas(Clone)":
                //Ghost2Behaviour
                posX = 14;
                posY = 12;
                break;
            case "Ghost3Canvas(Clone)":
                //Ghost3Behaviour
                posX = 14;
                posY = 16;
                break;
            case "Ghost4Canvas(Clone)":
                posX = 27;
                posY = 27;
                break;
            default:
                Debug.Log("Error");
                break;
        }
        isDead = false;
        bg.PlayNormalBackground();
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
}