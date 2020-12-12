

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager inst;

    public Transform target, end;
    public Text waveTxt;

    public Transform world;
    public List<GameObject> waves;
    private int iWave = 0;
    private GameObject wave;
    public static bool inWave;

    public UnityEvent newWaveEvent   = new UnityEvent();
    public UnityEvent startWaveEvent = new UnityEvent();
    public UnityEvent endWaveEvent   = new UnityEvent();


    private void Awake() {
        inst = this;
    }

    private void Start() { newWave(); }

    private void Update() { if (Enemy.enemies.Count == 0) endWave(); }


    public void newWave() {
        wave = Instantiate(waves[iWave % waves.Count], Vector3.zero, Quaternion.identity, world);
        iWave++;
        waveTxt.text = "Wave " + iWave.ToString();
        newWaveEvent.Invoke();
    }

    public void startWave() {
        inWave = true;
        startWaveEvent.Invoke();
    }

    public void endWave() {
        inWave = false;
        Destroy(wave);
        endWaveEvent.Invoke();
        newWave();
    }
}


