using UnityEngine;

public class CameraDitheringRenderer : MonoBehaviour
{
    public Material _ditherMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture main = RenderTexture.GetTemporary(820, 470);

        Graphics.Blit(source, main, _ditherMaterial);
        Graphics.Blit(main, destination);

        RenderTexture.ReleaseTemporary(main);
    }
}
