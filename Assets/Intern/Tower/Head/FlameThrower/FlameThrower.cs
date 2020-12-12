

using UnityEngine;

public class FlameThrower : TowerHead
{
    public static readonly float DAMAGE = 0.5f;

    public Transform flamePoint;
    public GameObject flamePrefab;
    private ParticleSystem flame;


    private void Start() {
        flame = Instantiate(flamePrefab).GetComponent<ParticleSystem>();
        flame.Stop();

        WaveManager.inst.startWaveEvent .AddListener(flame.Play);
        WaveManager.inst.endWaveEvent   .AddListener(flame.Stop);
    }

    protected override void Update() {
        base.Update();
        flame.transform.position = flamePoint.position;
        flame.transform.rotation = flamePoint.rotation;
    }

    public void OnDestroy() { if(flame) Destroy(flame.gameObject); }
}
