﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin {
    public class CharacterController : MonoBehaviour {
        private Rigidbody myRb;
        private Vector3 firstPos, lastPos;
        public List<Vector3> listPos = new List<Vector3>();
        public float currentVelocity, normalVelocity, slowVelocity;
        public float offsetDistance = .5f;

        public ParticleSystem constructionPS;
        public ParticleSystem walkingPS;

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
        }

        public void MoveToPosition(Node destNode, Node lastNode) {
            listPos.Clear();

            firstPos = new Vector3(destNode.transform.position.x, transform.position.y, destNode.transform.position.z);
            listPos.Add(firstPos);

            if (lastNode != destNode) {
                lastPos = new Vector3(lastNode.transform.position.x, transform.position.y, lastNode.transform.position.z);
                listPos.Add(lastPos);
            }

            StartCoroutine(ReachPosition());
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
    }
}
