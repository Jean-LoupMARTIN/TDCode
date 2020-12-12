
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public float speedTranslate = 1;
    public float speedRotation = 1;
    public float speedZoom = 1;
    private Vector2 lastMousePos;
    public float distLifeBar = 10;

    private void Update()
    {
        Vector2 newMousePos = Input.mousePosition;
        Vector2 dPos = newMousePos - lastMousePos;
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // translate
        if (Input.GetMouseButton(0)) {
            Vector3 lastPos = transform.position;
            transform.Translate(
                -dPos.x/50*speedTranslate,
                Mathf.Sin(transform.eulerAngles.x * Mathf.Deg2Rad) * (-dPos.y / 50 * speedTranslate),
                Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad) * (-dPos.y / 50 * speedTranslate));

            Ray ray = new Ray(transform.position, transform.forward);
            if (!Physics.Raycast(ray, out RaycastHit hit, 500, LayerMask.GetMask("Ground")))
                transform.position = lastPos;
        }

        // rotate
        else if (Input.GetMouseButton(1)) {

            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 500, LayerMask.GetMask("Ground"))) {
                transform.RotateAround(hit.point, Vector3.up, dPos.x * speedRotation);

                Vector3 lastPos = transform.position;
                Quaternion lastRot = transform.rotation;

                transform.RotateAround(hit.point, -transform.right, dPos.y * speedRotation);

                if (transform.eulerAngles.x < 10 || transform.eulerAngles.x > 90 || transform.eulerAngles.z > 170) {
                    transform.position = lastPos;
                    transform.rotation = lastRot;
                }
            }
        }

        // zoom
        else if (scroll != 0) 
            transform.Translate(0, 0, scroll * speedZoom);
        

        lastMousePos = newMousePos;
    }


}
