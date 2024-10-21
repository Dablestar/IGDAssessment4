using System;
using UnityEngine;
using UnityEngine.UI;


public class BorderColorChanger : MonoBehaviour
    {
        private Image borderLine;
        private float destination;
        private Color[] colorList;
        private int startIndex;
        private int endIndex;
        private float time;

        private void Start()
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
        }

        private void Update()
        {
            throw new NotImplementedException();
        }
        
        void BorderColorChange()
        {
            destination += Time.deltaTime / time;
            borderLine.color = Color.Lerp(colorList[startIndex], colorList[endIndex], destination);
            if (destination >= 1f)
            {
                destination = 0f;
                startIndex = endIndex;
                endIndex = endIndex >= 3 ? 0 : endIndex += 1;
            }
        }
    }
