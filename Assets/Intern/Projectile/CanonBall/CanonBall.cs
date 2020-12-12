

using UnityEngine;

public class CanonBall : Projectile
{
    public float damage;
    public float radius, speed;
    public GameObject explosion, debrit;

    void Start() {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }



    private void OnCollisionEnter(Collision collision) {
        explose();
        GameObject d = Instantiate(debrit, collision.contacts[0].point, transform.rotation);
        MeshRenderer mr = collision.collider.GetComponent<MeshRenderer>();
        if (mr) {
            ParticleSystemRenderer dr = d.GetComponent<ParticleSystemRenderer>();
            dr.material = mr.material;
        }
        Destroy(d, 3);
    }

    private void explose() {
        GameObject explo = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explo, 2);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders) {
            Rigidbody rg = collider.GetComponent<Rigidbody>();
            if (rg != null)  rg.AddExplosionForce(20 * damage, transform.position, radius);

            Enemy enemy = Tool.searchEnemy(collider.transform);
            if (enemy != null) {
                Vector3 dir = enemy.transform.position - transform.position;
                enemy.takeDamage(damage, dir);
            } 
        }

        Tool.shakeSphere(transform, 2f);

        destroy();
    }

}
