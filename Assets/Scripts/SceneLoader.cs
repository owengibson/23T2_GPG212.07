using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoundCipher
{
    public class SceneLoader : MonoBehaviour
    {
       public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}
