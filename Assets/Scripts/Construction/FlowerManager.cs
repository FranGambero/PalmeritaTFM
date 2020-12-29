using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour
{
    public List<Flower> flores;

    public bool CountFlowerStatus() {
        Debug.LogWarning("TIENES " + flores.Count + " flores " + flores.TrueForAll(f => f.flowerOpened));
        return flores.TrueForAll(f => f.flowerOpened);
    }
}
