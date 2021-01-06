using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowerManager : Singleton<FlowerManager>
{
    public List<Flower> flores;
    public int currentRTPCValue;
    public int targetValue;

    private void Awake() {
        //currentRTPCValue = 0;
    }

    private void Start() {
        QuitSound();
    }

    public bool CountFlowerStatus() {
        Debug.LogWarning("TIENES " + flores.Count + " flores " + flores.TrueForAll(f => f.flowerOpened));
        return flores.TrueForAll(f => f.flowerOpened);
    }

    public void OnFlowerOpening() {
        ChangeSound();
    }

    public void ChangeSound() {
        targetValue += 100 / flores.Count;
        float duration = 2;
        DOTween.To(() => currentRTPCValue, x => currentRTPCValue = x, targetValue, duration).OnUpdate(() => assignFlowerRTPCValue(currentRTPCValue));
    }

    public void QuitSound() {
        this.targetValue = 0;
        float duration = 2;
        DOTween.To(() => currentRTPCValue, x => currentRTPCValue = x, targetValue, duration).OnUpdate(() => assignFlowerRTPCValue(currentRTPCValue));
    }

    private void assignFlowerRTPCValue(int value) {
        AkSoundEngine.SetRTPCValue(Keys.WWise.RTPC_Flor, value);

    }
}
