using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingUVs : MonoBehaviour
{
    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public Renderer renderer;
    public string textureName = "_MainTex";
    public bool isDeltaTime = false;

    Vector2 uvOffset = Vector2.zero;

    void LateUpdate()
    {
        if (!isDeltaTime)
        {
            uvOffset += (uvAnimationRate * Time.unscaledDeltaTime);
        }
        else
        {
            uvOffset += (uvAnimationRate * Time.deltaTime);
        }
        if (renderer.enabled)
        {
            renderer.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
        }
    }
}
