using EasyAudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoundCipher
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] sounds;
        [Space]

        [SerializeField] private Texture speakerImage;
        [SerializeField] private Texture blankImage;
        [SerializeField] private Color32 activeTurnColor = new Color32(157, 255, 132, 255);
        [SerializeField] private CodeGenerator codeGenerator;
        
        private AudioManager _audioManager;

        private GameObject[,] _soundSlots = new GameObject[8, 4];
        private GameObject _nextSlot;

        private int _turnIndex = 0;
        private int _slotNumber = 0;
        private bool _isLastSlotFilled = false;
        private string _slotName = "Blank Slot";


        private void Awake()
        {
            int i = 0;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    _soundSlots[x,y] = sounds[i];
                    i++;
                }
            }

            _nextSlot = _soundSlots[_turnIndex, _slotNumber];

            _audioManager = FindObjectOfType<AudioManager>();
            
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
                _nextSlot = _soundSlots[_turnIndex, _slotNumber];
            }
            else _isLastSlotFilled = true;
        }

        public void VacateSlot()
        {
            if (!_isLastSlotFilled)
            {
                _slotNumber--;
                _nextSlot = _soundSlots[_turnIndex, _slotNumber];
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
                guess[i] = _soundSlots[_turnIndex, i].name;
            }
            Debug.Log($"Submitted guess is: {guess[0]}, {guess[1]}, {guess[2]}, {guess[3]}");

            if(guess == codeGenerator.code)
            {
                // WIN !!!
            }
            else
            {
                // CLUE LOGIC HERE
                // --------------//

                _turnIndex++;
                for (int i = 0;i < 4;i++)
                {
                    _soundSlots[_turnIndex, i].GetComponent<RawImage>().color = activeTurnColor;
                }
            }
        }
    }
}
