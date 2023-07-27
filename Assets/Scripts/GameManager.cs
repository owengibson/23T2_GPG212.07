using EasyAudioSystem;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoundCipher
{
    public enum GameMode { Easy, Normal };

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] sounds;
        [SerializeField] private GameObject[] solutionSlots;
        [Space]
        [SerializeField] private SoundButton[] soundButtons;
        [SerializeField] private Button[] otherButtons;
        [Space]

        [SerializeField] private Texture speakerImage;
        [SerializeField] private Texture blankImage;
        [SerializeField] private Color32 activeTurnColor = new Color32(157, 255, 132, 255);
        [SerializeField] private CodeGenerator codeGenerator;
        [SerializeField] private ClueCalculator clueCalculator;
        [SerializeField] private GameObject victoryPanel;

        public GameMode gameMode = GameMode.Normal;
        public int turnIndex = 0;

        private GameObject[,] _soundSlots = new GameObject[8, 4];
        private GameObject _nextSlot;

        private int _slotNumber = 0;
        private bool _isLastSlotFilled = false;
        private string _slotName = "Blank Slot";


        private void Awake()
        {
            //gameMode = (GameMode)Enum.Parse(typeof(GameMode), PlayerPrefs.GetString("GameMode"));

            int i = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    _soundSlots[x,y] = sounds[i];
                    i++;
                }
            }

            _nextSlot = _soundSlots[0,0];  
        }

        public void OccupySlot()
        {
            RawImage slotImage = _nextSlot.GetComponent<RawImage>();
            if (slotImage.texture != blankImage) return;

            slotImage.color = Color.white;
            slotImage.texture = speakerImage;

            Button slotButton = _nextSlot.GetComponent<Button>();
            SoundButton slotSoundButton = EventSystem.current.currentSelectedGameObject.GetComponent<SoundButton>();
            slotButton.onClick.AddListener(slotSoundButton.PlaySound);

            _nextSlot.name = slotSoundButton.soundName;

            if (_slotNumber != 3)
            {
                _slotNumber++;
                _nextSlot = _soundSlots[turnIndex, _slotNumber];
            }
            else _isLastSlotFilled = true;
        }

        public void VacateSlot()
        {
            if (!_isLastSlotFilled && _slotNumber != 0)
            {
                _slotNumber--;
                _nextSlot = _soundSlots[turnIndex, _slotNumber];
            }
            else _isLastSlotFilled = false;

            RawImage slotImage = _nextSlot.GetComponent<RawImage>();
            slotImage.texture = blankImage;
            slotImage.color = activeTurnColor;

            Button slotButton = _nextSlot.GetComponent<Button>();
            slotButton.onClick.RemoveAllListeners();

            _nextSlot.name = _slotName;
        }

        public void SubmitGuess()
        {
            string[] guess = new string[4];
            for (int i = 0; i < 4; i++)
            {
                guess[i] = _soundSlots[turnIndex, i].name;
            }

            if (guess.Contains("Blank Slot"))
            {
                Debug.Log("Guess not completed.");
                return;
            }
            Debug.Log($"Submitted guess is: {guess[0]}, {guess[1]}, {guess[2]}, {guess[3]}");

            if (Enumerable.SequenceEqual(guess, codeGenerator.code))
            {
                // WIN !!!
                victoryPanel.SetActive(true);
                StartCoroutine(PlaySolution());
                foreach(GameObject slot in solutionSlots)
                {
                    slot.GetComponent<RawImage>().texture = speakerImage;
                }
                DisableInteractables();
            }
            else
            {
                clueCalculator.CalculateClues(guess, codeGenerator.code);

                turnIndex++;
                _slotNumber = 0;
                _nextSlot = _soundSlots[turnIndex, _slotNumber];
                for (int i = 0;i < 4;i++)
                {
                    _soundSlots[turnIndex, i].GetComponent<RawImage>().color = activeTurnColor;
                }
            }
        }

        private IEnumerator PlaySolution()
        {
            for (int i = 0; i < solutionSlots.Length; i++)
            {
                Button slotButton = solutionSlots[i].GetComponent<Button>();
                SoundButton slotSoundButton = solutionSlots[i].AddComponent<SoundButton>();
                slotSoundButton.soundName = codeGenerator.code[i];
                slotButton.onClick.AddListener(slotSoundButton.PlaySound);

                AudioManager.PlayAudio(codeGenerator.code[i]);
                yield return new WaitForSeconds(0.6f);
            }
        }

        public void DisableInteractables()
        {
            foreach (var button in soundButtons) button.enabled = false;
            foreach (var button in otherButtons) button.enabled = false;
        }

        public void EnableInteractables()
        {
            foreach (var button in soundButtons) button.enabled = true;
            foreach (var button in otherButtons) button.enabled = true;
        }
    }
}
