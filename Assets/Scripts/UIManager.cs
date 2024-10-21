using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    private Image borderLine;
    private float destination;
    private Color[] colorList;
    private int startIndex;
    private int endIndex;
    private float time;
    
    // Start is called before the first frame update
    void Start()
    {
        startIndex = 0;
        endIndex = 1;
        colorList = new Color[]
        {
            Color.white,
            Color.blue,
            Color.red,
            Color.black
        };
        time = 1.5f;
        borderLine = GameObject.Find("Border").GetComponent<Image>();
        Debug.Log(borderLine.color == Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        BorderColorChange();
    }
    
    public void MoveToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void MoveToDesignIteration()
    {
        Debug.Log("Not Implemented");
    }

    void BorderColorChange()
    {
        destination += Time.deltaTime / time;
        borderLine.color = Color.Lerp(colorList[startIndex], colorList[endIndex], destination);
        Debug.Log(borderLine.color + " ||" + colorList[endIndex] + " DEST : " + destination );
        Debug.Log(startIndex + ", " + endIndex);
        if (destination >= 1f)
        {
            destination = 0f;
            startIndex = endIndex;
            endIndex = endIndex >= 3 ? 0 : endIndex += 1;
        }
    }
    
}
