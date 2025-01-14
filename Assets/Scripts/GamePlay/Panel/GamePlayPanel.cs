using System.Threading.Tasks;
using Assets.Scripts.Common;
using Assets.Scripts.GamePlay.Object.Egg;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayPanel : Panel
{
    [Header("SCORE", order = 0)]
    [SerializeField] private TextMeshProUGUI levelMaxText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreBestText;

    [Header("Time", order = 1)]
    [SerializeField] private Image countTimeImage;
    [SerializeField] private TextMeshProUGUI noticeEndGame;
    [SerializeField] private Animator animatorNotice;
    [SerializeField] private float fillTime;
    private bool _checkFillTime;

    [Header("NOTICE", order = 2)]
    [SerializeField] private GameObject overlayUI;
    [SerializeField] private GameObject noticeReset;
    [SerializeField] private GameObject noticeGoHome;

    [Header("COVER", order = 3)]
    [SerializeField] private RectTransform leftCover;
    [SerializeField] private RectTransform rightCover;

    [Header("OTHER", order = 4)]
    [SerializeField] private Image eggCurrentSelect;
    [SerializeField] private Image eggNext;

    private async void Start()
    {
        GameManager.Instance.LeftCover = leftCover;
        GameManager.Instance.RightCover = rightCover;

        countTimeImage.fillAmount = 1;
        scoreText.text = 0.ToString();
        levelMaxText.text = GamePlayController.Instance.LevelMaxOfEgg.ToString();
        scoreBestText.text = GamePrefs.GetHighScore().ToString();
        await GameManager.Instance.LoadScene();
        _checkFillTime = true;
    }

    private void OnEnable()
    {
        Messenger.AddListener(EventKey.RESET_TIME_AND_UPDATE_SCORE, ResetTimeAndUpdateScore);
        Messenger.AddListener<EggType>(EventKey.SET_IMAGE_EGG_CURRENT, SetImageEggCurrent);
        Messenger.AddListener(EventKey.SET_IMAGE_EGG_MAX, SetImageEggMax);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(EventKey.RESET_TIME_AND_UPDATE_SCORE, ResetTimeAndUpdateScore);
        Messenger.RemoveListener<EggType>(EventKey.SET_IMAGE_EGG_CURRENT, SetImageEggCurrent);
        Messenger.RemoveListener(EventKey.SET_IMAGE_EGG_MAX, SetImageEggMax);
    }

    private void SetImageEggMax()
    {
        if(GamePlayController.Instance.LevelMaxOfEgg < 14)
        {
            // eggCurrent
            EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + "Level" + GamePlayController.Instance.LevelMaxOfEgg);
            eggCurrentSelect.sprite = eggInfor.ImageGamePlay;
            eggCurrentSelect.SetNativeSize();

            // eggNext
            eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + "Level" + (GamePlayController.Instance.LevelMaxOfEgg + 1));
            eggNext.sprite = eggInfor.ImageGamePlay;
            eggNext.SetNativeSize();
        }
        else
        {
            EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + "Level" + GamePlayController.Instance.LevelMaxOfEgg);
            eggCurrentSelect.sprite = eggInfor.ImageGamePlay;
            eggCurrentSelect.SetNativeSize();

            eggNext.sprite = eggInfor.ImageGamePlay;
            eggNext.SetNativeSize();
        }
    }

    private void SetImageEggCurrent(EggType eggType)
    {
        // eggCurrent
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + eggType);
        eggCurrentSelect.sprite = eggInfor.ImageGamePlay;
        eggCurrentSelect.SetNativeSize();

        // eggNext
        eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + (EggType)((int)eggType + 1));
        eggNext.sprite = eggInfor.ImageGamePlay;
        eggNext.SetNativeSize();

    }

    private void ResetTimeAndUpdateScore()
    {
        scoreText.text = GamePlayController.Instance.Score.ToString();
        levelMaxText.text = GamePlayController.Instance.LevelMaxOfEgg.ToString();
        countTimeImage.fillAmount = 1;
    }

    private void Update()
    {
        if (GamePlayController.Instance.checkNoNextStep && _checkFillTime)
        {
            GamePrefs.SetLevelEggMax(GamePlayController.Instance.LevelMaxOfEgg);
            ShowEndGamePanel();
            _checkFillTime = false;
        }
        else
        {
            if (_checkFillTime)
            {
                if (countTimeImage.fillAmount > 0 && GamePlayController.Instance.checkContinue)
                {
                    overlayUI.SetActive(false);
                    //countTimeImage.fillAmount -= Time.deltaTime * fillTime;
                }
                else
                {
                    if (!GamePlayController.Instance.checkEndGame && GamePlayController.Instance.checkContinue)
                    {
                        GamePlayController.Instance.checkEndGame = true;
                        GamePrefs.SetLevelEggMax(GamePlayController.Instance.LevelMaxOfEgg);
                        ShowEndGamePanel();
                    }
                }
            }
        }
    }

    private async void ShowEndGamePanel()
    {
        animatorNotice.enabled = true;
        if (GamePlayController.Instance.checkNoNextStep)
        {
            noticeEndGame.text = "No more steps to move!!!";
        }
        else
        {
            noticeEndGame.text = "Time out!!!";
        }

        await Task.Delay(2000); // sau thay bằng thời gian nhạc
        PanelManager.Instance.OpenPanel(GameConfig.END_GAME_PANEL);
    }

    public void ShowNoticeReset()
    {
        if(!animatorNotice.enabled)
        {
            GamePlayController.Instance.checkContinue = false;
            overlayUI.SetActive(true);
            noticeReset.SetActive(true);
        }
    }

    public async void ResetGame()
    {
        await GameManager.Instance.CloseScene();
        SceneManager.LoadScene(GameConfig.GAME_PLAY);
    }


    public void HideNoticeReset()
    {
        GamePlayController.Instance.checkContinue = true;
        noticeReset.SetActive(false);
    }

    public void ShowNoticeBackHome()
    {
        if(!animatorNotice.enabled)
        {
            GamePlayController.Instance.checkContinue = false;
            overlayUI.SetActive(true);
            noticeGoHome.SetActive(true);
        }
    }

    public async void BackHome()
    {
        await GameManager.Instance.CloseScene();
        SceneManager.LoadScene(GameConfig.HOME);

    }


    public void HideNoticeBackHome()
    {
        GamePlayController.Instance.checkContinue = true;
        noticeGoHome.SetActive(false);
    }

    public void Share()
    {
        // take a picture

        // share
    }

    public void Pause()
    {
        if(!animatorNotice.enabled)
        {
            overlayUI.SetActive(true);
            GamePlayController.Instance.checkContinue = false;
            PanelManager.Instance.OpenPanel(GameConfig.SETTING_PANEL);
        }
    }

    public void ShowTutorial()
    {
        if(!animatorNotice.enabled)
        {
            overlayUI.SetActive(true);
            GamePlayController.Instance.checkContinue = false;
            PanelManager.Instance.OpenPanel(GameConfig.TUTORIAL_PANEL);
        }
    }

}
