using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteReceiveShadow : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponent<Renderer>().receiveShadows = true;
    }

    
}
