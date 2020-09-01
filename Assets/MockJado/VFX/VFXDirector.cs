using ElJardin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXDirector : Singleton<VFXDirector> {
    public List<VFX> vfxEffects;
    public ParticleSystem Play(string name, Transform position) {
        VFX vfx = vfxEffects.Find(v => v.name.Equals(name));
        ParticleSystem ps = null;
        if (vfx != null) {
            ps = vfx.Play(position);
            ps.transform.SetParent(this.transform);
            StartCoroutine(DisablePS(ps, vfx.time));
        } else {
            Debug.LogWarning("El vfx no existe: " + name);
        }
        return ps;
    }
    IEnumerator DisablePS(ParticleSystem ps, float time) {
        yield return new WaitForSeconds(time);
        ps.Stop();
        ps.gameObject.SetActive(false);
    }
    [Serializable]
    public class VFX {

        public string name;
        public float time;
        public ParticleSystem PSPrefab;
        public List<ParticleSystem> pool;

        public ParticleSystem Play(Transform position) {
            ParticleSystem particles;
            if (!(particles = pool.Find(p => !p.gameObject.activeInHierarchy))) {
                particles = Instantiate(PSPrefab);
                pool.Add(particles);
            }
            particles.transform.position = position.position;
            particles.transform.rotation = position.rotation;
            particles.gameObject.SetActive(true);
            particles.time = 0;
            particles.Play();

            return particles;
        }

        bool IsPlaying(Transform parent) {
            bool playing = false;

            foreach (ParticleSystem item in parent.GetComponentsInChildren<ParticleSystem>()) {
                if (playing = item.IsAlive(true)) {
                    break;
                }
            }
            return playing;
        }
    }
}
