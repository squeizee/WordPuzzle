using System;
using System.Collections.Generic;
using System.Linq;
using Commands;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Tiles;
using TMPro;
using UnityEngine.Serialization;

namespace UI
{
    public class GameScreenUI : BaseScreen
    {
        public Action OnSubmitClicked;
        public Action OnUndoClicked;
        public Action OnContinueClicked;
        
        [SerializeField] private Button submitButton;
        [SerializeField] private Button undoButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Word wordPrefab;
        [SerializeField] private Transform wordsParent;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private GameObject fireworkVFX;
        [SerializeField] private GameObject confettiVFX;

        private List<GameObject> _vfxObjects = new();
        private int _score;
        private void Start()
        {
            submitButton.onClick.AddListener(() =>
            {
                OnSubmitClicked?.Invoke();
            });
            undoButton.onClick.AddListener(() =>
            {
                OnUndoClicked?.Invoke();
            });
            continueButton.onClick.AddListener(() =>
            {
                OnContinueClicked?.Invoke();
                ClearVFX();
                HideScreen();
            });
        }
        public override void ShowScreen()
        {
            base.ShowScreen();
            
            titleText.text = LevelSo.Instance.GetCurrentLevelTitle();
        }

        public override void HideScreen()
        {
            base.HideScreen();
            
            ClearWords();
            _score = 0;
            scoreText.text = $"Score: {_score}";
            continueButton.gameObject.SetActive(false);
        }

        public void UpdateSubmitButtonState(bool state)
        {
            submitButton.interactable = state;
        }
        public void UpdateUndoButtonState(bool state)
        {
            undoButton.interactable = state;
        }
        public void AddWord(string word)
        {
            var wordObject = Instantiate(wordPrefab, wordsParent);
            wordObject.SetWord(word);
        }
        public void AddScore(int score)
        {
            _score += score;
            scoreText.text = $"Score: {_score}";
        }
        public void UpdateScore(int score)
        {
            _score = score;
            scoreText.text = $"Score: {_score}";
        }
        public void ShowLevelEnd(bool isHighScore)
        {
            ShowWinVFX(isHighScore);
            DOVirtual.DelayedCall(2f, ()=>continueButton.gameObject.SetActive(true));
        }
        private void ShowWinVFX(bool isHighScore)
        {
            if (isHighScore)
            {
               _vfxObjects.Add(Instantiate(fireworkVFX));
            }
            
            _vfxObjects.Add(Instantiate(confettiVFX));
            
        }
        private void ClearVFX()
        {
            foreach (var vfxObject in _vfxObjects)
            {
                Destroy(vfxObject);
            }
        }
        
        private void ClearWords()
        {
            foreach (Transform child in wordsParent)
            {
                Destroy(child.gameObject);
            }
        }
        
    }
}