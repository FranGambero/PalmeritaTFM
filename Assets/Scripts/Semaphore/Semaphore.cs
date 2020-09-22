using ElJardin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Semaphore : Singleton<Semaphore> {
    public List<ITurn> turnBasedElementList;
    public Action<int> onTurnStart;

    private void Awake() {
        turnBasedElementList = new List<ITurn>();

        turnBasedElementList.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<ITurn>());

        turnBasedElementList.Sort((E1, E2) => E1.turnIndex.CompareTo(E2.turnIndex));

        foreach (var item in turnBasedElementList) {
            onTurnStart += item.onTurnStart;
        }
    }

    private void onTurnEnd(int index) {
        int currentIndex;

        var currentITurn = turnBasedElementList.Find(e => e.turnIndex == index);
        currentIndex = turnBasedElementList.IndexOf(currentITurn);
        currentIndex = (currentIndex + 1) % turnBasedElementList.Count;

        onTurnStart?.Invoke(turnBasedElementList[currentIndex].turnIndex);      // :(
    }

}
