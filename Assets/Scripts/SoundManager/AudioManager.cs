using System.Collections.Generic;
using Interface;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundManager
{
    public class AudioManager : IService
    {
        private Dictionary<string, AudioClip> _sounds = new();
        private Dictionary<string, AudioMixerGroup> _mixerGroups = new();
        private AudioMixerGroup _masterMixerGroup;

        private float _musicVolume;
        private float _uiVolume;

        public float MusicVolume => _musicVolume;
        public float UIVolume => _uiVolume;

        public void Initialize(float musicVolume, float uiVolume)
        {
            _musicVolume = musicVolume;
            _uiVolume = uiVolume;

            _masterMixerGroup = Resources.Load<AudioMixerGroup>("Sound/Mixers/Main");
            
            CacheMixerGroups(_masterMixerGroup.audioMixer);
            SetMusicVolume(musicVolume);
            SetUiVolume(uiVolume);

            LoadAllSounds();
        }

        private void CacheMixerGroups(AudioMixer mixer)
        {
            string[] groupNames = { "Music", "UI" };

            foreach (var name in groupNames)
            {
                var groups = mixer.FindMatchingGroups(name);
                if (groups.Length > 0)
                {
                    _mixerGroups[name] = groups[0];
                }
                else
                {
                    Debug.LogWarning($"Mixer group '{name}' not found.");
                }
            }
        }

        public void ChangeMusicVolume(float musicVolume)
        {
            _masterMixerGroup.audioMixer.SetFloat("Music", musicVolume);
            _musicVolume = musicVolume;
        }

        public void ChangeUiVolume(float uiVolume)
        {
            _masterMixerGroup.audioMixer.SetFloat("UI", uiVolume);
            _uiVolume = uiVolume;
        }

        public void PlaySound(string soundName, string mixer)
        {
            if (!_sounds.ContainsKey(soundName))
            {
                Debug.LogWarning($"Sound '{soundName}' not found!");
                return;
            }

            var sound = new GameObject("Sound");
            var audioSource = sound.AddComponent<AudioSource>();
            audioSource.clip = _sounds[soundName];
            audioSource.outputAudioMixerGroup = _mixerGroups.GetValueOrDefault(mixer, _masterMixerGroup);
            audioSource.Play();

            Object.Destroy(sound, audioSource.clip.length);
        }

        private void LoadAllSounds()
        {
            var sounds = Resources.LoadAll<AudioClip>("Sound");
            foreach (var sound in sounds)
            {
                _sounds.Add(sound.name, sound);
            }
        }

        private void SetMusicVolume(float volume)
        {
            _masterMixerGroup.audioMixer.SetFloat("Music", volume);
        }

        private void SetUiVolume(float volume)
        {
            _masterMixerGroup.audioMixer.SetFloat("UI", volume);
        }
    }
}