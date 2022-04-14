using UnityEngine;

public class AudioSystem : Singleton<AudioSystem>
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;

    public void PlayMusic(AudioClip clip)
    {
        this.musicSource.clip = clip;
        this.musicSource.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1)
    {
        this.soundSource.transform.position = position;
        this.PlaySound(clip, volume);
    }

    public void PlaySound(AudioClip clip, float volume = 1)
    {
        this.soundSource.PlayOneShot(clip, volume);
    }
}
