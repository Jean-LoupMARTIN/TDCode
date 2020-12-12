

using UnityEngine;

public class AgentJump : Agent
{
    public float deltaO;
    public float probaJump;

    public string[] jumps;
    public ParticleSystem[] landDebrits;

    private bool jumping;

    protected override void Awake() {
        base.Awake();
        foreach (ParticleSystem ld in landDebrits) ld.Stop();
    }

    protected override void move() {
        if (Tool.animIs(animator, "Wait")) {
            if (jumping) jumping = false;
            if (Tool.percent(probaJump)) jump();
        }
        if (jumping) transform.Translate(Vector3.forward * Mathf.Max(speed * transform.lossyScale.z * Time.deltaTime));
    }

    private void rotate() {
        Vector3 dir = Tool.dir(transform, nva.steeringTarget, false);
        float O = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float crtO = transform.eulerAngles.y;
        float dO = O - crtO + Random.Range(-deltaO, deltaO);
        transform.Rotate(0, dO, 0);
    }

    private void jump() {
        rotate();
        jumping = true;
        Tool.playRandAnim(animator, jumps);
    }

    public void land() { jumping = false; }
    public void playLandDebrits() { foreach (ParticleSystem ld in landDebrits) ld.Play(); }
}
