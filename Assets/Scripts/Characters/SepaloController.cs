using System;
using System.Collections;
using System.Collections.Generic;
using ElJardin.Movement;
using UnityEngine;
using UnityEngine.Events;

namespace ElJardin.Characters {
    public class SepaloController : MonoBehaviour, ITurn {
        #region SerializeFields
        [Header("Starting Variables")] public Vector2 StartingNode;

        [Header("Injection")] public MovementController Movement;
        [Header("Turn")] [SerializeField] int myTurnIndex;
        public Coroutine movementCoroutine;
        public LayerMask groundLayer;
        [HideInInspector]
        public UnityEvent OnEndWalk => onEndWalk;
        UnityEvent onEndWalk = new UnityEvent();

        public bool isMoving;
        #endregion

        public Node CurrentNode { get; set; }

        #region Accessors
        public int turnIndex {
            get => myTurnIndex;
            set => myTurnIndex = value;
        }

        public bool IsMyTurn => turnIndex == Semaphore.Instance.currentTurn;
        #endregion


        void Start() {
            CurrentNode = MapManager.Instance?.GetNode((int)StartingNode.x, (int)StartingNode.y);
        }

        #region Actions
        public void DoTheMove(Node targetNode) {
            if (IsMyTurn) {
                //StopAllCoroutines();
                if (movementCoroutine != null) {
                    StopCoroutine(movementCoroutine);
                    movementCoroutine = null;
                }
                movementCoroutine = StartCoroutine(Move(targetNode));
            }
        }

        internal void CheckGrownd() {
            RaycastHit groundHits;
            //TODO Que el suelo tenga su porpia layer y que aqui se coja sola esa layer y no todas
            if (Physics.Raycast(transform.position, Vector3.down, out groundHits, 1f, groundLayer)) {
                CurrentNode = groundHits.collider.GetComponent<Node>();
            }
        }

        private IEnumerator Move(Node targetNode) {
            isMoving = true;
            GetComponentInChildren<ProtaAnimationController>().StartWalkAnimation(targetNode);
            Movement.StopAllCoroutines();
            yield return StartCoroutine(Movement.Move(CurrentNode, targetNode, this));
            CurrentNode = targetNode;
            isMoving = false;
            onEndWalk?.Invoke();
        }
        #endregion

        #region Turn
        public void onTurnStart(int currentIndex) {
            if (IsMyTurn) {
                Debug.Log("Turno sepalo");
                CardManager.Instance.DrawNextCard();
            }
        }

        public void onTurnFinished() {
            Semaphore.Instance.onTurnEnd(turnIndex);
        }
        #endregion
    }
}