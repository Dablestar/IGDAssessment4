using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CherryController : MonoBehaviour
{
    [SerializeField] private GameObject bonusCherry;
    private Tweener objTweener;
    private GameObject tempCherry;

    private List<Vector2> spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new List<Vector2>()
        {
            new(1.5f, -30f),
            new(1.5f, 10f),
            new(-33f, -9f),
            new(40f, -9f)
        };
        
        InvokeRepeating(nameof(SpawnCherry), 10, 10);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCherry()
    {
        int randVal = Random.Range(0, spawnPoint.Count);
        float movingTime = 10f;
        tempCherry = Instantiate(bonusCherry, spawnPoint[randVal], quaternion.identity,
            GameObject.Find("Map").transform);
        objTweener = tempCherry.GetComponent<Tweener>();
        switch (randVal)
        {
            case 0:
                objTweener.AddTween(tempCherry.transform, tempCherry.transform.position,
                    new Vector2(tempCherry.transform.position.x, tempCherry.transform.position.y + 100), movingTime);
                break;
            case 1:
                objTweener.AddTween(tempCherry.transform, tempCherry.transform.position,
                    new Vector2(tempCherry.transform.position.x, tempCherry.transform.position.y - 100), movingTime);
                break;
            case 2:
                objTweener.AddTween(tempCherry.transform, tempCherry.transform.position,
                    new Vector2(tempCherry.transform.position.x + 100, tempCherry.transform.position.y), movingTime);
                break;
            case 3:
                objTweener.AddTween(tempCherry.transform, tempCherry.transform.position,
                    new Vector2(tempCherry.transform.position.x - 100, tempCherry.transform.position.y), movingTime);
                break;
        }
        Invoke(nameof(DestroyCherry), 10f);
    }

    void DestroyCherry()
    {
        Destroy(tempCherry);
    }


}
