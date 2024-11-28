using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public static class GamePrefs
    {
        // path
        public const string HIGH_SCORE_KEY = "HighScore";
        public const string LEVEL_MAX_OF_EGG = "LevelEggMax";

        // High score
        public static int GetHighScore()
        {
            return PlayerPrefs.GetInt(HIGH_SCORE_KEY, GameConfig.HIGH_SCORE_START);
        }

        public static void SetHighScore(int newScore)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, newScore);
        }

        // Level max of egg
        public static int GetLevelEggMax()
        {
            return PlayerPrefs.GetInt(LEVEL_MAX_OF_EGG, GameConfig.LEVEL_MAX_OF_EGG_START);
        }

        public static void SetLevelEggMax(int newLevel) 
        { 
            PlayerPrefs.SetInt(LEVEL_MAX_OF_EGG, newLevel);
        }
    }
}
