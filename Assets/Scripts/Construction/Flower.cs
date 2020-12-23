using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {
    public bool hasWaterAround;
    public List<DoGrow> listaElementosCrecedores;

    private void Awake() {
        hasWaterAround = false;
    }

    [ContextMenu("CRECE CARLA")]
    public void activateFlor() {
        hasWaterAround = true;
        GetComponentInChildren<LotoAnimatorController>().LotoOpen();
        listaElementosCrecedores.ForEach(e => e.RandomGrow());
    }

    [ContextMenu("DESCRECE CARLA")]
    public void deActivateFlor() {
        hasWaterAround = false;
        // Hace la animación un poco regulinchi
        GetComponentInChildren<LotoAnimatorController>().LotoClose();
        listaElementosCrecedores.ForEach(e => e.Shrink());
    }
}
