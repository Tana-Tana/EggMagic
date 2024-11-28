using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common
{
    public static class GameConfig
    {
        //fps
        public const int FPS = 60;

        // path
        public const string EGG_INFOR_PATH = "EggsLevel/";
        public const string PANEL_PATH = "Panel/";

        // panel
        public const string GAME_PLAY_PANEL = "GamePlayPanel";
        public const string ACHIEVEMENT_PANEL = "AchievementPanel";
        public const string END_GAME_PANEL = "EndGamePanel";
        public const string HOME_PANEL = "HomePanel";
        public const string SETTING_PANEL = "SettingPanel";
        public const string TUTORIAL_PANEL = "TutorialPanel";


        // score
        public const int HIGH_SCORE_START = 0;

        // achievement
        public const int LEVEL_MAX_OF_EGG_START = 3;

        //scene
        public const int HOME = 0;
        public const int GAME_PLAY = 1;

        // link
        public const string FACEBOOK_LINK = "https://www.facebook.com/tan.phanthanh.731";
        public const string GITHUB_LINK = "https://github.com/TanaKeKe";
        public const string GROUP_LINK = "https://www.facebook.com/clubproptit";
    }
}
