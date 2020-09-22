using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TillingMovement : MonoBehaviour {
    public Material material;
    public float scroll_speed = 0.1f;
    float movement;
    Vector2 offset;
    private void Start() {
        movement = scroll_speed * Time.deltaTime;
        Vector2 offset = new Vector2(movement, 0);
    }
    private void Update() {
        movement = scroll_speed * Time.deltaTime;
        offset.x += movement;
        material.SetTextureOffset("_MainTex", offset);
    }
}
