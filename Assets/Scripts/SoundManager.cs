using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource sfxSource;

    public List<AudioClip> sfxClips;

    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitDictionaries()
    {
        foreach (var clip in sfxClips)
        {
            sfxDict[clip.name] = clip;
        }
    }
    public void PlaySFX(string name)
    {
        if (sfxDict.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX '{name}' not found!");
        }
    }
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
