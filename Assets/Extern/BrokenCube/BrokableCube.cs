

using UnityEngine;


public class BrokableCube : MonoBehaviour
{
    public GameObject brokenCube;

    private void OnParticleCollision(GameObject other) {
        Enemy enemy = Tool.searchEnemy(transform);
        if (!enemy || other.name != "Flame(Clone)") return;
        enemy.takeDamage(FlameThrower.DAMAGE, transform.position - other.transform.position);
    }

    public void explose(float strong, Vector3 dir, Transform parent){

        Transform brokenCubeTrans = Instantiate(
            brokenCube,
            transform.position,
            transform.rotation,
            parent)
            .transform;

        brokenCubeTrans.localScale = transform.lossyScale;

        Material mat = GetComponent<MeshRenderer>().material;

        foreach (Transform part in brokenCubeTrans) {
            TransiColor transiColor = part.GetComponent<TransiColor>();
            transiColor.start = mat;

            if (strong != 0) {
                Rigidbody rg = part.GetComponent<Rigidbody>();
                rg.AddExplosionForce(70 * strong, transform.position - dir.normalized, 1);
            }
        }

        Destroy(gameObject);
    }
}
