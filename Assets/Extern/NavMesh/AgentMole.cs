

using UnityEngine;


public class AgentMole : AgentWalk {

    public readonly static float PROBA_DEBRIT = 10;
    public ParticleSystem[] debrits;

    protected override void Awake() {
        base.Awake();
        foreach (ParticleSystem debrit in debrits) debrit.Stop();
    }

    protected override void move() {
        base.move();

        if (moving && Tool.percent(PROBA_DEBRIT))
            Tool.rand(debrits).Play();
    }
}
