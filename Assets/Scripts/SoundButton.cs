using EasyAudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundCipher
{
    public class SoundButton : MonoBehaviour
    {
        public string soundName;

        private void Awake()
        {
            soundName = gameObject.name;
        }

        public void PlaySound()
        {
            AudioManager.PlayAudio(soundName);
        }
    }
}
