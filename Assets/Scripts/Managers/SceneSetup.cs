using UnityEngine;
using Unity.Cinemachine;

public class SceneSetup : MonoBehaviour
{
    private CinemachineInputAxisController _cinemachineController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        //GameManager.ChangeState(GameManager.i.State);
        Debug.Log(GameManager.i.State);
        //Trigger all the functions of the current state of the game manager on the startup of new scene
        GameManager.RefreshSceneReferences();
    }
}
