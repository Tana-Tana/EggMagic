using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [Header("----------Egg----------")]
    [SerializeField] private bool checkHaveEgg;
    [SerializeField] private int idEgg;
    [SerializeField] private float offsetWhenSelecting;

    [Header("----------Status Ground----------")]
    public bool isSelecting = false;

    private void OnEnable()
    {
        Messenger.AddListener(EventKey.UnSelectionGround, UnSelectionGround);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(EventKey.UnSelectionGround, UnSelectionGround);
    }

    private void UnSelectionGround()
    {
        if(isSelecting == true)
        {
            transform.position -= Vector3.up * offsetWhenSelecting;
            isSelecting = false;
        }
    }

    public void OnClick()
    {
        Messenger.Broadcast(EventKey.UnSelectionGround);
        if(isSelecting == false)
        {
            transform.position += Vector3.up * offsetWhenSelecting;
            isSelecting = true;
        }
    }

}
