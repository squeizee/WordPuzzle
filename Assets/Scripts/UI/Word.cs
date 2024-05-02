using TMPro;
using UnityEngine;

namespace UI
{
    public class Word : MonoBehaviour
    {
        [SerializeField] private TMP_Text wordText;
        
        public void SetWord(string word)
        {
            wordText.text = word;
        }
    }
}