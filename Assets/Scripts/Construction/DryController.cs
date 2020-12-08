using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryController : MonoBehaviour, ITurn {
    public int numTurns = 3;
    private bool secadoDelTo;
    public int turnIndex { get { return _turnIndex; } set { _turnIndex = value; } }

    public int _turnIndex;

    public void onTurnStart(int currentIndex) {
        if (turnIndex == currentIndex) {
            checkDry();
        }
    }

    public void initDry(int newIndex) {
        //List<ITurn> listaTurnosActuales = Semaphore.Instance.turnBasedElementList;
        //turnIndex = listaTurnosActuales[listaTurnosActuales.Count].turnIndex + 1;
        secadoDelTo = false;
        turnIndex = newIndex;
    }

    private void checkDry() {
        if(numTurns > 0) {
            numTurns--;
            Debug.Log("Me quedan " + numTurns + " turnos");
        }  else {
            //Lo destruye
            secadoDelTo = true;
        }

        onTurnFinished();
    }

    public void onTurnFinished() {
        Semaphore.Instance.onTurnEnd(turnIndex);
        if (secadoDelTo) {
            Semaphore.Instance.RemoveTurn(turnIndex);
            // TODO, vuelve a poner como suelo normal :3  y quitar el dry, sera el Node posiblemete

            Debug.LogWarning("Quiero ser una casilla de verdad!");
        }
    }
}
