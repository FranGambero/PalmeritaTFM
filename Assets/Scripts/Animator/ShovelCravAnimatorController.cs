using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelCravAnimatorController : MonoBehaviour
{
    public Animator animator;
    public ParticleSystem RealDigPS, AppearsPS, DisappearsPS;
    public Renderer bodyMat;
    public List<Texture> skins;
    [ContextMenu("DIG")]
    public void PlayDig() {
        gameObject.SetActive(true);
        animator.Play("Dig");
    }
  public void OnAnimDig() {
        bodyMat.material.SetTexture("_MainTex", skins[Random.Range(0, skins.Count)]);
        RealDigPS?.Play();
    }
    public void OnEndAnim() {
        gameObject.SetActive(false);
    }
    public void OnEndDigAnim() {
        DisappearsPS?.Play();
    }
    public void OnStartAnim() {
        AppearsPS?.Play();
    }
}
