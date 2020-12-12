

using UnityEngine;

public class LayersManager : MonoBehaviour
{
    public static int grabTowerLayer;
    public static int projCollisionLayer;
    public static int enemyMaskArea = 8;

    private void Awake() {
        Physics.IgnoreLayerCollision(12, 10); // Proj Tower
        Physics.IgnoreLayerCollision(12, 11); // Proj CubePart
        Physics.IgnoreLayerCollision(12, 12); // Proj Proj
        Physics.IgnoreLayerCollision(12, 13); // Proj Target
        Physics.IgnoreLayerCollision(11, 11); // CubePart CubePart

        grabTowerLayer = ~mask("Tower");
        projCollisionLayer = ~mask("Tower") & ~mask("CubePart") & ~mask("Projectile") & ~mask("Target");
    }

    private int mask(string name) {return  LayerMask.GetMask(name);}
}
