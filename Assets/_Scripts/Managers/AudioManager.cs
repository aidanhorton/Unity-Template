using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] Sounds;

    private void Start()
    {
        // For each sound in the list, add audio source to the manager.
        foreach (var sound in this.Sounds)
        {
            sound.Source = this.gameObject.AddComponent<AudioSource>();

            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.DefaultVolume;
            sound.Source.pitch = sound.DefaultPitch;
            sound.Source.loop = sound.Loop;
            sound.Source.outputAudioMixerGroup = sound.Mixer;
        }
    }

    public void PlaySound(string soundName)
    {
        var sound = Array.Find(this.Sounds, s => s.Name == soundName);
        if (sound == null)
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return;
        }

        sound.Source.volume = sound.DefaultVolume;

        sound.Source.DOKill();
        sound.Source.Play();
    }

    public void StopSound(string soundName, float fadeTime = 0f)
    {
        var sound = Array.Find(this.Sounds, s => s.Name == soundName);
        if (sound == null)
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return;
        }

        sound.Source.DOFade(0, fadeTime);
    }
}

[Serializable]
public class Sound
{
    public string Name;

    public AudioClip Clip;

    public AudioMixerGroup Mixer;

    [Range(0f, 1f)]
    public float DefaultVolume = 0.5f;

    [Range(0.1f, 3f)]
    public float DefaultPitch = 1f;

    public bool Loop;

    [HideInInspector]
    public AudioSource Source;
}
