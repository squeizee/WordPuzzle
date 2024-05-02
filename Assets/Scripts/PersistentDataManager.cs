using System.Collections.Generic;
using Level;
using Newtonsoft.Json;
using UnityEngine;

public static class PersistentDataManager
{
    private static List<PersistentLevelData> _persistentLevelDataList;
    
    private const string Levels = "Levels";

    public static int playingLevel = 1;
    public static void SetReachedLevel(int level)
    {
        PlayerPrefs.SetInt("ReachedLevel", level);
    }
    public static int GetReachedLevel()
    {
        return PlayerPrefs.GetInt("ReachedLevel", 1);
    }
    public static void NextLevel()
    {
        var currentLevel = GetReachedLevel();
        currentLevel++;
        SetReachedLevel(currentLevel);
    }
    public static void SaveLevel(int level,int highScore)
    {
        
        if (_persistentLevelDataList == null)
        {
            GetLevels();
            _persistentLevelDataList ??= new List<PersistentLevelData>();
        }
        
        var levelData = _persistentLevelDataList.Find(x => x.level == level);

        if (levelData != null)
        {
            levelData.highScore = highScore;
        }
        else
        {
            _persistentLevelDataList.Add(new PersistentLevelData()
            {
                level = level,
                highScore = highScore,
            });
        }
        
        var json = JsonConvert.SerializeObject(_persistentLevelDataList);
        PlayerPrefs.SetString(Levels,json);
    }

    public static int GetHighScore(int level = -1)
    {
        if (level == -1)
            level = playingLevel;
        
        if (_persistentLevelDataList == null)
        {
            GetLevels();
            _persistentLevelDataList ??= new List<PersistentLevelData>();
        }
        
        if (_persistentLevelDataList == null)
        {
            SaveLevel(level,0);
        }
        
        var levelData = _persistentLevelDataList.Find(x => x.level == level);

        if (levelData == null)
        {
            SaveLevel(level, 0);
            return 0;
        }
        
        return levelData.highScore;
    }
    private static void GetLevels()
    {
        _persistentLevelDataList = JsonConvert.DeserializeObject<List<PersistentLevelData>>(PlayerPrefs.GetString(Levels));
    }
}