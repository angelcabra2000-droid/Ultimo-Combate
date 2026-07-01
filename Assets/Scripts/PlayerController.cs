using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 3f;
    public float guardWalkSpeed = 2f;

    [Header("Hitboxes")]
    public HitboxMano hitboxIzq;
    public HitboxMano hitboxDer;

    [Header("Pesas")]
    public WeightLifter weightLifter;

    // Estado global accesible por otros scripts
    public static bool IsGuard = false;
    public static Vector3 LastMoveDirection = Vector3.forward;

    private Animator animator;
    private CharacterController characterController;
    private bool isGuard = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleGuard();
        HandleMovement();
        HandleCombat();
    }

    // ─────────────────────────────────────────
    //  GUARDIA
    // ─────────────────────────────────────────
    void HandleGuard()
    {
        // No puede entrar/salir de guardia mientras levanta pesas
        if (weightLifter != null && weightLifter.isLifting) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            isGuard = !isGuard;
            IsGuard = isGuard;
            animator.SetBool("isGuard", isGuard);
        }
    }

    // ─────────────────────────────────────────
    //  MOVIMIENTO
    // ─────────────────────────────────────────
    void HandleMovement()
    {
        // Bloqueado mientras levanta pesas
        if (weightLifter != null && weightLifter.isLifting)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingGuard", false);
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (isGuard)
        {
            bool isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingGuard", isMoving);

            if (isMoving)
            {
                Vector3 cameraForward = Camera.main.transform.forward;
                Vector3 cameraRight = Camera.main.transform.right;
                cameraForward.y = 0;
                cameraRight.y = 0;
                cameraForward.Normalize();
                cameraRight.Normalize();

                Vector3 moveDir = Vector3.zero;

                if (v > 0.1f)
                {
                    animator.SetInteger("walkDir", 0); // Walk_Front
                    moveDir = cameraForward;
                }
                else if (v < -0.1f)
                {
                    animator.SetInteger("walkDir", 1); // Walk_Back
                    moveDir = -cameraForward;
                }
                else if (h < -0.1f)
                {
                    animator.SetInteger("walkDir", 2); // Walk_Left
                    moveDir = -cameraRight;
                }
                else if (h > 0.1f)
                {
                    animator.SetInteger("walkDir", 3); // Walk_Right
                    moveDir = cameraRight;
                }

                characterController.Move(moveDir.normalized * guardWalkSpeed * TrainingStats.SpeedMultiplier * Time.deltaTime);

            }
        }
        else
        {
            Vector3 move = new Vector3(h, 0, v);
            animator.SetBool("isWalkingGuard", false);
            animator.SetBool("isWalking", move.magnitude > 0.1f);

            if (move.magnitude > 0.1f)
            {
                Vector3 cameraForward = Camera.main.transform.forward;
                Vector3 cameraRight = Camera.main.transform.right;
                cameraForward.y = 0;
                cameraRight.y = 0;
                cameraForward.Normalize();
                cameraRight.Normalize();

                Vector3 moveDir = (cameraForward * v + cameraRight * h).normalized;
                LastMoveDirection = moveDir;

                float angle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(90, angle, -180);

                characterController.Move(moveDir * walkSpeed * TrainingStats.SpeedMultiplier * Time.deltaTime);
            }
        }
    }

    // ─────────────────────────────────────────
    //  COMBATE
    // ─────────────────────────────────────────
    void HandleCombat()
    {
        if (!isGuard) return;
        if (weightLifter != null && weightLifter.isLifting) return;

        bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (shift)
        {
            if (Input.GetKeyDown(KeyCode.E)) { animator.SetInteger("esquivaDir", 0); animator.SetTrigger("Esquivar"); TrainingStats.Instance?.GolpeAire(); }
            if (Input.GetKeyDown(KeyCode.Q)) { animator.SetInteger("esquivaDir", 1); animator.SetTrigger("Esquivar"); TrainingStats.Instance?.GolpeAire(); }
            if (Input.GetKeyDown(KeyCode.C)) { animator.SetInteger("esquivaDir", 2); animator.SetTrigger("Esquivar"); TrainingStats.Instance?.GolpeAire(); }
            if (Input.GetKeyDown(KeyCode.X)) { animator.SetInteger("esquivaDir", 3); animator.SetTrigger("Esquivar"); TrainingStats.Instance?.GolpeAire(); }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E)) { animator.SetTrigger("Jab_Der"); StartCoroutine(GolpeConDelay(hitboxDer, 0f, 0.2f)); TrainingStats.Instance?.GolpeAire(); }
            if (Input.GetKeyDown(KeyCode.Q)) { animator.SetTrigger("Jab_Izq"); StartCoroutine(GolpeConDelay(hitboxIzq, 0f, 0.2f)); TrainingStats.Instance?.GolpeAire(); }
            if (Input.GetKeyDown(KeyCode.C)) { animator.SetTrigger("Gancho_Der"); StartCoroutine(GolpeConDelay(hitboxDer, 0.15f, 0.35f)); TrainingStats.Instance?.GolpeAire(); }
            if (Input.GetKeyDown(KeyCode.X)) { animator.SetTrigger("Gancho_Izq"); StartCoroutine(GolpeConDelay(hitboxIzq, 0.15f, 0.35f)); TrainingStats.Instance?.GolpeAire(); }
        }
    }

    private System.Collections.IEnumerator GolpeConDelay(HitboxMano hitbox, float delay, float duracion)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);
        hitbox.ActivarGolpe(duracion);
    }
}