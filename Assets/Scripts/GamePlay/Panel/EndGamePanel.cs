using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using Assets.Scripts.GamePlay.Object.Egg;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGamePanel : Panel
{
    [Header("BLOCK_INFORMATION", order = 0)]
    [SerializeField] private RectTransform title;
    [SerializeField] private RectTransform elementBottom;
    [SerializeField] private GameObject popup;
    [SerializeField] private Animator animatorPopup;

    [Header("STAR", order = 2)]
    [SerializeField] private Image starOne;
    [SerializeField] private Image starTwo;
    [SerializeField] private Image starThree;

    [Header("SCORE", order = 1)]
    [SerializeField] private TextMeshProUGUI yourScore;
    [SerializeField] private TextMeshProUGUI bestScore;

    [Header("EGG", order = 3)]
    [SerializeField] private Image egg;

    [Header("OTHER", order = 4)]
    [SerializeField] private GameObject glow;
    [SerializeField] private float glowZ = 0f;

    private bool _checkStartGlow = false;
    private float _starScore = 0f;
    private AnimatorStateInfo _animatorStateInfo;


    private async void Start()
    {
        _animatorStateInfo = animatorPopup.GetCurrentAnimatorStateInfo(0);
        //animatorPopup.enabled = false;
        _starScore = GamePlayController.Instance.BestScore / 3;
        await LoadPopUp();
        await LoadScore();
        await LoadEgg();
        await LoadStar();
        await LoadOption();
        _checkStartGlow = true;
        //glow.transform
        //    .DORotate(Vector3.forward * -360, 2f, RotateMode.FastBeyond360)
        //    .SetDelay(0)
        //    .SetLoops(-1, LoopType.Restart);
    }

    private void OnEnable()
    {
        Messenger.AddListener(EventKey.SHOW_POPUP_END_GAME, ShowPopupEndGame);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(EventKey.SHOW_POPUP_END_GAME, ShowPopupEndGame);
    }

    private async void ShowPopupEndGame()
    {
        animatorPopup.SetBool("isOpen", true);
        elementBottom.DOAnchorPosY(70, _animatorStateInfo.length).SetEase(Ease.OutExpo);
        await Task.Delay((int)(_animatorStateInfo.length * 1000));

        animatorPopup.enabled = false;
        animatorPopup.SetBool("isOpen", false);
        PanelManager.Instance.ClosePanel(GameConfig.ACHIEVEMENT_PANEL);
    }

    private async Task LoadPopUp()
    {
        popup.transform.DOScale(1, 1.5f).SetEase(Ease.OutCubic);
        await Task.Delay(1500);
    }
    private async Task LoadStar()
    {
        if(1 <= GamePlayController.Instance.Score && GamePlayController.Instance.Score <= GamePlayController.Instance.BestScore)
        {
            starOne.transform.DOMove(starOne.transform.parent.position, 1.5f).SetEase(Ease.OutBounce);
            await Task.Delay(1500);
        }
        
        if(_starScore <= GamePlayController.Instance.Score && GamePlayController.Instance.Score <= GamePlayController.Instance.BestScore)
        {
            starTwo.transform.DOMove(starTwo.transform.parent.position, 1.5f).SetEase(Ease.OutBounce);
            await Task.Delay(1500);
        }
        
        if(GamePlayController.Instance.Score == GamePlayController.Instance.BestScore)
        {
            starThree.transform.DOMove(starThree.transform.parent.position, 1.5f).SetEase(Ease.OutBounce);
            await Task.Delay(1500);
        }
        
    }

    private async Task LoadEgg()
    {
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + ((EggType)(GamePlayController.Instance.LevelMaxOfEgg)).ToString());
        //Debug.Log(eggInfor == null);
        egg.sprite = eggInfor.Icon;
        egg.SetNativeSize();
        egg.gameObject.transform.DOScale(1f, 1f).SetEase(Ease.OutQuad);
        await Task.Delay(1000);
    }

    private async Task LoadScore()
    {
        yourScore.text = GamePlayController.Instance.Score.ToString();
        yourScore.gameObject.transform.DOScale(1f, 1f).SetEase(Ease.OutQuad);
        bestScore.text = GamePlayController.Instance.BestScore.ToString();
        bestScore.gameObject.transform.DOScale(1f, 1f).SetEase(Ease.OutQuad);
        await Task.Delay(1000);
    }

    private async Task LoadOption()
    {
        title.DOAnchorPosY(-90,1f).SetEase(Ease.OutExpo);
        elementBottom.DOAnchorPosY(70,1f).SetEase(Ease.OutExpo);
        await Task.Delay(1000);
    }

    private void Update()
    {
        if (_checkStartGlow)
        {
            DoRotateGlow();
        }
    }

    private void DoRotateGlow()
    {
        Quaternion quaternion = Quaternion.Euler(0, 0, glowZ);
        glow.transform.rotation = quaternion;
        glowZ += 0.1f;
    }

    public async void ResetGame()
    {
        await GameManager.Instance.CloseScene();
        SceneManager.LoadScene(GameConfig.GAME_PLAY);
    }

    public async void BackHome()
    {
        await GameManager.Instance.CloseScene();
        SceneManager.LoadScene(GameConfig.HOME);
    }

    public void OpenAchievement()
    {
        animatorPopup.enabled = true;
        PanelManager.Instance.OpenPanel(GameConfig.ACHIEVEMENT_PANEL);
        elementBottom.DOAnchorPosY(-200, 0.5f).SetEase(Ease.OutExpo);
    }

}
