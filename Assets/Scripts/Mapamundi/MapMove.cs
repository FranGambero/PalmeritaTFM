using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MapMove : MonoBehaviour {
    Sequence moveSequence;
    public Transform nextT;
    public bool moveFinished;
    public int currentLevel;
    public LevelManager levelManager;

    private void Awake() {
        levelManager.findLevels();
        moveFinished = true;
    }


    private void Start() {
        MapamundiManager.Instance.onZoneChange += OnZoneChanged;
        OnZoneChanged(MapamundiManager.Instance.currentZone);
        CheckLevelPosition();
    }
    private void CheckLevelPosition() {
        int lastPlayedLevel = SessionVariables.Instance.levels.lastPlayedLevel;
        currentLevel = lastPlayedLevel != -1 ? lastPlayedLevel : 0;
        //this.transform.position = levelManager.levelTList[currentLevel].transform.position;
        if (SessionVariables.Instance.sceneData.lastScene == 0) {

            focusMove(currentLevel, true);
        } else {
            transform.position = levelManager.levelTList[currentLevel].transform.position;
        }
    }

    private void directMove() {

    }

    public void focusMove(int targetLevel, bool forceMove = false) {

        if (currentLevel != targetLevel || forceMove) {
            Debug.Log("ENTRAMOS LO PRIMERO " + currentLevel + " / " + targetLevel);
            if (forceMove) {
                //transform.position = levelManager.levelTList[currentLevel].transform.position;
                transform.DOMove(levelManager.levelTList[currentLevel].transform.position, 2f).SetEase(Ease.Linear);
            } else {


                List<Transform> listaPosiciones = MakeRecorrido(targetLevel);
                Sequence moveSeq = DOTween.Sequence();
                moveFinished = false;
                GetComponent<Animator>().SetBool("Walking", true);
                for (int i = 0; i < listaPosiciones.Count; i++) {
                    // Easing
                    Ease moveEase = Ease.Linear;
                    float moveTime = .5f + listaPosiciones.Count * .25f;

                    moveSeq.Append(transform.DOMove(listaPosiciones[i].position, moveTime).SetEase(moveEase));
                }
                moveSeq.Play().OnComplete(() => {
                    Debug.Log("He terminao");
                    moveFinished = true;
                    GetComponent<Animator>().SetBool("Walking", false);
                });

                currentLevel = targetLevel;
            }
        }
    }
    public void OnStep() { //Animator
        AkSoundEngine.PostEvent("Steps_Mapa_In", gameObject);
        GetComponentInChildren<ParticleSystem>().Play();
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
    private void OnZoneChanged(int zone) {
        if (levelManager.zone == zone) {
            gameObject.SetActive(true);
            //  focusMove(currentLevel);
        } else {
            gameObject.SetActive(false);
        }
    }
}
