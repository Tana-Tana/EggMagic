using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private Animator effect;
    [SerializeField] private Egg egg;

    private void OnEnable()
    {
        Messenger.AddListener<int, int ,int >(EventKey.EFFECT_EGG_UP, SetEffect);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<int, int ,int >(EventKey.EFFECT_EGG_UP, SetEffect);

    }

    private void SetEffect(int level, int row, int col)
    {
        if (egg.Row == row && egg.Col == col)
        {
            if (level < 6)
            {
                effect.SetTrigger(GameConfig.SMOKE_EFFECT);
            }
            else if (level < 8)
            {
                effect.SetTrigger(GameConfig.BLINK_EFFECT);
            }
            else if (level < 10)
            {
                Debug.Log("Effect ánh lửa tỏa xung quanh");
            }
            else
            {
                Debug.Log("Thêm hiệu ứng thì làm thêm");
            }
        }
    }
}
