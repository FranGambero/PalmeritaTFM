using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public Renderer malla;
    MaterialPropertyBlock PropertyBlock;

    private void Awake()
    {
        PropertyBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            EmpiezaDissolve();
        }

    }

    void EmpiezaDissolve()
    {
        StartCoroutine(HacerDissolve());
    }

    IEnumerator HacerDissolve()
    {
        float duracion = 1;
        float t = 0;
        while (t < (duracion+1)) //si no le ponía +1, quedaban unos puntitos de que no se había disuelto del todo
        {
            malla.GetPropertyBlock(PropertyBlock);
            PropertyBlock.SetFloat("_DissolveAmount", t / duracion); //t/duración va de 0 a 1, donde 0 es no disuelto y 1 es totalmente disuelto
            malla.SetPropertyBlock(PropertyBlock);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame(); //Espera a que termine el frame para añadirle a t otro frame, así dura justo lo que queremos
        }
    }
}
