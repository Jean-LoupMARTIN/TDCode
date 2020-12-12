

using UnityEngine;
using UnityEngine.AI;

public abstract class Agent : MonoBehaviour
{
    public readonly static float PROBA_WAIT_ANIM = 0.1f;

    public NavMeshAgent nva;
    public float dist_exigence = 0.2f;
    public float speed;
    public Animator animator;
    public string[] waitAnims;

    protected bool moving = false;

    protected virtual void Awake() { nva.updateRotation = false; }

    private void Update() {
        nva.velocity = Vector3.zero;

        if (moving) {
            if (Tool.dist(transform, nva.destination) > dist_exigence) move();
            else {
                moving = false;
                animator.Play("Wait");
                if(Tool.hasParam(animator, "move")) animator.SetFloat("move", 0);
            }
        }

        else if (!WaveManager.inWave && Tool.animIs(animator, "Wait") && Tool.percent(PROBA_WAIT_ANIM))
            Tool.playRandAnim(animator, waitAnims);
    }

    protected abstract void move();

    public void moveTo(GameObject target) { moveTo(target.transform); }
    public void moveTo(Transform  target) { moveTo(target.position); }
    public void moveTo(Vector3 target) {

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 100, nva.areaMask)) {
            target = hit.position;
            nva.SetDestination(target);
            if (Tool.dist(Tool.last(nva.path.corners), transform.position) > 2 * dist_exigence) {
                nva.SetDestination(target);
                moving = true;
            }
            else nva.path.ClearCorners();
        }
    }



    public void enable()  { enable(true); }
    public void desable() { enable(false); }
    public void enable(bool b) { nva.enabled = b; }

    public void setPos(Vector3 pos) {
        desable();
        transform.position = pos;
        enable();
    }
 }