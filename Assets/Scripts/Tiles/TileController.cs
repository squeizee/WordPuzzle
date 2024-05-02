using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tiles
{
    public class TileController : MonoBehaviour
    {
        public Action OnTileClicked;

        [SerializeField] private TMP_Text characterText;
        [SerializeField] private SpriteRenderer darkOverlay;

        public List<int> remainingChildren;

        private Level.Tile _tileData;
        private bool _canSelect = false;
        private Sequence _sequence;

        private Vector3 _parentOffset => transform.parent.position;
        public void Initialize(Level.Tile tileData, bool isLocked)
        {
            _tileData = tileData;
            remainingChildren = new List<int>(_tileData.children);

            _canSelect = !isLocked;
            characterText.text = _tileData.character;
            darkOverlay.enabled = isLocked;
        }

        public int GetId()
        {
            return _tileData.id;
        }

        public bool CanBeSelect()
        {
            return _canSelect;
        }

        public void ResetTile(bool isLocked)
        {
            remainingChildren = new List<int>(_tileData.children);
            MoveBack(isLocked);
        }

        public void UnlockTile()
        {
            _canSelect = true;
            darkOverlay.enabled = false;
        }

        public void LockTile()
        {
            _canSelect = false;
            darkOverlay.enabled = true;
        }
        public string GetCharacter()
        {
            return _tileData.character;
        }

        public void InvalidTap()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.Append(transform.DOShakePosition(.25f, .1f, 10, 90));
        }
        public void MoveToPosition(Vector3 position)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.AppendCallback(() => { _canSelect = false; });
            _sequence.Append(transform.DOMove(position, .35f));
            _sequence.Join(transform.DOScale(6f, .35f));
        }

        private void MoveBack(bool isLocked)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.AppendCallback(() =>
            {
                _canSelect = !isLocked;
                darkOverlay.enabled = isLocked;
            });
            _sequence.Append(transform.DOMove(_tileData.position + _parentOffset, .25f));
            _sequence.Join(transform.DOScale(8f, .25f));
        }
    }
}