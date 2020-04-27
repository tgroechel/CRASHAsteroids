using UnityEngine.Audio;
using System;
using UnityEngine;

// To use sound outside this class:
// FindObjectOfType<AudioManager>().Play("GameWin");
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioClip gameWinClip;
    private AudioSource audioSource;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            //s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        //Play("Background");
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
        }
        audioSource.PlayOneShot(gameWinClip, s.source.volume);
    }
}
