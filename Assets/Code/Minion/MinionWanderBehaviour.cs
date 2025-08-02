using UnityEngine;

public class MinionWanderBehaviour : LogicMachineBehaviour<MinionLogicManager>
{
    [Header("Movement")]
    public float moveSpeed;
    public float destinationThreshold = 0.1f;



    [Header("Point Selection")]
    public float wanderDistance = 2f;
    public LayerMask castMask = ~0;
    private Vector3? wanderPoint;

    [Header("Waiting")]
    public Vector2 waitingTicksRange;
    private float? waitingTicks;

    public override void OnAwake()
    {
    }

    public override void OnEnter()
    {
    }

    public override void OnUpdate()
    {
        if (!wanderPoint.HasValue)
            AssignWanderPoint();
        else
        {
            if (Vector3.Distance(transform.position, wanderPoint.Value) > destinationThreshold)
                MoveToPoint();
            else
            {

                if (!waitingTicks.HasValue)
                    waitingTicks = Random.Range(waitingTicksRange.x, waitingTicksRange.y);

                waitingTicks--;

                if (waitingTicks <= 0)
                {
                    wanderPoint = null;
                    waitingTicks = null;
                }
            }
        }
    }


    void AssignWanderPoint()
    {
        for (int i = 0; i < 500; i++)
        {
            Vector2 randomDir = Random.insideUnitCircle;
            Vector3 randomPoint = logicAnimator.transform.position + new Vector3(randomDir.x, 0, randomDir.y) * wanderDistance;

            bool hasHit = Physics.Raycast(randomPoint + Vector3.up, -Vector3.up, 10, castMask);

            if (hasHit)
            {
                wanderPoint = randomPoint;

                Debug.DrawRay(randomPoint + Vector3.up, -Vector3.up, Color.yellow, 4);
                return;
            }
        }
    }

    void MoveToPoint()
    {
        var dir = wanderPoint.Value - logicAnimator.transform.position;
        dir.Normalize();

        logicAnimator.transform.position += dir * moveSpeed * Time.deltaTime;
    }

    public override void OnExit()
    {
    }
}
