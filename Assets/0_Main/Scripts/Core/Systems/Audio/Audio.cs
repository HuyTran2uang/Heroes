using UnityEngine;

[System.Serializable]
public class Audio
{
    public AudioName name;
    public AudioType type;
    public AudioClip clip;
    public bool loop;
    [Range(0, 1)]
    public float volume;
}

public enum AudioType
{
    Sound,
    Music
}

public enum AudioName
{
    
}