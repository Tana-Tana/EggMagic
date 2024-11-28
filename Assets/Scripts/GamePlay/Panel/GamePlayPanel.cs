using System;
using System.Collections;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using DG.Tweening;
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
    [SerializeField] private float fillTime;
    private bool _checkFillTime;

    [Header("NOTICE", order = 2)]
    [SerializeField] private GameObject overlayUI;
    [SerializeField] private GameObject noticeReset;
    [SerializeField] private GameObject noticeGoHome;

    [Header("COVER", order = 3)]
    [SerializeField] private RectTransform leftCover;
    [SerializeField] private RectTransform rightCover;

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
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(EventKey.RESET_TIME_AND_UPDATE_SCORE, ResetTimeAndUpdateScore);

    }

    private void ResetTimeAndUpdateScore()
    {
        scoreText.text = GamePlayController.Instance.Score.ToString();
        levelMaxText.text = GamePlayController.Instance.LevelMaxOfEgg.ToString();
        countTimeImage.fillAmount = 1;
    }

    private void Update()
    {
        if (_checkFillTime)
        {
            if (countTimeImage.fillAmount > 0 && GamePlayController.Instance.CheckContinue && !GamePlayController.Instance.CheckNoNextStep)
            {
                overlayUI.SetActive(false);
                //Debug.Log(countTimeImage.fillAmount);
                countTimeImage.fillAmount -= Time.deltaTime * fillTime;
            }
            else
            {
                if (!GamePlayController.Instance.CheckEndGame && GamePlayController.Instance.CheckContinue)
                {
                    GamePlayController.Instance.CheckEndGame = true;
                    GamePrefs.SetLevelEggMax(GamePlayController.Instance.LevelMaxOfEgg);
                    ShowEndGamePanel();
                }
            }
        }
    }

    private void ShowEndGamePanel()
    {
        if (GamePlayController.Instance.CheckNoNextStep)
        {
            noticeEndGame.text = "No more steps to move!!!";
        }
        else
        {
            noticeEndGame.text = "Time out!!!";
        }

        noticeEndGame.gameObject.SetActive(true);
        StartCoroutine(IEDelayTimeShowText());
    }

    private IEnumerator IEDelayTimeShowText()
    {
        yield return new WaitForSeconds(2);
        PanelManager.Instance.OpenPanel(GameConfig.END_GAME_PANEL);
    }

    public void ShowNoticeReset()
    {
        GamePlayController.Instance.CheckContinue = false;
        overlayUI.SetActive(true);
        noticeReset.SetActive(true);
    }

    public async void ResetGame()
    {
        await GameManager.Instance.CloseScene();
        SceneManager.LoadScene(GameConfig.GAME_PLAY);
    }


    public void HideNoticeReset()
    {
        GamePlayController.Instance.CheckContinue = true;
        noticeReset.SetActive(false);
    }

    public void ShowNoticeBackHome()
    {
        GamePlayController.Instance.CheckContinue = false;
        overlayUI.SetActive(true);
        noticeGoHome.SetActive(true);
    }

    public async void BackHome()
    {
        await GameManager.Instance.CloseScene();
        SceneManager.LoadScene(GameConfig.HOME);

    }


    public void HideNoticeBackHome()
    {
        GamePlayController.Instance.CheckContinue = true;
        noticeGoHome.SetActive(false);
    }

    public void Share()
    {
        // take a picture

        // share
    }

    public void Pause()
    {
        overlayUI.SetActive(true);
        GamePlayController.Instance.CheckContinue = false;
        PanelManager.Instance.OpenPanel(GameConfig.SETTING_PANEL);
    }

    public void ShowTutorial()
    {
        overlayUI.SetActive(true);
        GamePlayController.Instance.CheckContinue = false;
        PanelManager.Instance.OpenPanel(GameConfig.TUTORIAL_PANEL);
    }

}
