using System.Collections;
using ElJardin.Movement;
using UnityEngine;

namespace ElJardin.Characters
{
    public class SepaloController : MonoBehaviour, ITurn
    {
        #region SerializeFields
        [Header("Starting Variables")] public Vector2 StartingNode;

        [Header("Injection")] public MovementController Movement;
        [Header("Turn")] [SerializeField] int myTurnIndex;
        #endregion

        Node currentNode;

        #region Accessors
        public int turnIndex
        {
            get => myTurnIndex;
            set => myTurnIndex = value;
        }

        public bool IsMyTurn => turnIndex == Semaphore.Instance.currentTurn;
        #endregion


        void Start()
        {
            currentNode = MapManager.Instance?.GetNode((int)StartingNode.x, (int)StartingNode.y);
        }

        #region Actions
        public IEnumerator Move(Node targetNode)
        {
            yield return StartCoroutine(Movement.Move(currentNode, targetNode));
            currentNode = targetNode;
            //TODO: abstraer comportamiento
            BuildManager.Instance.buildCells();
            onTurnFinished();
        }
        #endregion

        #region Turn
        public void onTurnStart(int currentIndex)
        {
            if(IsMyTurn)
                CardManager.Instance.drawNextCard();
        }

        public void onTurnFinished()
        {
            Semaphore.Instance.onTurnEnd(turnIndex);
        }
        #endregion
    }
}