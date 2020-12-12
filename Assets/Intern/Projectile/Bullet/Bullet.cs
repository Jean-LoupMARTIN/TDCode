

using UnityEngine;

public class Bullet : Projectile
{
    public float speed, damage;
    public GameObject bulletImpact, debrit;
    

    private void Update() {

        float dist = speed * Time.deltaTime;

        if (Physics.Raycast(
            transform.position,
            transform.forward,
            out RaycastHit hit,
            dist,
            LayersManager.projCollisionLayer)) {

            Enemy enemy = Tool.searchEnemy(hit.collider.transform);
            if (enemy) enemy.takeDamage(damage, transform.forward);

            GameObject bi = Instantiate(bulletImpact, hit.point, transform.rotation);
            Destroy(bi, 1);

            GameObject d = Instantiate(debrit, hit.point, transform.rotation);
            MeshRenderer mr = hit.collider.gameObject.GetComponent<MeshRenderer>();
            if (mr) {
                ParticleSystemRenderer dr = d.GetComponent<ParticleSystemRenderer>();
                dr.material = mr.material;
            }
            Destroy(d, 3);

            destroy();
        }
        else transform.Translate(Vector3.forward * dist);
    }
}
