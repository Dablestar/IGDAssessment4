using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
            GameObject.Find("EnemySpawnPoint").transform.position,
            GameObject.Find("EnemySpawnPoint").transform.GetChild(1).position,
            GameObject.Find("EnemySpawnPoint").transform.GetChild(2).position,
            GameObject.Find("EnemySpawnPoint").transform.GetChild(3).position,
        };
        Instantiate(player, playerSpawnPoint, Quaternion.identity, GameObject.Find("LevelGenerator").transform);

        for (int i = 0; i < 4; i++)
        {
            var temp = Instantiate(enemy, enemySpawnPointList[i], Quaternion.identity, GameObject.Find("LevelGenerator").transform);
            enemyList.Add(temp);
            var canvasTemp = Instantiate(new GameObject($"Ghost{i + 1}Canvas"), temp.transform).AddComponent<Canvas>();
            var textTemp = canvasTemp.AddComponent<TextMeshPro>();
            textTemp.text = $"{i + 1}";
            textTemp.fontSize = 5;
            textTemp.alignment = TextAlignmentOptions.Center;
            textTemp.rectTransform.anchoredPosition = new Vector2(0, 1);
        }
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