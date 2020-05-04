using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionBoxTest : MonoBehaviour
{

    public bool isVisible = false;

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;

    }
}
