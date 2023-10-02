using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip clip;
        [Range(0, 1)]
        public float volume = 1f;
        public float cooldown = 2f; // Cooldown period for this sound effect.
        [HideInInspector]
        public float lastPlayTime; // Time when the last play occurred.
    }

    public List<SoundEffect> soundEffects;
    private Dictionary<string, AudioSource> audioSources;

    private void Awake()
    {
        audioSources = new Dictionary<string, AudioSource>();
        foreach (var soundEffect in soundEffects)
        {
            // Create an AudioSource for each sound effect.
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = soundEffect.clip;
            audioSource.volume = soundEffect.volume;
            audioSources[soundEffect.name] = audioSource;
        }
    }

    // Add a method to play a sound effect with cooldown.
    public void PlaySoundEffect(string soundEffectName)
    {
        if (audioSources.ContainsKey(soundEffectName))
        {
            var soundEffect = soundEffects.Find(s => s.name == soundEffectName);
            if (Time.time - soundEffect.lastPlayTime >= soundEffect.cooldown)
            {
                audioSources[soundEffectName].PlayOneShot(soundEffect.clip);
                soundEffect.lastPlayTime = Time.time;
            }
        }
        else
        {
            Debug.LogWarning("Sound effect not found: " + soundEffectName);
        }
    }
}