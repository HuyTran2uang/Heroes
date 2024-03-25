using UnityEngine;

[CreateAssetMenu(fileName = "Audio Storage", menuName = "Audio/Audio Storage")]
public class AudioStorage : ScriptableObject
{
    private static AudioStorage _instance;

    public static AudioStorage Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<AudioStorage>("SO/Audio/Audio Storage");
            return _instance;
        }
    }

    [SerializeField] private Audio[] _audios;

    public Audio[] Audios => _audios;
}
