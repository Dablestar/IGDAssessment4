using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAudioPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioList;

    public List<AudioClip> AudioList
    {
        get { return audioList; }
    }

    private AudioSource source;

    public AudioSource Source
    {
        get
        {
            return source;
        }
    }

    void Awake()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    public void PlayKilledBackground()
    {
        source.clip = audioList[3];
        source.Play();
    }

    public void PlayWeakenBackground()
    {
        source.clip = audioList[2];
        source.Play();
    }

    public void PlayNormalBackground()
    {
        source.clip = audioList[1];
        source.Play();
    }
}
