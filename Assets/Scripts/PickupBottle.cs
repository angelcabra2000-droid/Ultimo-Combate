using UnityEngine;

public class PickupBottle : MonoBehaviour
{
    [Header("Configuracion")]
    public Transform handBone;
    public float pickupRange = 2f;
    public KeyCode interactKey = KeyCode.F;

    [Header("Offset de la botella en la mano")]
    public Vector3 handPositionOffset = new Vector3(-0.1f, -0.62f, 0.53f);
    public Vector3 handRotationOffset = new Vector3(-56.05f, 0f, -1.03f);

    [Header("Delays (ajusta segun tus clips)")]
    public float delayTriggerAlzar = 0.8f;  // cuando termina BajarAgua
    public float delayAttach = 1.2f;         // cuando la mano toca la botella
    public float delayDetach = 0.4f;         // cuando suelta en Dejar_agua

    [Header("Debug (solo en editor)")]
    public bool debugOffsetMode = false;     // activa para ajustar offset en Play Mode

    private GameObject bottle;
    private bool holding = false;
    private bool isPickingUp = false;

    // Guardamos el estado original completo de la botella
    private Transform originalParent;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Modo debug: ajusta el offset en tiempo real mientras el personaje sostiene la botella
        if (debugOffsetMode && holding && bottle != null)
        {
            bottle.transform.localPosition = handPositionOffset;
            bottle.transform.localRotation = Quaternion.Euler(handRotationOffset);
        }

        if (Input.GetKeyDown(interactKey))
        {
            if (!holding && !isPickingUp)
                TryPickup();
            else if (holding)
                Drop();
        }
    }

    void TryPickup()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);
        foreach (var hit in hits)
        {
            GameObject obj = hit.gameObject;

            // Busca el tag en el objeto o en su padre
            if (!obj.CompareTag("Bottle") && obj.transform.parent != null)
                obj = obj.transform.parent.gameObject;

            if (obj.CompareTag("Bottle"))
            {
                bottle = obj;

                // Guardamos el estado original COMPLETO
                originalParent = bottle.transform.parent;
                originalLocalPosition = bottle.transform.localPosition;
                originalLocalRotation = bottle.transform.localRotation;

                isPickingUp = true;

                animator.SetTrigger("BajarAgua");
                Invoke(nameof(TriggerAlzar), delayTriggerAlzar);
                Invoke(nameof(AttachToHand), delayAttach);
                return;
            }
        }
    }

    void TriggerAlzar()
    {
        animator.SetTrigger("Alzar_Tomar");
    }

    void AttachToHand()
    {
        if (bottle == null) return;
        if (handBone == null)
        {
            Debug.LogError("PickupBottle: handBone no asignado en el Inspector.");
            isPickingUp = false;
            return;
        }

        // Desactiva fisica antes de repadrentar
        Rigidbody rb = bottle.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        bottle.transform.SetParent(handBone, worldPositionStays: false);
        bottle.transform.localPosition = handPositionOffset;
        bottle.transform.localRotation = Quaternion.Euler(handRotationOffset);

        holding = true;
        isPickingUp = false;
    }

    void Drop()
    {
        animator.SetTrigger("Dejar_agua");
        Invoke(nameof(DetachBottle), delayDetach);
        holding = false;

        TrainingStats.Instance?.RecargarEnergia();
    }

    void DetachBottle()
    {
        if (bottle == null) return;

        Vector3 worldPos = bottle.transform.position;

        bottle.transform.SetParent(originalParent, worldPositionStays: false);

        // Mantener X y Z actuales, restaurar Y original
        bottle.transform.position = new Vector3(worldPos.x, originalLocalPosition.y, worldPos.z);
        bottle.transform.localRotation = originalLocalRotation;

        Rigidbody rb = bottle.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        bottle = null;
    }

}