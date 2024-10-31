using System.Collections;
using System.Collections.Generic;
using System.Threading;
using dir;
using Unity.Mathematics;
using UnityEngine;

public class Tweener : MonoBehaviour {
    //private Tween activeTween;
    private List<Tween> activeTweens;
    private List<Vector2> movingArr;
    private List<string[]> map;

    void Awake() {
        activeTweens = new List<Tween>();
        map = LevelGenerator.MapInfo;
    }
    
    void Update() {
            Tween activeTween;
            for (int i = activeTweens.Count-1; i >=0; i--)
            {
                activeTween = activeTweens[i];
                if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.1f) {
                    float timeFraction = (Time.time - activeTween.StartTime) / activeTween.Duration;
                    activeTween.Target.position = Vector3.Lerp(activeTween.StartPos,
                        activeTween.EndPos,
                        timeFraction);                
                } else {
                    activeTween.Target.position = activeTween.EndPos;
                    activeTweens.RemoveAt(i);
                }
            }
    }

    public bool AddTween(Transform targetObject, Vector2 startPos, Vector2 endPos, float duration)
    {
        if (!TweenExists(targetObject))
        {
            activeTweens.Add(new Tween(targetObject, new Vector3(startPos.x, startPos.y, -1), new Vector3(endPos.x, endPos.y, -1), Time.time, duration));
            return true;
        }
        return false;
    }
    public bool Move(Transform targetObject, Vector2 startPos, Direction direction, float moveSpeed, int x, int y)
    {
        movingArr = new List<Vector2>()
        {
            new(targetObject.transform.position.x, targetObject.transform.position.y+1),
            new(targetObject.transform.position.x, targetObject.transform.position.y-1),
            new(targetObject.transform.position.x-1, targetObject.transform.position.y),
            new(targetObject.transform.position.x+1, targetObject.transform.position.y)
        };
        if (IsWalkable(direction, x, y) && !TweenExists(targetObject) && direction != Direction.None)
        {
            AddTween(targetObject, startPos, movingArr[(int)direction], moveSpeed);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TweenExists(Transform target) {
        foreach (Tween activeTween in activeTweens) {
            if (activeTween.Target.transform == target)
                return true;
        }
        return false;
    }
    private bool IsWalkable(Direction direction, int x, int y)
    {
        //neighbor indexing
        string upNeighbor = y > 0 ? map[y - 1][x] : "0";
        string downNeighbor = y < map.Count - 1 ? map[y + 1][x] : "0";
        string leftNeighbor = x > 0 ? map[y][x - 1] : "0";
        string rightNeighbor = x < map[y].Length - 1 ? map[y][x + 1] : "0";

        bool isWall(string neighbor)
        {
            return !(neighbor.Equals("0") || neighbor.Equals("5") || neighbor.Equals("6"));
        }
        
        switch (direction)
        {
            case Direction.Up:
                return !isWall(upNeighbor);
            case Direction.Down:
                return !isWall(downNeighbor);
            case Direction.Left:
                return !isWall(leftNeighbor);
            case Direction.Right:
                return !isWall(rightNeighbor);
            default:
                return false;
        }
    }

    public void AbortTween()
    {
        activeTweens.Clear();
    }
}