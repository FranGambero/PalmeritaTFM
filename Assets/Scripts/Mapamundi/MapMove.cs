using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapMove : MonoBehaviour {
    Sequence moveSequence;
    public Transform nextT;
    public bool moveFinished;
    public int currentLevel;
    public LevelManager levelManager;

    private void Awake() {
        currentLevel = 1;
        moveFinished = false;
    }

    public void focusMove(Transform nextPosition, int targetLevel) {

        if (currentLevel != targetLevel) {
            List<Transform> listaPosiciones = MakeRecorrido(targetLevel);
            Sequence moveSeq = DOTween.Sequence();
            moveFinished = false;
            for (int i = 0; i < listaPosiciones.Count; i++) {
                // Easing
                Ease moveEase = Ease.OutCubic;
                float moveTime = .5f + listaPosiciones.Count * .25f;

                moveSeq.Append(transform.DOMove(listaPosiciones[i].position, moveTime).SetEase(moveEase));
            }
            moveSeq.Play().OnComplete(() => {
                Debug.Log("He terminao");
                moveFinished = true;
            });

            currentLevel = targetLevel;
        }
    }

    private List<Transform> MakeRecorrido(int targetLevel) {
        List<Transform> listita = new List<Transform>();

        if (targetLevel > currentLevel) {
            for (int i = currentLevel + 1; i <= targetLevel; i++) {
                listita.Add(levelManager.levelTList[i].transform);
            }
        } else {
            for (int i = currentLevel - 1; i >= targetLevel; i--) {
                listita.Add(levelManager.levelTList[i].transform);
            }
        }

        return listita;
    }

}
