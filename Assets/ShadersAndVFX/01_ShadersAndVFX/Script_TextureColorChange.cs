using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TextureColorChange : MonoBehaviour
{
    public Renderer mrenderer;
    public Color SelectedColor;
    public Color NotSelectedColor;

    MaterialPropertyBlock materialPropertyBlock;

    private void Awake()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        //Puse dos ifs con pulsar teclas para comprobar si funcionaba, la cosa es que tengáis una condición para que cambie de color y otra para que vuelva al original
       
        if (Input.GetKeyDown(KeyCode.Space)) //AQUÍ PONED LA CONDICIÓN PARA QUE CAMBIE DE COLOR (QUE PASE EL RATÓN POR ENCIMA O LO QUE SEA)
        {
            mrenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor("_Color", SelectedColor);
            mrenderer.SetPropertyBlock(materialPropertyBlock);
        }

        if (Input.GetKeyDown(KeyCode.A)) //aquí la de que vuelva a lo suyo
        {
            mrenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor("_Color", NotSelectedColor);
            mrenderer.SetPropertyBlock(materialPropertyBlock);
        }

    }
}
