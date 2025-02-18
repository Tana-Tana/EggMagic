using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

public class ShadowEgg : MonoBehaviour
{
    [Header("COMPONENT", order = 0)]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer tileSpriteRenderer;

    private void Start()
    {
        spriteRenderer.sortingLayerName = GameConfig.OBJECT_LAYER;
        spriteRenderer.sortingOrder = tileSpriteRenderer.sortingOrder + 1;
    }
}
