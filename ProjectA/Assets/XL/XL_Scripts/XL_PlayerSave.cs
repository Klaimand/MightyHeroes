using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_PlayerSave : MonoBehaviour
{
    public int[,] characterLevel = new int[2, 6]; //0 = BLAST ; 1 = SIMO ; 2 = SAYURI ; 3 = SLOWMAN ; 4 = DRAINTANK ; 5 = CLONE

    private void Awake()
    {
        for (int i = 0; i < characterLevel.GetLength(0); i++)
        {
            for (int j = 0; j < characterLevel.GetLength(1); j++)
            {
                characterLevel[i, j] = 0;
            }
        }
    }
}
