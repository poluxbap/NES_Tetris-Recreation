using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public Sounds[] sounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach(Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.loop = s.loop;
            s.source.volume = s.volume;
        }
    }

    private void Start()
    {
        Play(Sounds.SoundName.Music);
    }

    public void Play(Sounds.SoundName name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);

        s.source.Play();
    }

}
