using System;
using System.Collections.Generic;
using Commands;
using DG.Tweening;
using Holder;
using Tiles;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

public class GameManager : MonoBehaviour
{
    
    public enum GameState
    {
        LevelSelection,
        LevelStart,
        LevelEnd
    }
    
    [SerializeField] private InputManager inputManager;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private HolderManager holderManager;
    [SerializeField] private UIManager uiManager;

    private GameState _gameState;
    public void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        inputManager.UserButtonDown -= OnUserButtonDown;
        uiManager.levelSelectionScreen.OnLevelSelect -= OnLevelSelected;
        uiManager.gameScreen.OnSubmitClicked -= Submit;
        uiManager.gameScreen.OnUndoClicked -= Undo;
        uiManager.gameScreen.OnContinueClicked -= OnLevelSelection;
        holderManager.OnWordChanged -= uiManager.gameScreen.UpdateSubmitButtonState;
        holderManager.OnHolderEmpty -= uiManager.gameScreen.UpdateUndoButtonState;
    }

    private void SubscribeEvents()
    {
        inputManager.UserButtonDown += OnUserButtonDown;
        uiManager.levelSelectionScreen.OnLevelSelect += OnLevelSelected;
        uiManager.gameScreen.OnSubmitClicked += Submit;
        uiManager.gameScreen.OnUndoClicked += Undo;
        uiManager.gameScreen.OnContinueClicked += OnLevelSelection;
        holderManager.OnWordChanged += uiManager.gameScreen.UpdateSubmitButtonState;
        holderManager.OnHolderEmpty += uiManager.gameScreen.UpdateUndoButtonState;
    }

    private void OnUserButtonDown(Vector3 clickPosition)
    {
        if (inputManager.GetGameObjectUnderMouse(LayerMask.GetMask("Tile"), out var tileGameObject, out var hit))
        {
            var tileController = tileGameObject.GetComponent<TileController>();
            if (tileController && tileController.CanBeSelect())
            {
                new HandleTileClickedCommand().Execute(tileController, tileManager, holderManager);
            }
            else
            {
                tileController.InvalidTap();
            }
        }
    }

    private void OnLevelSelected(int level)
    {
        new StartLevelCommand().Execute(level, tileManager, holderManager);
        _gameState = GameState.LevelStart;
    }

    private void OnLevelSelection()
    {
        _gameState = GameState.LevelSelection;
    }
    private void OnLevelEnd()
    {
        if(_gameState == GameState.LevelEnd) 
            return;
        
        _gameState = GameState.LevelEnd;

        List<string> submittedWords = holderManager.GetSubmittedWords();
        int remainingCharacterCount = holderManager.GetCharactersOnHolder().Count + tileManager.GetCharactersOnTiles().Count;
        
        int totalScore = WordChecker.GetTotalScore(submittedWords, remainingCharacterCount);
        var highScore = PersistentDataManager.GetHighScore();
        
        var score = totalScore > highScore ? totalScore : highScore;
        PersistentDataManager.SaveLevel(PersistentDataManager.GetReachedLevel(),score);
        PersistentDataManager.NextLevel();
        
        uiManager.ShowLevelEnd(totalScore>highScore,totalScore);
    }
    private void Submit()
    {
        new SubmitCommand().Execute(tileManager, holderManager, uiManager);
        CheckAllTiles();
    }

    private void Undo()
    {
        new UndoCommand().Execute(tileManager, holderManager);
        CheckValidWord();
    }

    private void CheckAllTiles()
    {
        if (tileManager.IsAllTilesPlaced() && holderManager.IsHolderEmpty())
        {
            OnLevelEnd();
        }
        
        DOVirtual.DelayedCall(3f,CheckValidWord);
    }

    private void CheckValidWord()
    {
        var characters = holderManager.GetCharactersOnHolder();
        characters.AddRange(tileManager.GetCharactersOnTiles());
        
        bool hasValidWord = WordChecker.HasValidWordWithCharacters(characters);
        
        if(hasValidWord == false)
        {
            OnLevelEnd();
        }
    }
}