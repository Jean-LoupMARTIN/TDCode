
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleporter : Tower
{
    public float delay;
    public Animator animator;
    private float teleporting = 0;
    private bool teleported = true;
    private Vector3 target;
    public GameObject shockWave;

    protected override void Awake() {
        base.Awake();
        animator.SetFloat("speed", 1f / delay);
        WaveManager.inst.endWaveEvent.AddListener(cancelTeleportation);
        funcs.Add("teleport", funcs["moveTo"]);
        funcs.Remove("moveTo");
    }

    public override void moveTo(Vector3 target) {
        if (teleporting <= 0) {
            if (NavMesh.SamplePosition(target, out NavMeshHit hit, 200, agent.nva.areaMask)) {
                teleporting = delay;
                teleported = false;
                this.target = hit.position + 0.3f * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                animator.SetTrigger("teleport");
                createShockWave();
            }
        }
    }

    private void createShockWave() {
        GameObject sw = Instantiate(shockWave, transform.position, Quaternion.identity);
        sw.transform.localScale = 4 * new Vector3(1, 2, 1);
        Destroy(sw, 1);
    }

    protected override void Update() {

        if (teleporting > 0) {
            teleporting -= Time.deltaTime;
            if(!teleported && teleporting < delay*0.75) {
                teleported = true;
                agent.setPos(target);
                createShockWave();
            }
        }

        base.Update();
    }

    private void cancelTeleportation() {
        teleporting = 0;
        teleported = true;
        for(int i = 0; i < 5; i++) animator.Play("Wait"); // lance plusieur fois pour etre sur
    }
}

