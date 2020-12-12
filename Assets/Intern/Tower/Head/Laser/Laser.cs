
using UnityEngine;

public class Laser : TowerHead
{
    public Transform firePoint;
    public GameObject laserBeamGO;
    private LaserBeam laserBeam;
    public Animator animator;

    public float laserDamage;

    private void Start() {
        laserBeam = Instantiate(laserBeamGO).GetComponent<LaserBeam>();
        desactiveLaserBeam();

        WaveManager.inst.startWaveEvent.AddListener(activeLaserBeam);
        WaveManager.inst.endWaveEvent.AddListener(desactiveLaserBeam);
    }

    public void activeLaserBeam()    { laserBeam.gameObject.active = true;  animator.SetBool("shooting", true); }
    public void desactiveLaserBeam() { laserBeam.gameObject.active = false; animator.SetBool("shooting", false); }

    protected override void Update() {
        base.Update();
        laserBeam.transform.position = firePoint.position;
        laserBeam.transform.rotation = firePoint.rotation;
    }

    public void OnDestroy() { if(laserBeam) Destroy(laserBeam.gameObject); }
}
