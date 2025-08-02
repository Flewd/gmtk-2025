using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MinionAnimationController : MonoBehaviour
{
    //public Transform movementController;
    private Animator animator;

    private Vector3 lastPos;
    private Vector3 defaultScale;

    private Vector3 deltaPos;
    private float previousOrientation;


    [Header("Snapping Animations")]
    public float stepFPS = 6f;
    private float timer = 0f;
    private float frameTime;
    private float currentAnimatorTime = 0f;

    private void Awake()
    {
        lastPos = transform.position;
        defaultScale = transform.localScale;
        previousOrientation = 1;

        animator = GetComponent<Animator>();

        //animator.speed = 0f; // We'll drive it manually
        frameTime = 1f / stepFPS;

    }

    // Update is called once per frame
    void Update()
    {
        deltaPos = transform.position - lastPos;

        FlipToVelocity();
        Billboard();
        UpdateAnimator();

        lastPos = transform.position;


        animator.SetFloat("Speed", deltaPos.sqrMagnitude / Time.deltaTime);
    }

    void UpdateAnimator()
    {
        timer += Time.deltaTime;

        if (timer >= frameTime)
        {
            timer -= frameTime;
            currentAnimatorTime += frameTime;

            // Advance animator by one "stepped" frame
            animator.Update(frameTime);
        }
    }


    void FlipToVelocity()
    {

        float orientation = previousOrientation;
        if (deltaPos.x != 0)
            orientation = Mathf.Sign(deltaPos.x);

        transform.localScale = new Vector3(defaultScale.x * orientation, defaultScale.y, defaultScale.z);

        previousOrientation = orientation;
    }

    void Billboard()
    {
        var camPos = Camera.main.transform.position;

        var billboardForward = camPos - transform.position;
        var originalForward = transform.forward;
        transform.forward = new Vector3(originalForward.x, billboardForward.y, billboardForward.z);
    }
}
