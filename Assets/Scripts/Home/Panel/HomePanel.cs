using System;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomePanel : Panel
{
    [Header("COVER", order = 0)]
    [SerializeField] private RectTransform leftCover;
    [SerializeField] private RectTransform rightCover;

    [Header("Other", order = 1)]
    [SerializeField] private RectTransform title;
    [SerializeField] private RectTransform bottom;

    private async void Start()
    {
        GameManager.Instance.LeftCover = leftCover;
        GameManager.Instance.RightCover = rightCover;

        Application.targetFrameRate = GameConfig.FPS;
        await GameManager.Instance.LoadScene();

    }

    private void OnEnable()
    {
        Messenger.AddListener<float>(EventKey.HIDE_BOTTOM_AND_PUSH_UP_TITLE, hideElementHomePanel);
        Messenger.AddListener<float>(EventKey.SHOW_BOTTOM_AND_PUSH_UP_TITLE, showElementHomePanel);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<float>(EventKey.HIDE_BOTTOM_AND_PUSH_UP_TITLE, hideElementHomePanel);
        Messenger.RemoveListener<float>(EventKey.SHOW_BOTTOM_AND_PUSH_UP_TITLE, showElementHomePanel);
    }

    private void showElementHomePanel(float lengthClip)
    {
        title.DOAnchorPosY(-100, lengthClip).SetEase(Ease.OutCubic).SetRelative();
        bottom.DOAnchorPosY(300, lengthClip).SetEase(Ease.OutCubic).SetRelative();
    }

    private void hideElementHomePanel(float lengthClip)
    {
        title.DOAnchorPosY(100, lengthClip).SetEase(Ease.OutCubic).SetRelative();
        bottom.DOAnchorPosY(-300, lengthClip).SetEase(Ease.OutCubic).SetRelative();
    }

    public async void Play()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.OPEN_POPUP_AUDIO);
        await GameManager.Instance.CloseScene();
        SceneManager.LoadScene(GameConfig.GAME_PLAY);
    }

    public void OpenAchievement()
    {
        PanelManager.Instance.OpenPanel(GameConfig.ACHIEVEMENT_PANEL);
    }

    public void OpenSetting()
    {
        PanelManager.Instance.OpenPanel(GameConfig.SETTING_PANEL);
    }

    public void OpenTutorial()
    {
        PanelManager.Instance.OpenPanel(GameConfig.TUTORIAL_PANEL);
    }

}
