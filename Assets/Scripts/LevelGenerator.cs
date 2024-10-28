using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    private const string path = "Assets/PacManLevelMap.csv";
    [SerializeField] private GameObject[] tilePalette;
    private List<string[]> mapInfo;
    private Vector3 generatorCoordinate;

    private GameObject parent;
    private Tilemap wallParent;
    private Tilemap palletParent;

    // Start is called before the first frame update
    void Awake()
    {
        mapInfo = new List<string[]>();
        GetMapInfoFromFile();
        parent = GameObject.Find("Map");
        wallParent = Instantiate(new GameObject("Walls"), transform).AddComponent<Tilemap>();
        palletParent = Instantiate(new GameObject("Pallets"), transform).AddComponent<Tilemap>();
        wallParent.transform.position = new Vector3(0, 0, -1);
        palletParent.transform.position = new Vector3(0, 0, -1);
        CreateMap();
        
        Camera.main.orthographicSize = Screen.height / (tilePalette[1].transform.localScale.x * 30);
    }

    public void GetMapInfoFromFile()
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
        TextReader reader = new StreamReader(fileStream);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            mapInfo.Add(line.Split(","));
        }

        reader.Close();
    }


    public void CreateMap()
    {
        Vector3Int position = Vector3Int.FloorToInt(wallParent.transform.position);
        int origin = position.x;
        for (int y = 0; y < mapInfo.Count; y++)
        {
            for (int x = 0; x < mapInfo[y].Length; x++)
            {
                //Neighbor Indexing
                string upNeighbor = y > 0 ? mapInfo[y - 1][x] : "0";
                string downNeighbor = y < mapInfo.Count - 1 ? mapInfo[y + 1][x] : "0";
                string leftNeighbor = x > 0 ? mapInfo[y][x - 1] : "0";
                string rightNeighbor = x < mapInfo[y].Length - 1 ? mapInfo[y][x + 1] : "0";

                bool upIsWall = !(upNeighbor.Equals("0") || upNeighbor.Equals("5") || upNeighbor.Equals("6"));
                bool downIsWall = !(downNeighbor.Equals("0") || downNeighbor.Equals("5") || downNeighbor.Equals("6"));
                bool rightIsWall = !(rightNeighbor.Equals("0") || rightNeighbor.Equals("5") || rightNeighbor.Equals("6"));
                bool leftIsWall = !(leftNeighbor.Equals("0") || leftNeighbor.Equals("5") || leftNeighbor.Equals("6"));
                
                //diagonal neighbor indexing
                string upLeftNeighbor =  y > 0 && x > 0 ? mapInfo[y - 1][x - 1] : "0";
                string upRightNeighbor =  y > 0 && x < mapInfo[y].Length - 1 ? mapInfo[y - 1][x + 1] : "0";
                string downLeftNeighbor = y < mapInfo.Count - 1 && x  > 0? mapInfo[y + 1][x - 1] : "0";
                string downRightNeighbor = y < mapInfo.Count - 1 && x  < mapInfo[y].Length - 1? mapInfo[y + 1][x + 1] : "0";
                
                bool upleftIsWall = !(upLeftNeighbor.Equals("0") || upLeftNeighbor.Equals("5") || upLeftNeighbor.Equals("6"));
                bool downleftIsWall = !(downLeftNeighbor.Equals("0") || downLeftNeighbor.Equals("5") || downLeftNeighbor.Equals("6"));
                bool uprightIsWall = !(upRightNeighbor.Equals("0") || upRightNeighbor.Equals("5") || upRightNeighbor.Equals("6"));
                bool downrightIsWall = !(downRightNeighbor.Equals("0") || downRightNeighbor.Equals("5") || downRightNeighbor.Equals("6"));

                bool specialCase1 = (!upIsWall && !downIsWall && rightIsWall && !leftIsWall);
                bool specialCase2 = (!upIsWall && !downIsWall && !rightIsWall && leftIsWall);


                Quaternion rotation = Quaternion.identity;
                switch (mapInfo[y][x])
                {
                    case "0":
                        Debug.Log($"x : {x}, y: {y}");
                        Instantiate(tilePalette[0], position, Quaternion.identity, wallParent.transform);
                        break;

                    case "2":
                    case "4":
                        //Base case : border
                        
                        //I tried my best, but there was still some errors on rotation.
                        //So, this code works with basic logic(check empty place around neighbor) + case by case solution. Please release solution later. I want to know better way to do this.
                        if (leftIsWall && rightIsWall)
                        {
                            rotation = Quaternion.Euler(0, 0, 90f);
                        }
                        else if (upIsWall && downIsWall)
                        {
                            rotation = Quaternion.identity;
                        }
                        else
                        {
                            rotation = Quaternion.Euler(0, 0, 90f);
                        }

                        Instantiate(tilePalette[2], position + new Vector3Int(0, 0, -1), rotation, wallParent.transform);
                        break;
                    case "1":
                    case "3":
                        if (!leftIsWall && rightIsWall)
                        {
                            if (downIsWall)
                                rotation *= Quaternion.Euler(0, 0, 270);
                            else if (upIsWall)
                                rotation *= Quaternion.Euler(0, 0, 0);
                        }
                        else if (leftIsWall && !rightIsWall)
                        {
                            if (downIsWall)
                                rotation *= Quaternion.Euler(0, 0, 180);
                            else if (upIsWall)
                                rotation *= Quaternion.Euler(0, 0, 90);
                        }
                        else if(leftIsWall && rightIsWall)
                        {
                            if (!upleftIsWall)
                            {
                                rotation *= Quaternion.Euler(0, 0, 90);
                            }else if (!downleftIsWall)
                            {
                                rotation *= Quaternion.Euler(0, 0, 180);
                            }else if (!uprightIsWall)
                            {
                                rotation *= Quaternion.identity;
                            }
                            else if(!downrightIsWall)
                            {
                                rotation *= Quaternion.Euler(0, 0, 270);
                            }
                        }

                        Instantiate(tilePalette[1], position + new Vector3Int(0, 0, 1), rotation, wallParent.transform);
                        break;
                    case "5":
                        Debug.Log($"x : {x}, y: {y}");
                        Instantiate(tilePalette[0], position, Quaternion.identity, wallParent.transform);
                        position.z --;
                        Instantiate(tilePalette[5], position, Quaternion.identity, palletParent.transform);
                        break;
                    case "6":
                        Debug.Log($"x : {x}, y: {y}");
                        Instantiate(tilePalette[0], position, Quaternion.identity, wallParent.transform);
                        position.z --;
                        Instantiate(tilePalette[6], position, Quaternion.identity, palletParent.transform);
                        break;
                    case "7":
                        Debug.Log($"x : {x}, y: {y}");
                        if (y == mapInfo.Count - 1)
                        {
                            rotation *= Quaternion.Euler(0, 0, 90f);
                        }
                        else
                        {
                            
                            if (x != 0 && x != mapInfo[y].Length - 1)
                            {
                                if ((downNeighbor.Equals("0") || rightNeighbor.Equals("2")) ||
                                    (upNeighbor.Equals("0") || leftNeighbor.Equals("2")))
                                {
                                    rotation *= Quaternion.Euler(0, 0, -90f);
                                }
                            }
                        }

                        Instantiate(tilePalette[7], position, rotation, wallParent.transform);
                        break;
                    default:
                        break;
                }

                position.z = 0;
                position += Vector3Int.right;
            }

            position.x = origin;
            position += Vector3Int.down;
        }
    }
}