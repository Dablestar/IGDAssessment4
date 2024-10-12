using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAudioPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioList;

    private AudioSource source;

    private int index = 0;
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        InvokeRepeating(nameof(TestBackgroundCoroutine), 0f,  10f);
    }


    private void TestBackgroundCoroutine()
    {
        source.Stop();
        source.clip = audioList[index];
        if (index == 3) index = 0;
        else index++;
        source.Play();
    }
}
