using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class GameManager
{
    private static GameManager instance;
    public static GameManager i
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
                instance.Prints = new List<Sprite>
                {
                    GameAssets.i.LeftFootPrint,
                    GameAssets.i.RightFootPrint,
                    GameAssets.i.HandPrint
                };
            }

            return instance;
        }
    }

    public List<Sprite> Prints;

}
