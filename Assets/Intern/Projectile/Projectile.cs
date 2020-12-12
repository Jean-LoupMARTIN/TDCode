
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    private int life = 0;

    private void Awake() {
        WaveManager.inst.endWaveEvent.AddListener(destroy);
    }

    private void FixedUpdate() {
        life++;
        if (life > 1000) destroy();
    }

    protected void destroy() { Destroy(gameObject); }
}
