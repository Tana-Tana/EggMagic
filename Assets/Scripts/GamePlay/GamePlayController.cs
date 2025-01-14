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

    //public check
    public bool checkContinue = true;
    public bool checkEndGame = false;
    public bool checkNoNextStep = false;
    public bool checkMerging = false;
    public bool checkBFS = false;

    private void Start()
    {
        Application.targetFrameRate = GameConfig.FPS;
        _bestScore = GamePrefs.GetHighScore();
    }

}
