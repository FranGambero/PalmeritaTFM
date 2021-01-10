using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ElJardin {
    public class CharacterController : Singleton<CharacterController>, ITurn {
        private Rigidbody myRb;
        private Vector3 firstPos, lastPos;
        public List<Vector3> listPos = new List<Vector3>();
        public float currentVelocity, normalVelocity, slowVelocity;
        public float offsetDistance = .5f;

        public ParticleSystem constructionPS;
        public ParticleSystem walkingPS;

        public AvoidObstacles mySteering;

        public Sequence jumpSeq;

        public int myTurnIndex;
        public bool _turneable=true;
        public int turnIndex { get => myTurnIndex; set => myTurnIndex = value; }
        public bool isMyTurn { get { return turnIndex == Semaphore.Instance.currentTurn; } }

        public bool turneable { get => _turneable; set => _turneable = value; }

        private void Awake() {
            myRb = GetComponent<Rigidbody>();
            currentVelocity = normalVelocity = 6f;
            slowVelocity = 3f;
        }

        public void startConstPS() {
            constructionPS.Play();
            currentVelocity = slowVelocity;
        }

        public void stopConstPS() {
            constructionPS.Stop();
            currentVelocity = normalVelocity;
        }

        public void startWalkingPS() {
            walkingPS.Play();
        }

        public void stopWalkingPS() {
            walkingPS.Stop();
            BuildManager.Instance.buildCells();
            onTurnFinished();
        }

        public void MoveToPosition(Node destNode, Node lastNode) {
            if (isMyTurn) {
                listPos.Clear();

                firstPos = new Vector3(destNode.transform.position.x, transform.position.y, destNode.transform.position.z);
                listPos.Add(firstPos);

                if (lastNode != destNode) {
                    lastPos = new Vector3(lastNode.transform.position.x, transform.position.y, lastNode.transform.position.z);
                    listPos.Add(lastPos);
                }

                transform.LookAt(destNode.transform);

                mySteering.assignObjective(destNode.transform);

                startWalkingPS();
            }
        }

        private IEnumerator ReachPosition() {
            int index = 0;
            Vector3 nextPos;

            startWalkingPS();

            while (index < listPos.Count) {
                nextPos = listPos[index];

                transform.LookAt(nextPos); // Quizá se pueda cambiar mejor a uno con DOTween para que quede más natural

                if (Vector3.Distance(transform.position, nextPos) > offsetDistance) {
                    transform.position = Vector3.MoveTowards(transform.position, nextPos, Time.deltaTime * currentVelocity);
                    yield return new WaitForEndOfFrame();
                } else {
                    if (!constructionPS.IsAlive())
                        startConstPS();
                    index++;
                }
            }

            stopConstPS();
            stopWalkingPS();


        }

        public Sequence jumpSequence() {
            Sequence jumpSeq = DOTween.Sequence();

            //jumpSeq.Append(transform.DOJump(new Vector3(transform.position.x, )));

            return jumpSeq;
        }

        public void onTurnStart(int currentIndex) {
            if (isMyTurn) {
            Debug.LogWarning("EMPIEZA MI VIDA" + isMyTurn);
                CardManager.Instance.DrawNextCard();
            }
        }

        public void onTurnFinished() {
            Semaphore.Instance.onTurnEnd(turnIndex);
        }
    }
}
