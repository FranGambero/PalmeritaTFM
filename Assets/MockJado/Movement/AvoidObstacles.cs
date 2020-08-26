using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ElJardin {
    public class AvoidObstacles : SteeringBehaviour {
        public Transform targetT;
        public float stopDistance = 0.5f;
        public float detectionDistance = 3f;
        public LayerMask obstacleMask, jumpMask;

        private Vector3 _targetDir;
        private Vector3 _avoidDir;
        private RaycastHit _obstacleHit, jumpRaycast;
        private Collider _obstacleCollider, jumpCollider;
        public bool hasObjective = false;

        public Sequence mySeq;

        // Variables de apoyo
        private float _distanceToTarget => _targetDir.magnitude;
        private bool _obstacleDetected => _obstacleCollider != null;
        private bool _jumpDetected => jumpCollider != null;
        private float _obstacleDist => Vector3.Distance(_obstacleCollider.ClosestPoint(transform.position), transform.position);
        private float jumpObsDist => Vector3.Distance(jumpCollider.ClosestPoint(transform.position), transform.position);

        protected override void Update() {

            if (targetT && Vector3.Distance(transform.position, targetT.position) < stopDistance) {
                quita();
            }

            if (targetT != null) {

                _targetDir = new Vector3(targetT.position.x, transform.position.y, targetT.position.z) - transform.position; // Vector dirección al objetivo
                if (_obstacleDetected) { // Si hay obstáculo
                    _avoidDir = transform.position - new Vector3(_obstacleCollider.transform.position.x, transform.position.y, _obstacleCollider.transform.position.z); // Vector dirección de evasión
                    //mySeq = jumpSequence(_obstacleCollider.transform);
                    //mySeq.Play();
                }
                base.Update(); // Llamada al Update del padre
            }

        }

        public void assignObjective(Transform destiny) {
            targetT = destiny;
        }

        public void quita() {
            targetT = null;
            CharacterController.Instance.stopWalkingPS();
        }

        protected override void OnDrawGizmosSelected() {
            // Gizmos objetivo
            Gizmos.color = Color.green;
            //Gizmos.DrawWireSphere(targetT.position, 0.5f);
            Gizmos.DrawRay(transform.position, _targetDir);

            // Gizmos detección
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, transform.forward * detectionDistance); // Rayo detector
            if (_obstacleDetected) {
                Gizmos.DrawRay(transform.position, _avoidDir); // Vector dirección de evasión
                Gizmos.DrawWireSphere(_obstacleCollider.ClosestPoint(transform.position), 0.5f); // Punto del obstáculo más cercano
            }

            base.OnDrawGizmosSelected(); // Llamada a Gizmos del padre
        }

        protected override Vector3 CalculateDirection() {
            // Detección de obstáculo al frente [Se podría mejorar la detección con ángulo a izquierda y derecha]
            if (Physics.Raycast(transform.position, transform.forward, out _obstacleHit, detectionDistance, obstacleMask))
                _obstacleCollider = _obstacleHit.collider;


            if (_obstacleDetected) // Si hay obstáculo almacenado
            {
                if (_obstacleDist < detectionDistance) // Si el obstáculo está dentro de la distancia de evasión
                {
                    float n = _obstacleDist / detectionDistance; // Factor peso que depende del factor de evasión (a más factor, más peso damos a evadir el objeto)
                    return _avoidDir.normalized * (1 - n) + _targetDir.normalized * n; // retorna la dirección proporcionada
                }
                _obstacleCollider = null; // Aquí entramos si el obstáculo queda fuera de la distancia de evasión
                return _targetDir; // Nos dirigimos a la posición objetivo
            } else if (_distanceToTarget > stopDistance) // Si no hay obstáculo y aún no hemos llegado
                return _targetDir;
            else // Si hemos llegado
                return transform.forward;
        }

        //private Vector3 lookDown() {
        //    if(Physics.Raycast(transform.position, -transform.up, out jumpRaycast, detectionDistance, jumpMask)) {
        //        //jumpCollider
        //    }

        //    return 

        //}

        protected override float CalculateSpeed() {
            if (_distanceToTarget > stopDistance) // Aún no ha llegado al objetivo
            {
                if (_obstacleCollider)
                    return Mathf.Lerp(0.5f, 1f, _obstacleDist / detectionDistance); // Proporcionar la velocidad a la distancia al obstáculo
                else
                    return 1;
            } else
                return _distanceToTarget / stopDistance; // Velocidad proporcionada a la distancia al objetivo
        }

        public Sequence jumpSequence(Transform obstacleT) {
            Sequence jumpSeq = DOTween.Sequence();

            //jumpSeq.Append(transform.DOJump(transform.forward * 3.2f, .4f, 1, 1));

            jumpSeq.Append(transform.DOJump(new Vector3(obstacleT.position.x, obstacleT.position.y + .9f, obstacleT.position.z), .4f, 1, 1));
            //jumpSeq.Append(transform.DOJump(new Vector3(obstacleT.position.x, obstacleT.position.y, transform.forward.z + 3f), 1f, 1, 1));

            return jumpSeq;
        }
    }
}
