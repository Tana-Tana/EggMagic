using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggManager : MonoBehaviour
{
    private Egg[,] _egg;
    private void OnEnable()
    {
        Messenger.AddListener<Egg[,]>(EventKey.SetEggMatrix, SetEggMatrix);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<Egg[,]>(EventKey.SetEggMatrix, SetEggMatrix);
    }

    private void SetEggMatrix(Egg[,] _egg)
    {
        this._egg = _egg;
    }
}
