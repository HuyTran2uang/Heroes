using UnityEngine;

public class AudioData
{
    public Audio Audio { get; private set; }
    public AudioSource Source { get; private set; }

    public AudioData(Audio audio, AudioSource source)
    {
        this.Audio = audio;
        this.Source = source;
        Source.clip = Audio.clip;
        Source.loop = Audio.loop;
    }

    public void Play(bool mute = false, float volume = 1)
    {
        Source.volume = volume;
        Source.mute = mute;
        Source.Play();
    }

    public void PlayOnceShot(bool mute = false, float volume = 1)
    {
        Source.mute = mute;
        Source.PlayOneShot(Audio.clip, volume);
    }

    public void Pause()
    {
        if (Source.isPlaying)
        {
            Source.Pause();
        }
    }
}
