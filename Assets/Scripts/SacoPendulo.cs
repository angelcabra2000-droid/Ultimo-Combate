using UnityEngine;

public class SacoPendulo : MonoBehaviour
{
    [Header("Péndulo")]
    public Transform pivote;
    public float amortiguacion = 2.5f;
    public float fuerzaRestauracion = 20f;
    public float masaSaco = 8f;

    private Vector3 velocidad = Vector3.zero;
    private float longitudFija;

    void Start()
    {
        if (pivote != null)
            longitudFija = Vector3.Distance(transform.position, pivote.position);
    }

    void FixedUpdate()
    {
        if (pivote == null) return;

        // Fuerza restauradora hacia posición de reposo (abajo del pivote)
        Vector3 posReposo = pivote.position + Vector3.down * longitudFija;
        Vector3 fuerzaRestauradora = (posReposo - transform.position) * fuerzaRestauracion;
        velocidad += fuerzaRestauradora * Time.fixedDeltaTime;

        // Amortiguación
        velocidad -= velocidad * amortiguacion * Time.fixedDeltaTime;

        // Mover
        transform.position += velocidad * Time.fixedDeltaTime;

        // Restricción de longitud fija al pivote
        Vector3 dir = (transform.position - pivote.position).normalized;
        transform.position = pivote.position + dir * longitudFija;

        // Orientar saco
        transform.rotation = Quaternion.FromToRotation(Vector3.up, -dir);
    }

    public void RecibirGolpe(Vector3 direccion, float fuerza)
    {
        if (Vector3.Dot(velocidad, direccion) < 0)
            velocidad = Vector3.zero;

        velocidad += direccion * (fuerza / masaSaco);
    }
}