using System;
using UnityEngine;
using UnityEngine.UI;

namespace EasyAudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        public Sound[] sounds;

        public static Action<string> PlayAudio;
        public static Action StopAudio;

        [SerializeField] private RawImage buttonIcon;
        [SerializeField] private Texture speakerOn;
        [SerializeField] private Texture speakerOff;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Play();
        }

        public void Stop()
        {
            foreach(Sound s in sounds)
            {
                s.source.Stop();
            }
        }

        public void Mute(string name)
        {
            Sound s = Array.Find(sounds, sounds => sounds.name == name);
            s.volume = 0;
        }

        public void Unmute(string name, float volume)
        {
            Sound s = Array.Find(sounds, sounds => sounds.name == name);
            s.volume = volume;
        }

        private void Start()
        {
            Play("Soundtrack");
        }

        public void ToggleSoundtrack()
        {
            Sound s = Array.Find(sounds, sounds => sounds.name == "Soundtrack");
            if (s.source.volume == 0)
            {
                s.source.volume = 0.2f;
                buttonIcon.texture = speakerOn;
            }
            else
            {
                s.source.volume = 0;
                buttonIcon.texture = speakerOff;
            }
        }

        private void OnEnable()
        {
            PlayAudio += Play;
            StopAudio += Stop;
        }
        private void OnDisable()
        {
            PlayAudio -= Play;
            StopAudio -= Stop;
        }
    }
}