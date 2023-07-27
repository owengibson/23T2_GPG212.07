using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoundCipher
{
    public class GamemodeSelector : MonoBehaviour
    {
        public void PlayGameOnGamemode(string gamemode)
        {
            PlayerPrefs.SetString("GameMode", gamemode);
            SceneManager.LoadScene("Main");
        }
    }
}
