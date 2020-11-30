using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Spin : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.Rotate(Vector2.up * speed * Time.deltaTime);
    }
}
