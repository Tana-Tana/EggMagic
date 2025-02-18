using System;
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
    [SerializeField] private GameObject overlayAction;

    private async void Start()
    {
        GameManager.Instance.LeftCover = leftCover;
        GameManager.Instance.RightCover = rightCover;

        countTimeImage.fillAmount = 1;
        scoreText.text = GamePlayController.Instance.Score.ToString();
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
        Messenger.AddListener(EventKey.OPEN_OVERLAYUI, OpenOverlayUI);
        Messenger.AddListener(EventKey.CLOSE_OVERLAYUI, CloseOverlayUI);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(EventKey.RESET_TIME_AND_UPDATE_SCORE, ResetTimeAndUpdateScore);
        Messenger.RemoveListener<EggType>(EventKey.SET_IMAGE_EGG_CURRENT, SetImageEggCurrent);
        Messenger.RemoveListener(EventKey.SET_IMAGE_EGG_MAX, SetImageEggMax);
        Messenger.RemoveListener(EventKey.OPEN_OVERLAYUI, OpenOverlayUI);
        Messenger.RemoveListener(EventKey.CLOSE_OVERLAYUI, CloseOverlayUI);
    }

    private void CloseOverlayUI()
    {
        overlayAction.SetActive(false);
    }

    private void OpenOverlayUI()
    {
        overlayAction.SetActive(true);
    }



    private void SetImageEggMax()
    {
        int levelCurrent = GamePlayController.Instance.LevelMaxOfEgg;
        int levelMaxCurrent = GamePlayController.Instance.LevelMaxOfEgg < 14 ? GamePlayController.Instance.LevelMaxOfEgg + 1 : GamePlayController.Instance.LevelMaxOfEgg;
        // eggCurrent
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + (EggType)levelCurrent);
        eggCurrentSelect.sprite = eggInfor.ImageAchivement;
        SetSizeImageEgg(eggCurrentSelect);

        // eggNext
        eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + (EggType)levelMaxCurrent);
        eggNext.sprite = eggInfor.ImageAchivement;
        SetSizeImageEgg(eggNext);

    }

    private void SetImageEggCurrent(EggType eggType)
    {
        // eggCurrent
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + eggType);
        eggCurrentSelect.sprite = eggInfor.ImageAchivement;
        SetSizeImageEgg(eggCurrentSelect);

        // eggNext
        eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + (EggType)((int)eggType + 1));
        eggNext.sprite = eggInfor.ImageAchivement;
        SetSizeImageEgg(eggNext);

    }

    private void SetSizeImageEgg(Image egg)
    {
        egg.SetNativeSize();
        Vector2 size = egg.rectTransform.sizeDelta;
        size.x /= 1.2f;
        size.y /= 1.2f;
        egg.rectTransform.sizeDelta = size;
    }

    private void ResetTimeAndUpdateScore()
    {
        scoreText.text = GamePlayController.Instance.Score.ToString();
        scoreBestText.text = GamePlayController.Instance.BestScore.ToString();
        levelMaxText.text = GamePlayController.Instance.LevelMaxOfEgg.ToString();
        countTimeImage.fillAmount = 1;
    }

    private void Update()
    {
        if (GamePlayController.Instance.checkNoNextStep && _checkFillTime)
        {
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
                    countTimeImage.fillAmount -= Time.deltaTime * fillTime;
                }
                else
                {
                    if (!GamePlayController.Instance.checkEndGame && GamePlayController.Instance.checkContinue)
                    {
                        GamePlayController.Instance.checkEndGame = true;
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

        GamePrefs.SetLevelEggMax(GamePlayController.Instance.LevelMaxOfEgg);
        GamePrefs.SetHighScore(GamePlayController.Instance.BestScore);

        SoundController.Instance.PlayOneShotAudio(GameConfig.GAMEOVER_AUDIO);
        await Task.Delay((int)(SoundController.Instance.GetLengthOfClip(GameConfig.GAMEOVER_AUDIO) * 1000));
        PanelManager.Instance.OpenPanel(GameConfig.END_GAME_PANEL);
    }

    public void ShowNoticeReset()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.OPEN_POPUP_AUDIO);
        if (!animatorNotice.enabled)
        {
            GamePlayController.Instance.checkContinue = false;
            overlayUI.SetActive(true);
            noticeReset.SetActive(true);
        }
    }

    public async void ResetGame()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.TAP_AUDIO);
        await GameManager.Instance.CloseScene();
        SceneManager.LoadScene(GameConfig.GAME_PLAY);
    }


    public void HideNoticeReset()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.TAP_AUDIO);
        GamePlayController.Instance.checkContinue = true;
        noticeReset.SetActive(false);
    }

    public void ShowNoticeBackHome()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.OPEN_POPUP_AUDIO);
        if (!animatorNotice.enabled)
        {
            GamePlayController.Instance.checkContinue = false;
            overlayUI.SetActive(true);
            noticeGoHome.SetActive(true);
        }
    }

    public async void BackHome()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.TAP_AUDIO);
        await GameManager.Instance.CloseScene();
        SceneManager.LoadScene(GameConfig.HOME);

    }


    public void HideNoticeBackHome()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.TAP_AUDIO);
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
        if (!animatorNotice.enabled)
        {
            overlayUI.SetActive(true);
            GamePlayController.Instance.checkContinue = false;
            PanelManager.Instance.OpenPanel(GameConfig.SETTING_PANEL);
        }
    }

    public void ShowTutorial()
    {
        if (!animatorNotice.enabled)
        {
            overlayUI.SetActive(true);
            GamePlayController.Instance.checkContinue = false;
            PanelManager.Instance.OpenPanel(GameConfig.TUTORIAL_PANEL);
        }
    }

}
