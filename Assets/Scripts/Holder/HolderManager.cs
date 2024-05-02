using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Tiles;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace Holder
{
    public class HolderManager : MonoBehaviour
    {
        public Action<bool> OnWordChanged;
        public Action<bool> OnHolderEmpty;

        [SerializeField] private List<HolderSlot> holderSlots;

        private List<string> _submittedWords = new();

        private Sequence _sequence;
        public void Initialize()
        {
            holderSlots = GetComponentsInChildren<HolderSlot>(true).ToList();
            holderSlots.ForEach(holderSlot =>
            {
                var tile = holderSlot.GetTile();
                if (tile != null)
                {
                    Destroy(tile.gameObject);
                }
                holderSlot.RemoveTile();
                holderSlot.gameObject.SetActive(true);
            });
        }

        public void AddTile(TileController tile)
        {
            foreach (var holderSlot in holderSlots.Where(holderSlot => holderSlot.CanAddTile()))
            {
                holderSlot.AddTile(tile);
                var targetPosition = holderSlot.transform.position;
                targetPosition.z = tile.transform.position.z;
                tile.MoveToPosition(targetPosition);
                CheckWordOnHolder();
                OnHolderEmpty?.Invoke(IsHolderEmpty());
                break;
            }
        }
        

        public bool CanAddTile()
        {
            return holderSlots.Any(holderSlot => holderSlot.CanAddTile());
        }

        public bool IsHolderEmpty()
        {
            return holderSlots.Any(holderSlot => holderSlot.IsFull());
        }
        public List<string> GetCharactersOnHolder()
        {
            var characters = new List<string>();
            foreach (var holderSlot in holderSlots)
            {
                if (holderSlot.IsFull())
                {
                    characters.Add(holderSlot.GetTile().GetCharacter());
                }
            }

            return characters;
        }
        private string GetWordOnHolder()
        {
            var word = string.Empty;
            foreach (var holderSlot in holderSlots)
            {
                if (holderSlot.IsFull())
                {
                    word += holderSlot.GetTile().GetCharacter();
                }
            }

            return word;
        }

        private void CheckWordOnHolder()
        {
            var word = GetWordOnHolder();

            var isValidWord = WordChecker.IsValidWord(word);
            OnWordChanged?.Invoke(isValidWord);
        }

        public void Undo(out TileController tileController)
        {
            tileController = null;
            
            for (int i = 1; i <= holderSlots.Count; i++)
            {
                if(holderSlots[^i].IsFull())
                {
                    tileController = holderSlots[^i].GetTile();
                    holderSlots[^i].RemoveTile();
                    break;
                }
            }

            OnHolderEmpty?.Invoke(IsHolderEmpty());
            CheckWordOnHolder();
        }
        
        public void Submit(out string word, out int score)
        {
            word = GetWordOnHolder();
            score = WordChecker.GetWordScore(word);
            _submittedWords.Add(word);
            
            SubmitAnimation();
            
            OnWordChanged?.Invoke(false);
            OnHolderEmpty?.Invoke(false);
        }
        public List<string> GetSubmittedWords()
        {
            return _submittedWords;
        }
        private void SubmitAnimation()
        {
            _sequence?.Complete();
            _sequence = DOTween.Sequence();
            
            foreach (var holderSlot in holderSlots)
            {
                if (holderSlot.IsFull())
                {
                    var tile = holderSlot.GetTile();
                    _sequence.AppendCallback(() => holderSlot.RemoveTile());
                    _sequence.Join(tile.transform.DOScale(tile.transform.localScale * 1.1f, 0.1f));
                    _sequence.Append(tile.transform.DOScale(Vector3.zero, .05f));
                    _sequence.AppendCallback(() => Destroy(tile.gameObject));
                }
            }
        }
    }
}