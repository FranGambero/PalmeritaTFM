using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ElJardin.Characters {
    public class BongormigaAfterAction : MonoBehaviour {
        public Animator anim;
        Coroutine animCoroutine;

        public UnityEvent onAnimationEnd = new UnityEvent();

        public void PlayBongos() {
            Action();
        }

        void Action() {
            anim.Play("PlayBongo0");
            VFXDirector.Instance.Play("AntIn", transform.position);

            animCoroutine = StartCoroutine(DestroyOnEndBongos());
        }

        IEnumerator DestroyOnEndBongos() {
            float aux = 2;
            yield return new WaitForSeconds(aux);
            onAnimationEnd.Invoke();
            aux = 1.5f;
            yield  return new WaitForSeconds(aux);
            VFXDirector.Instance.Play("AntOut", transform.position);
            Destroy(this.gameObject);
        }

        void OnDestroy() {
            if (animCoroutine != null)
                StopCoroutine(animCoroutine);
        }
    }
}