using System;
using Commands;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
     public MainScreenUI mainScreen;
     public LevelSelectionScreenUI levelSelectionScreen;
     public GameScreenUI gameScreen;

    private void OnEnable()
    {
        mainScreen.OnLevelButtonClicked += OnLevelSelection;
        levelSelectionScreen.OnLevelSelect += OnLevelSelected;
        gameScreen.OnContinueClicked += OnLevelSelection;
    }

    private void OnDisable()
    {
        mainScreen.OnLevelButtonClicked -= OnLevelSelection;
        levelSelectionScreen.OnLevelSelect -= OnLevelSelected;
        gameScreen.OnContinueClicked -= OnLevelSelection;
    }

    private void Start()
    {
        mainScreen.ShowScreen();
        levelSelectionScreen.HideScreen();
        gameScreen.HideScreen();
    }

    public void ShowLevelEnd(bool isHighScore, int score)
    {
        gameScreen.ShowLevelEnd(isHighScore);
        gameScreen.UpdateScore(score);
    }
    
    private void OnLevelSelection()
    {
        levelSelectionScreen.Initialize();
        
        mainScreen.HideScreen();
        levelSelectionScreen.ShowScreen();
    }
    private void OnLevelSelected(int level)
    {
        levelSelectionScreen.HideScreen();
        gameScreen.ShowScreen();
    }
}