
namespace Assets.Scripts.Common
{
    public static class GameConfig
    {
        //fps
        public const int FPS = 60;

        // path
        public const string EGG_INFOR_PATH = "EggsLevel/";
        public const string PANEL_PATH = "Panel/";
        public const string AUDIO_PATH = "AudioClip/";


        // panel
        public const string GAME_PLAY_PANEL = "GamePlayPanel";
        public const string ACHIEVEMENT_PANEL = "AchievementPanel";
        public const string END_GAME_PANEL = "EndGamePanel";
        public const string HOME_PANEL = "HomePanel";
        public const string SETTING_PANEL = "SettingPanel";
        public const string TUTORIAL_PANEL = "TutorialPanel";

        // sorting Layer
        public const string OBJECT_LAYER = "Object";

        // score
        public const int HIGH_SCORE_START = 0;

        // achievement
        public const int LEVEL_MAX_OF_EGG_START = 3;

        //scene
        public const int HOME = 0;
        public const int GAME_PLAY = 1;

        // link
        public const string FACEBOOK_LINK = "https://www.facebook.com/tana.tana.1403";
        public const string GITHUB_LINK = "https://github.com/Tana-Tana";
        public const string GROUP_LINK = "https://www.facebook.com/clubproptit";

        #region Audio
        // audio
        public const string BACKGROUND_AUDIO = "Crimson Loftwing  The Legend of Zelda Skyward Sword";
        public const string ADD_SCORE_LEVEL1_5_AUDIO = "AddScore";
        public const string ADD_SCORE_LEVEL6_7_AUDIO = "Blink";
        public const string ADD_SCORE_LEVEL8_9_AUDIO = "EggBreak";
        public const string ADD_SCORE_LEVEL10_12_AUDIO = "Whistle";
        public const string ADD_SCORE_LEVEL_13_14_AUDIO = "SuperWhistle";
        public const string GAMEOVER_AUDIO = "GameOver";
        public const string SELECT_EGG_TO_MERGE_AUDIO = "Select";
        public const string TAP_AUDIO = "Tap";
        public const string START_GAME_AUDIO = "Notification";
        public const string OPEN_POPUP_AUDIO = "Pop";

        // sound and music
        public const int MUSIC_START = 1;
        public const int SOUND_START = 1;
        #endregion

        #region Animator

        // Effect
        public const string SMOKE_EFFECT = "Smoke";
        public const string BLINK_EFFECT = "Blink";

        // Egg
        public const string JUMP = "Jump";
        public const string FLY = "Fly";
        #endregion

        
    }
}
