﻿using ElJardin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Semaphore : Singleton<Semaphore> {
    public List<ITurn> turnBasedElementList;
    public Action<int> onTurnStart;
    public int currentTurn;

    private void Awake() {
        turnBasedElementList = new List<ITurn>();

        turnBasedElementList.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<ITurn>());

        turnBasedElementList.Sort((E1, E2) => E1.turnIndex.CompareTo(E2.turnIndex));

        foreach (var item in turnBasedElementList) {
            onTurnStart += item.onTurnStart;
        }
    }

    private void Start() {
        // Se cambiará por otra función que inicie el nivel
        if (turnBasedElementList.Count > 0)
            currentTurn = turnBasedElementList[0].turnIndex;
        else
            Debug.LogError("NO TIENES NÁ EN LA LISTA");
        onTurnStart?.Invoke(currentTurn);
    }

    public void onTurnEnd(int index) {
        Debug.Log("El turno actual " + currentTurn + " y el index " + index);
        if (currentTurn == index) {
            int currentIndex;

            var currentITurn = turnBasedElementList.Find(e => e.turnIndex == index);
            currentIndex = turnBasedElementList.IndexOf(currentITurn);
            currentIndex = (currentIndex + 1) % turnBasedElementList.Count;

            currentTurn = turnBasedElementList[currentIndex].turnIndex;

            onTurnStart?.Invoke(currentTurn);      // :(
        }
    }

    public int GetNewIndex() {
        return turnBasedElementList[turnBasedElementList.Count - 1].turnIndex + 1;
    }

    public void AddTurn(ITurn newTurnObject) {
        onTurnStart += newTurnObject.onTurnStart;
        if (turnBasedElementList.Find(n => n.turnIndex == newTurnObject.turnIndex) == null) {
            turnBasedElementList.Add(newTurnObject);
        }
    }

    public void RemoveTurn(int indexToRemove) {
        turnBasedElementList.ForEach((n) => {
            if (n.turnIndex == indexToRemove) {
                onTurnStart -= n.onTurnStart;
            }
        });
        turnBasedElementList.RemoveAll(n => n.turnIndex == indexToRemove);
    }

}
