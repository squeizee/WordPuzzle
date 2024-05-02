using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class MainScreenUI : BaseScreen
    {
        public Action OnLevelButtonClicked;
        
        [SerializeField] private Button levelsButton;
        
        private void Start()
        {
            levelsButton.onClick.AddListener(ButtonClicked);
        }
        
        public override void ShowScreen()
        {
            base.ShowScreen();
        }

        public override void HideScreen()
        {
            base.HideScreen();
        }

        private void ButtonClicked()
        {
            ButtonHideAnimation();
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                OnLevelButtonClicked?.Invoke();
            });
        }
        
        private void ButtonHideAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(levelsButton.transform.DOScale(Vector3.zero, 1f));
            
        }
    }
}