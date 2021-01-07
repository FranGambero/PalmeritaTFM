using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ElJardin.Characters
{
    public class BongormigaAfterAction : MonoBehaviour
    {
        public Animator anim;
        Coroutine animCoroutine;
        
        public UnityEvent onAnimationEnd = new UnityEvent();

        public void PlayBongos()
        {
            Action();
        }

        void Action()
        {
            anim.Play("PlayBongo0");
            animCoroutine = StartCoroutine(DestroyOnEndBongos());
        }

        IEnumerator DestroyOnEndBongos()
        {
            var aux = 83;
            while( aux > 0)
            {
                aux--;
                yield return null;
            }
            onAnimationEnd.Invoke();
            Destroy(this.gameObject);
        }

        void OnDestroy()
        {
            if(animCoroutine != null)
                StopCoroutine(animCoroutine);
        }
    }
}