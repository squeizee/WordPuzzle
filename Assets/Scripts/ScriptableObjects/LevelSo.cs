using System.Collections.Generic;
using Level;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelSo", menuName = "GGTask/LevelSo")]
    public class LevelSo : ScriptableSingleton<LevelSo>
    {
        public List<TextAsset> levelDataList;
        public TextAsset levelDictionary;
        public LevelData GetLevelData(int level)
        {
            return JsonConvert.DeserializeObject<LevelData>(levelDataList[level - 1].text);
        }

        public string GetCurrentLevelTitle()
        {
            return GetLevelTitle(PersistentDataManager.playingLevel - 1);
        }
        public List<string> GetLevelTitleList()
        {
            List<string> titleList = new List<string>();

            for (int i = 0; i < levelDataList.Count; i++)
            {
                titleList.Add(GetLevelTitle(i));
            }

            return titleList;
        }

        private string GetLevelTitle(int index)
        {
            return JsonConvert.DeserializeObject<LevelData>(levelDataList[index].text).title;
        }
    }
}
