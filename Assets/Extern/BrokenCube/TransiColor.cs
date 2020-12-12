

using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TransiColor : MonoBehaviour
{
    [HideInInspector] public Material start;
    public Material end;
    public int time = 200;
    private int compter = 0;

    private MeshRenderer mr;

    private void Awake() {
        mr = GetComponent<MeshRenderer>();
        start = mr.material;
    }

    public void Update() {
        compter++;
        float progress = Tool.progress(compter, time);
        Color color = Color.Lerp(start.color, end.color, progress);
        mr.material.color = color;
        if (compter == time) enabled = false;
    }
}
