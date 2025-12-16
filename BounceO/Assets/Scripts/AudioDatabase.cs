using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "AudioDatabase",
    menuName = "Audio/Audio Database"
)]
public class AudioDatabase : ScriptableObject
{
    public List<AudioEntry> sounds = new List<AudioEntry>();

    Dictionary<string, AudioClip> lookup;

    void OnEnable()
    {
        BuildLookup();
    }

    void BuildLookup()
    {
        lookup = new Dictionary<string, AudioClip>();

        foreach (var s in sounds)
        {
            if (!lookup.ContainsKey(s.key) && s.clip != null)
            {
                lookup.Add(s.key, s.clip);
            }
        }
    }

    public AudioClip Get(string key)
    {
        if (lookup == null)
            BuildLookup();

        lookup.TryGetValue(key, out var clip);
        return clip;
    }
}
