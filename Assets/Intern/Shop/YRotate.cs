

using UnityEngine;

public class YRotate : MonoBehaviour
{
    public float speed;

    private void Update() {
        transform.Rotate(speed * Time.deltaTime * Vector3.up);
    }
}
