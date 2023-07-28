using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SoundCipher
{
    public class ClueCalculator : MonoBehaviour
    {
        [SerializeField] private GameObject[] clues;
        [Space]

        [SerializeField] private GameManager gameManager;

        private GameObject[,] _clueSlots = new GameObject[8, 4];
        private GameObject _nextSlot;

        public void CalculateClues(string[] guess, string[] solution)
        {
            if (guess.Length != 4 && solution.Length != 4)
            {
                Debug.Log("Guess and/or code array are incorrect size.");
                return;
            }

            int slotNumber = 0;
            List<int> blacklist = new();

            // Right sound, right place
            for (int i = 0; i < guess.Length; i++)
            {
                _nextSlot = _clueSlots[gameManager.turnIndex, gameManager.gameMode == GameMode.Normal ? slotNumber : i];

                if (guess[i] == solution[i])
                {
                    Debug.Log($"Guess slot {i} is the correct sound in the correct spot.");
                    _nextSlot.GetComponent<RawImage>().color = Color.green;
                    slotNumber++;
                    blacklist.Add(i);
                }
            }

            slotNumber = 0;
            // Right sound, wrong place
            for (int i = 0; i < guess.Length; i++)
            {
                if (blacklist.Contains(i)) continue;

                _nextSlot = _clueSlots[gameManager.turnIndex, gameManager.gameMode == GameMode.Normal ? slotNumber : i];
                
                if (solution.Contains(guess[i]))
                {
                    Debug.Log($"Guess slot {i} is the correct sound in the wrong spot.");
                    _nextSlot.GetComponent<RawImage>().color = Color.yellow;
                    slotNumber++;
                }
            }
        }

        private void Start()
        {
            int i = 0;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    _clueSlots[x, y] = clues[i];
                    i++;
                }
            }

            _nextSlot = _clueSlots[0, 0];
        }
    }
}
