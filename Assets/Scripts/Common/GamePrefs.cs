using UnityEngine;

namespace Assets.Scripts.Common
{
    public static class GamePrefs
    {
        // path
        public const string HIGH_SCORE_KEY = "HighScore";
        public const string LEVEL_MAX_OF_EGG = "LevelEggMax";
        public const string MUSIC_KEY = "music";
        public const string SOUND_KEY = "sound";

        // music
        public static int GetMusic()
        {
            return PlayerPrefs.GetInt(MUSIC_KEY, GameConfig.MUSIC_START);
        }

        public static void SetMusic(int status)
        {
            PlayerPrefs.SetInt(MUSIC_KEY, status);
        }

        // sound
        public static int GetSound()
        {
            return PlayerPrefs.GetInt(SOUND_KEY, GameConfig.SOUND_START);
        }

        public static void SetSound(int status)
        {
            PlayerPrefs.SetInt(SOUND_KEY, status);
        }

        // High score
        public static int GetHighScore()
        {
            return PlayerPrefs.GetInt(HIGH_SCORE_KEY, GameConfig.HIGH_SCORE_START);
        }

        public static void SetHighScore(int newScore)
        {
            if (newScore > GetHighScore())
            {
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, newScore);
            }
        }

        // Level max of egg
        public static int GetLevelEggMax()
        {
            return PlayerPrefs.GetInt(LEVEL_MAX_OF_EGG, GameConfig.LEVEL_MAX_OF_EGG_START);
        }

        public static void SetLevelEggMax(int newLevel) 
        { 
            if(newLevel > GetLevelEggMax())
            {
                PlayerPrefs.SetInt(LEVEL_MAX_OF_EGG, newLevel);
            }
        }
    }
}
