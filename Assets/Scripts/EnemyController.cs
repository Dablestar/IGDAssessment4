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
        Recovering,
        Dead
    }
    private float moveSpeed = 2f;
    private Animator enemyAnim;
    private bool isWeaken { get; set; }
    private bool isDead { get; set; }
    private BGAudioPlayer bg;

    private List<GameObject> enemyList;

    public EnemyStatus CurrentStatus { get; set; }
    


    // Start is called before the first frame update
    void Start()
    {
        isWeaken = false;
        isDead = false;
        enemyAnim = gameObject.GetComponent<Animator>();
        bg = GameObject.Find("BGPlayer").GetComponent<BGAudioPlayer>();
        CurrentStatus = EnemyStatus.Walking;
        enemyAnim.SetFloat("moveSpeed", moveSpeed);
        enemyAnim.SetInteger("movingDirection", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWeaken && !isDead)
        {
            moveSpeed = 2.5f;
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
        enemyAnim.SetTrigger("isDead");
        isDead = true;
        CurrentStatus = EnemyStatus.Dead;
        //respawn to spawn point;
        yield return new WaitForSeconds(5f);
    }
    
    
}
