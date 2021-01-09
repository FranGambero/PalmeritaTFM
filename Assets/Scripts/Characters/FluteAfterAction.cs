using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ElJardin.Characters
{
    public class FluteAfterAction : MonoBehaviour
    {
        public Animator anim;
        Coroutine animCoroutine;
        [SerializeField]FlautibelulaAnimController animController;
        
        public UnityEvent onAnimationEnd = new UnityEvent();

        public void PlayFlute()
        {
            Action();
        }

        void Action()
        {
            animController.Attack();
            //VFXDirector.Instance.Play("AntIn", transform.position);
            
            animCoroutine = StartCoroutine(BurnOnEndAnim());
        }

        IEnumerator BurnOnEndAnim()
        {
            float aux = 5;
            yield return new WaitForSeconds(aux);
            onAnimationEnd.Invoke();
            aux = 1.5f;
            yield return new WaitForSeconds(aux);
            Destroy(this.gameObject);
        }
        
        void OnDestroy() {
            if (animCoroutine != null)
                StopCoroutine(animCoroutine);
        }
    }
}