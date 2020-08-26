using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour {

    // ------------ Notas reu Edu 30/07 ------------

    // Cambio mediante RTPC (Real time programming change?)
    // Random de pasos con distintas variaciones para darle más vida a los movimientos
    // Duración percusión hay que especificar cuánto durarán o intentar que sea siempre la misma

    private void Start() {
        // Si va en el Awake peta, esta linea irá en la clase que inicie bien el juego
        AkSoundEngine.PostEvent("Amb_Base_In", gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            // Para probar el efecto OUT, al darle a la tecla P se para el audio In y entra este en su lugar
            AkSoundEngine.PostEvent("Amb_Base_Out", gameObject);
        }


        if (Input.GetKeyDown(KeyCode.Space)) {
            AkSoundEngine.PostEvent("Amb_Base_In", gameObject);
        }
    }
}
