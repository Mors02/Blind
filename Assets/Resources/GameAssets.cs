using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/***
 *  GameAssets contains assets needed to be changed on runtime.
 *  It is a component of an object in the scene, and the object MUST be a prefab.
    Also, the prefab MUST be saved in the following path: Assets/Resources, otherwise it won't work.
 */
public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) { _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));}
            return _i;
        }
    }

    [Header("Prints")]
    public Sprite HandPrint;
    public Sprite LeftFootPrint;
    public Sprite RightFootPrint;

    [Header("Materials")]
    public Material HandMaterial;
    public Material LeftFootMaterial;
    public Material RightFootMaterial;
}
