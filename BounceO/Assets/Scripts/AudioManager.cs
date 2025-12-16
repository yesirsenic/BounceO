using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioDatabase database;
    [Header("Effect Sounds")]
    public AudioSource sfxSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(string key)
    {
        if (!GameManager.Instance.MusicOn)
            return;

        var clip = database.Get(key);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

}
