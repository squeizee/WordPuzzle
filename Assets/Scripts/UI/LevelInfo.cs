using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class LevelInfo : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private GameObject lockedObject;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text highScoreText;

        private const string LevelTitle = "Level - ";
        private const string HighScore = "High Score : ";
        private bool _isLocked;
        private int _level;

        public void Initialize(int level, string title, string txt, bool isLocked, Action<int> onClick)
        {
            _isLocked = isLocked;
            _level = level + 1;

            titleText.text = LevelTitle + $"{_level} : {title}";
            highScoreText.text = isLocked ? txt : HighScore + $"{txt}";

            playButton.gameObject.SetActive(!_isLocked);
            lockedObject.SetActive(_isLocked);

            playButton.onClick.AddListener(() => onClick(_level));
        }

        public void UnlockLevel()
        {
            _isLocked = false;
        }
    }
}