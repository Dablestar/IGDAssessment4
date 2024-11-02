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


    private static Vector3 playerSpawnPoint;
    private static List<Vector3> enemySpawnPointList;

    // Start is called before the first frame update
    void Awake()
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
        Instantiate(player, playerSpawnPoint, Quaternion.identity, GameObject.Find("LevelGenerator").transform);

        for (int i = 0; i < 4; i++)
        {
            var temp = Instantiate(enemy, enemySpawnPointList[i], Quaternion.identity,
                GameObject.Find("LevelGenerator").transform);
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

    public static bool Respawn(Transform player)
    {
        if (PacStudentController.PlayerLife > 0)
        {
            Debug.Log($"Respawning at: {playerSpawnPoint}");
            PacStudentController.StudentTweener.AbortTween();
            PacStudentController.StudentTweener.AddTween(player, player.position, playerSpawnPoint, 3f);
            return true;
        }

        return false;
    }

    public static void EnemyRespawn(Transform enemy)
    {
        Vector3 spawnPoint;
        EnemyController controller = enemy.GetComponent<EnemyController>();
        switch (enemy.transform.GetChild(0).name)
        {
            case "Ghost1Canvas(Clone)":
                spawnPoint = enemySpawnPointList[0];
                break;
            case "Ghost2Canvas(Clone)":
                spawnPoint = enemySpawnPointList[1];
                break;
            case "Ghost3Canvas(Clone)":
                spawnPoint = enemySpawnPointList[2];
                break;
            case "Ghost4Canvas(Clone)":
                spawnPoint = enemySpawnPointList[3];
                break;
            default:
                spawnPoint = new Vector3(0, 0, 0);
                Debug.Log("Error");
                break;

        }
        Debug.Log($"Respawning at: {spawnPoint}");
        PacStudentController.StudentTweener.AbortTween();
        PacStudentController.StudentTweener.AddTween(enemy, enemy.position, spawnPoint, 3f);
    }
}