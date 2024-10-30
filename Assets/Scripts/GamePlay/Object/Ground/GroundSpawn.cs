using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundSpawn : MonoBehaviour
{
    [Header("-----------Prefabs Ground-----------")]
    [SerializeField] private Ground oddGroundPrefab;
    [SerializeField] private Ground evenGroundPrefab;

    [Header("----------------Board---------------")]
    [SerializeField] private int column;
    [SerializeField] private int row;

    private Ground[,] _ground;

    private void Start()
    {
        _ground = new Ground[row, column];
        GenerateGround();
    }

    private void GenerateGround()
    {
        for(int i = 0; i < row; ++i)
        {
            for(int j=0;j< column; ++j)
            {
                int sum = i + j;
                if(sum % 2 == 0)
                {
                    _ground[i, j] = Instantiate(oddGroundPrefab, transform); 
                }
                else
                {
                    _ground[i, j] = Instantiate(evenGroundPrefab, transform);
                }
            }
        }
    }
}
