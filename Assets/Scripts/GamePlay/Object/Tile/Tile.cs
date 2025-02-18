using System;
using Assets.Scripts.Common;
using Assets.Scripts.GamePlay.Object.Egg;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Component", order = 0)]
    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } set { spriteRenderer = value; } }

    [Header("Children", order = 1)]
    [SerializeField] private Egg egg;
    public Egg Egg { get { return egg; } set { egg = value; } }
    [SerializeField] private ShadowEgg shadowEgg;
    public ShadowEgg ShadowEgg { get => shadowEgg; set => shadowEgg = value; }
    private int row = 0; public int Row { get => row; set => row = value; }

    private int col = 0; public int Col { get => col; set => col = value; }

    public bool clicked = false;

    public Tile(int row, int col)
    {
        this.Row = row;
        this.Col = col;
    }

    private void OnMouseDown()
    {
        if (UIHelper.IsPointerOverUIObject()) return;
        if(clicked)
        {
            Debug.Log("Merge Egg");
            Messenger.Broadcast(EventKey.SET_EGG_FREE, row, col);
            Messenger.Broadcast(EventKey.EGG_MOVING, row, col);
        }
        else
        {
            SoundController.Instance.PlayOneShotAudio(GameConfig.SELECT_EGG_TO_MERGE_AUDIO);
            if (GamePlayController.Instance.checkMerging)
            {
                Debug.Log("Reset bảng và hạ ô");
                Messenger.Broadcast(EventKey.RESET_ALL_TILES);
            }
            else
            {
                Debug.Log("BFS và nâng ô");
                Messenger.Broadcast(EventKey.BFS, Row, Col);
            }
        }
    }
}
