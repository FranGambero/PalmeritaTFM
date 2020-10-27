using System.Collections;
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
                Debug.Log("Haz movimiento");
                movementCoroutine = StartCoroutine(Move(targetNode));
            }
        }

        private IEnumerator Move(Node targetNode) {
            isMoving = true;
            Movement.StopAllCoroutines();
            yield return StartCoroutine(Movement.Move(CurrentNode, targetNode, this));
            CurrentNode = targetNode;
            isMoving = false;
            onEndWalk?.Invoke();
        }
        #endregion

        #region Turn
        public void onTurnStart(int currentIndex) {
            if (IsMyTurn)
                CardManager.Instance.drawNextCard();
        }

        public void onTurnFinished() {
            Semaphore.Instance.onTurnEnd(turnIndex);
        }
        #endregion
    }
}