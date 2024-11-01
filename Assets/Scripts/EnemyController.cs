using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public enum EnemyStatus
    {
        Walking,
        Weaken,
        Recovering
    }
    private float moveSpeed = 2f;
    [SerializeField] private static Animator enemyAnim;
    public static bool isWeaken { get; set; }

    private AudioSource soundPlayer;

    private AudioSource backgroundSoundPlayer;

    private List<GameObject> enemyList;

    public static EnemyStatus CurrentStatus { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        isWeaken = false;
        CurrentStatus = EnemyStatus.Walking;
        enemyAnim.SetFloat(1, moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (isWeaken)
        {
            moveSpeed = 2.5f;
        }
    }

    public static IEnumerator WeakenEnemy()
    {
        if (!isWeaken)
        {
            isWeaken = true;
        }
        Debug.Log("IsWeaken");
        enemyAnim.SetTrigger(0);
        enemyAnim.SetFloat(1, 2.5f);
        CurrentStatus = EnemyStatus.Weaken;
        yield return new WaitForSeconds(5f);
        //set animation recovering
        CurrentStatus = EnemyStatus.Recovering;
        yield return new WaitForSeconds(5f);
        enemyAnim.SetTrigger(0);
        isWeaken = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (isWeaken)
            {
                StartCoroutine(KillEnemy());    
            }
        }
    }

    IEnumerator KillEnemy()
    {
        Debug.Log("Killed");
        yield return new WaitForSeconds(1f);
    }
    
    
}
