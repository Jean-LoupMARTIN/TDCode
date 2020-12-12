

using UnityEngine;

public class AgentWalk : Agent
{
    public float rotSpeed;

    protected override void move() {
        if (speed == 0 && rotSpeed == 0) return;
        Vector3 dir = nva.steeringTarget - transform.position;
        float targetO = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float crtO = transform.eulerAngles.y;
        float deltaO = (targetO - crtO) % 360;
        if (deltaO > 180) deltaO -= 360;
        else if (deltaO < -180) deltaO += 360;

        if (deltaO != 0) {
            float rot = 100 * rotSpeed /** transform.lossyScale.z*/ * Time.deltaTime;
            if (deltaO > 0) rot = Mathf.Min(deltaO, rot);
            else rot = Mathf.Max(deltaO, -rot);

            transform.Rotate(0, rot, 0);
        }

        float move = speed * Time.deltaTime;
        animator.SetFloat("move", 10 * move);
        move *= transform.lossyScale.z;
        transform.Translate(Vector3.forward * move);

    }
}
