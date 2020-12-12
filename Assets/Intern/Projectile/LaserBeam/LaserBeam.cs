

using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float damage;

    public LineRenderer lr;
    public Transform end;
    public GameObject debrit;

    private void Update() {

        float dist = 500;

        if (Physics.Raycast(
            transform.position,
            transform.forward,
            out RaycastHit h,
            dist,
            LayersManager.projCollisionLayer)) {

            dist = (h.point - transform.position).magnitude;

            if(Tool.percent(10)) {
                GameObject d = Instantiate(debrit, h.point, transform.rotation);
                MeshRenderer mr = h.collider.gameObject.GetComponent<MeshRenderer>();
                if (mr) {
                    ParticleSystemRenderer dr = d.GetComponent<ParticleSystemRenderer>();
                    dr.material = mr.material;
                }
                Destroy(d, 3);

                Tool.shakeSphere(h.point, 0.8f);
            }

            Enemy enemy = Tool.searchEnemy(h.collider.transform);
            if (enemy) enemy.takeDamage(damage, transform.forward, 0);
        }
        lr.SetPosition(1, Vector3.forward * dist);
        end.position = transform.position + transform.forward * dist;
    }


}
