using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("COVER", order = 0)]
    [SerializeField] private RectTransform leftCover;
    public RectTransform LeftCover { get { return leftCover; }  set { leftCover = value; } }
    [SerializeField] private RectTransform rightCover;
    public RectTransform RightCover { get { return rightCover; } set { rightCover = value; } }

    public async Task LoadScene()
    {
        leftCover.DOAnchorPosX(-1000, 0.5f).SetEase(Ease.OutCubic);
        rightCover.DOAnchorPosX(1000, 0.5f).SetEase(Ease.OutCubic);
        await Task.Delay(500);
    }

    public async Task CloseScene()
    {
        leftCover.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutCubic);
        rightCover.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutCubic);
        await Task.Delay(500);
    }

}
