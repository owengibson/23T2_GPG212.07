using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoundCipher
{
    public class CodeGenerator : MonoBehaviour
    {
        public string[] code = new string[4];

        private string[] soundBank = new string[8] { "C_low", "D", "E", "F", "G", "A", "B", "C_high" };

        private void Start()
        {
            for (int i = 0; i < code.Length; i++)
            {
                code[i] = soundBank[Random.Range(0, soundBank.Length)];
            }
            Debug.Log($"The code is: {code[0]}, {code[1]}, {code[2]}, {code[3]}");
        }
    }
}
