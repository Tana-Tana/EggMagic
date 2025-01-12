using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Component", order = 0)]
    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } set { spriteRenderer = value; } }

    [Header("Children", order = 1)]
    [SerializeField] private Egg egg;
    public Egg Egg { get { return egg; } set { egg = value; } }


    //public
    public bool clicked = false;
    public int row = 0;
    public int col = 0;

    private void OnMouseDown()
    {
        if(clicked)
        {
            Debug.Log("Merge");
        }
        else
        {
            if (GamePlayController.Instance.checkMerging)
            {
                Debug.Log("Reset bảng và hạ ô");
                GamePlayController.Instance.checkMerging = false;
            }
            else
            {
                Debug.Log("BFS");
                Messenger.Broadcast(EventKey.BFS, row, col);
            }
        }
    }
}
