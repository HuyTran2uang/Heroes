using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    Dictionary<AudioName, AudioData> _audioDict = new Dictionary<AudioName, AudioData>();
    bool _isOpenMusic = true, _isOpenSound = true;
    private float _musicVolume, _soundVolume;

    public bool IsOpenSound => _isOpenSound;
    public bool IsOpenMusic => _isOpenMusic;

    private void Awake()
    {
        _audioDict = new Dictionary<AudioName, AudioData>();
        foreach (var audio in AudioStorage.Instance.Audios)
        {
            GameObject obj = new GameObject($"{audio.name}");
            obj.transform.SetParent(transform);
            _audioDict[audio.name] = new AudioData(audio, obj.AddComponent<AudioSource>());
        }
    }

    public void Music(out bool isOpen)
    {
        _isOpenMusic = !_isOpenMusic;
        foreach (var name in _audioDict.Keys)
        {
            if (_audioDict[name].Audio.type == AudioType.Music)
            {
                _audioDict[name].Source.volume = _musicVolume;
                _audioDict[name].Source.mute = !_isOpenMusic;
            }
        }
        isOpen = _isOpenMusic;
    }

    public void Sound(out bool isOpen)
    {
        _isOpenSound = !_isOpenSound;
        foreach (var name in _audioDict.Keys)
        {
            if (_audioDict[name].Audio.type == AudioType.Sound)
            {
                _audioDict[name].Source.volume = _soundVolume;
                _audioDict[name].Source.mute = !_isOpenSound;
            }
        }
        isOpen = _isOpenSound;
    }

    public void ChangeVolume(AudioType audioType, float volume)
    {
        foreach (var name in _audioDict.Keys)
        {
            if (_audioDict[name].Audio.type == audioType)
            {
                _audioDict[name].Source.volume = _musicVolume;
            }
        }
    }

    public void PlayAudio(AudioName name, float volume = 1)
    {
        AudioData audioData = _audioDict[name];
        bool mute = (audioData.Audio.type == AudioType.Music) ? !_isOpenMusic : !_isOpenSound;
        audioData.Play(mute, audioData.Audio.volume * volume);
    }

    public void PlayAudioOnceShot(AudioName name, float volume = 1)
    {
        AudioData audioData = _audioDict[name];
        bool mute = (audioData.Audio.type == AudioType.Music) ? !_isOpenMusic : !_isOpenSound;
        audioData.PlayOnceShot(mute, audioData.Audio.volume * volume);
    }

    public void PauseAudio(AudioName name)
    {
        _audioDict[name]?.Pause();
    }
}
