using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameobjectCounter : MonoBehaviour
{
    
    [ContextMenu("Count Objects")]
    private void CountObjects()
    {
        int total = 0;
        var gameObjects = gameObject.GetComponentsInChildren<Transform>();

        print($"{gameObject.name} total childs: {gameObjects.Length}");
    }

}
