
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public static class Tool
{
    private static Camera cam;

    static Tool() {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public static int   rand(int   max) { return Random.Range(0, max); }
    public static float rand(float max) { return Random.Range(0, max); }
    public static T rand<T>(T[]     list) where T : Object { if(list.Length == 0) return null; return list[rand(list.Length)]; }
    public static T rand<T>(List<T> list) where T : Object { if(list.Count  == 0) return null; return list[rand(list.Count)]; }
    public static bool percent(float proba) { return rand(100f) < proba; }

    public static float progress(float t, float max) { return t / max; }

    public static void playRandAnim(Animator animator, string[] anims) {
        if (anims.Length == 0) return;
        animator.Play(anims[rand(anims.Length)]);
    }
    public static bool animIs(Animator animator, string name) { return animator.GetCurrentAnimatorStateInfo(0).IsName(name); }
    public static bool hasParam(Animator animator, string name) {
        foreach (AnimatorControllerParameter param in animator.parameters)
            if (param.name == name) return true;
        return false;
    }
    public static float dist(Transform a, Transform b) { return dist(a.position, b.position); }
    public static float dist(Vector3   a, Transform b) { return dist(a,          b.position) ; }
    public static float dist(Transform a, Vector3   b) { return dist(a.position, b); }
    public static float dist(Vector3 a, Vector3 b) { return (a - b).magnitude; }

    public static Vector3 dir(Transform a, Transform b, bool normalized) { return dir(a.position, b.position, normalized); }
    public static Vector3 dir(Vector3   a, Transform b, bool normalized) { return dir(a,          b.position, normalized); }
    public static Vector3 dir(Transform a, Vector3   b, bool normalized) { return dir(a.position, b,          normalized); }
    public static Vector3 dir(Vector3   a, Vector3   b, bool normalized) {
        if (normalized) return (b - a).normalized;
        return b - a;
    }

    public static Enemy searchEnemy(Transform trans) {
        Transform parent = trans.parent;
        if (parent) {
            Enemy enemy = parent.GetComponent<Enemy>();
            if (enemy) return enemy;
        }
        return null;
    }

    public static Tower searchTower(Transform trans) {
        for (; trans != null; trans = trans.parent) {
            Tower t = trans.GetComponent<Tower>();
            if (t != null) return t;
        }
        return null;
    }

    public static T last<T>(T[]     list) { return list[list.Length - 1]; }
    public static T last<T>(List<T> list) { return list[list.Count  - 1]; }
    //public static T last<T>(T[]     list) where T : Object { if(list.Length == 0) return null; return list[list.Length - 1]; }
    //public static T last<T>(List<T> list) where T : Object { if(list.Count  == 0) return null; return list[list.Count  - 1]; }


    public static bool mouseHit(out RaycastHit hit, int layer = -1) {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(layer == -1) return Physics.Raycast(ray, out hit, 300);
        else            return Physics.Raycast(ray, out hit, 300, layer);
    }



    public static void shakeSphere(Transform pos, float strong) { shakeSphere(pos.position, strong); }
    public static void shakeSphere(Vector3   pos, float strong) {
        float radius = 25;
        float d = dist(pos, cam.transform);
        if (d < radius) {
            float p = Mathf.Max(1 - progress(d, radius), 0.2f);
            strong *= p;
            CameraShaker.Instance.ShakeOnce(4*strong, 4*strong, 0, strong*p);
        }
    }

    public static Vector2 world2screen(Transform pos) { return world2screen(pos.position); }
    public static Vector2 world2screen(Vector3 pos) {
        pos = cam.WorldToScreenPoint(pos);
        return new Vector2(pos.x, pos.y);
    }

    public static bool pointInRect(Vector2 pos, RectTransform rt) {
        float x = pos.x, y = pos.y,
            w = rt.sizeDelta.x, h = rt.sizeDelta.y,
            l = rt.position.x,
            d = rt.position.y,
            r = rt.position.x + w,
            t = rt.position.y + h;

        return x > l && x < r && y > d && y < t;
    }
}
