using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TillingMovement : MonoBehaviour {
    public Material material;
    public float scroll_speed = 0.1f;
    float movement;
    public Vector2 offset;
    Vector2 offsetMov;
    private void Start() {
        if (!material)
            material = GetComponent<Renderer>().material;
        movement = scroll_speed * Time.deltaTime;
        offsetMov *= movement;

    }
    private void Update() {
        movement = scroll_speed * Time.deltaTime;
        offsetMov += offset * movement;

        material.SetTextureOffset("_MainTex", offsetMov);
    }
}
