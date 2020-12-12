

using UnityEngine;

public class EnemyFly : Enemy
{
    protected override void Update()
    {
        if (WaveManager.inWave && enemyTarget == null) {
            Vector3 target = WaveManager.inst.target.position;
            float distProj = (new Vector2(target.x, target.z) -
                              new Vector2(gravity.position.x, gravity.position.z)
                              ).magnitude;
            if (distProj < DIST_GRAB) agent.animator.Play("Grab");
        }
        base.Update();
    }
}
