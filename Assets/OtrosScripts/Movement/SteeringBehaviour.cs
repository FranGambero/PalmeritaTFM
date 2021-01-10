using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour {
    public float maxSpeed = 3f;
    public float angularSpeed = 180f;

    private Vector3 _desiredDir;
    private float _desiredSpeed;

    protected virtual void Update() {
        _desiredSpeed = CalculateSpeed();
        _desiredDir = CalculateDirection();

        Rotate();
        Move();
    }

    protected virtual void OnDrawGizmosSelected() {
        // Draw forward
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);

        // Draw target direction
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _desiredDir.normalized * 2);
    }

    // Mueve hacia el frente el pez a la velocidad actual
    protected virtual void Move() {
        transform.position += GetCurrentSpeed() * transform.forward * Time.deltaTime;
    }

    // Rota en todo momento hacia la dirección deseada
    protected virtual void Rotate() {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_desiredDir), angularSpeed * Time.deltaTime);
    }

    // La velocidad la controlamos con un factor 0 --> 1 que se aplica a la velocidad máxima
    public float GetCurrentSpeed() {
        return _desiredSpeed * maxSpeed;
    }

    // Métodos abstractos a implementar por herederos
    protected abstract float CalculateSpeed();
    protected abstract Vector3 CalculateDirection();
}
