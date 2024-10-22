using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    private List<GameObject> enemyList;

    private Vector3 playerSpawnPoint;
    private List<Vector3> enemySpawnPointList;

    private static int playerLife = 3;

    public static int PlayerLife
    {
        get;
        set;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyList = new List<GameObject>();
        playerSpawnPoint = GameObject.Find("PlayerSpawnPoint").transform.position;
        enemySpawnPointList = new List<Vector3>()
        {
            GameObject.Find("EnemySpawnPoint").transform.GetChild(0).position,
            GameObject.Find("EnemySpawnPoint").transform.GetChild(1).position,
            GameObject.Find("EnemySpawnPoint").transform.GetChild(2).position,
            GameObject.Find("EnemySpawnPoint").transform.GetChild(3).position,
        };
        Instantiate(player, playerSpawnPoint, Quaternion.identity, GameObject.Find("Map").transform);
        
        enemyList.Add(Instantiate(enemy, enemySpawnPointList[0], Quaternion.identity, GameObject.Find("Map").transform));
        enemyList.Add(Instantiate(enemy, enemySpawnPointList[1], Quaternion.identity, GameObject.Find("Map").transform));
        enemyList.Add(Instantiate(enemy, enemySpawnPointList[2], Quaternion.identity, GameObject.Find("Map").transform));
        enemyList.Add(Instantiate(enemy, enemySpawnPointList[3], Quaternion.identity, GameObject.Find("Map").transform));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool Respawn()
    {
        if (playerLife > 0)
        {
            return true;
        }
        return false;
    }
}