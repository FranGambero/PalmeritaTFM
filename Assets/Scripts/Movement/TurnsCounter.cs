using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsCounter : Singleton<TurnsCounter>
{
    public int currMovements;
    public int maxMovements;

    public void OnCardUsed() {
        currMovements++;
    }

    public bool CheckResults() {
        return currMovements <= maxMovements;
    }
}
