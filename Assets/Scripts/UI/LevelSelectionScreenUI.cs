using System;
using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;

namespace UI
{
    public class LevelSelectionScreenUI : BaseScreen
    {
        public Action<int> OnLevelSelect;

        [SerializeField] private LevelInfo _levelInfoPrefab;
        [SerializeField] private Transform _contentParent;

        

        public void Initialize()
        {
            CreateSelectionView();
        }

        public override void ShowScreen()
        {
            base.ShowScreen();
            ShowAnimation();
        }

        public override void HideScreen()
        {
            base.HideScreen();
            
            HideAnimation();
            ClearSelectionView();
        }

        private void ClearSelectionView()
        {
            foreach (Transform child in _contentParent)
            {
                Destroy(child.gameObject);
            }
        }
        private void CreateSelectionView()
        {
            List<string> titleList = LevelSo.Instance.GetLevelTitleList();

            for (int i = 0; i < titleList.Count; i++)
            {
                var levelInfo = Instantiate(_levelInfoPrefab, _contentParent);
                var reachedMaxLevel = PersistentDataManager.GetReachedLevel();
                var isLocked = reachedMaxLevel < i + 1;
                var highScore = PersistentDataManager.GetHighScore(i + 1).ToString();

                var txt = isLocked ? "Level Locked" : highScore;

                levelInfo.Initialize(i, titleList[i], txt, isLocked, OnLevelSelect);
            }
        }
        private void ShowAnimation()
        {
            // Animation
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(()=>transform.localPosition = Vector3.zero);
            sequence.Append(_contentParent.DOLocalMoveY(_contentParent.transform.localPosition.y + 2000, 0f));
            sequence.Append(_contentParent.DOLocalMoveY(_contentParent.transform.localPosition.y, 2f)).SetEase(Ease.OutQuad);
        }
        private void HideAnimation()
        {
            // Animation
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalMoveX(transform.localPosition.x - 3000, .75f)).SetEase(Ease.InCubic);
        }
    }
}