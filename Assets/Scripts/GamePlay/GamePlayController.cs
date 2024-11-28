using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

public class GamePlayController : Singleton<GamePlayController>
{
    private int _score = 0;
    public int Score { get { return _score; } set { _score = value; } }

    private int _levelMaxOfEgg = 3;
    public int LevelMaxOfEgg { get { return _levelMaxOfEgg; } set { _levelMaxOfEgg = value; } }

    private int _bestScore;
    public int BestScore { get { return _bestScore; } set { _bestScore = value; } }

    private bool _checkContinue = true;
    public bool CheckContinue { get { return _checkContinue; } set { _checkContinue = value; } }

    private bool _checkEndGame = false;
    public bool CheckEndGame { get { return _checkEndGame; } set {_checkEndGame = value; } }

    public bool _checkNoNextStep = false;
    public bool CheckNoNextStep { get { return _checkNoNextStep; } set { _checkContinue = value; } }

    private void Start()
    {
        Application.targetFrameRate = GameConfig.FPS;
        _bestScore = GamePrefs.GetHighScore();
        PanelManager.Instance.OpenPanel(GameConfig.GAME_PLAY_PANEL);
    }

    private void Update()
    {
        if(_score >= _bestScore)
        {
            _bestScore = _score;
            GamePrefs.SetHighScore(_bestScore);
        }
    }

}
