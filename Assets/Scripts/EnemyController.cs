using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float moveSpeed = 2f;
    [SerializeField] private static Animator enemyAnim;
    public static bool isWeaken { get; set; }

    private AudioSource soundPlayer;

    private AudioSource backgroundSoundPlayer;

    private List<GameObject> enemyList;
    
    
    // Start is called before the first frame update
    void Start()
    {
        isWeaken = false;
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
        enemyAnim.SetTrigger(0);
        enemyAnim.SetFloat(1, 2.5f);
        yield return new WaitForSeconds(7f);
        //set animation recovering
        yield return new WaitForSeconds(3f);
        enemyAnim.SetTrigger(0);
        isWeaken = false;
    }
    
    
}
