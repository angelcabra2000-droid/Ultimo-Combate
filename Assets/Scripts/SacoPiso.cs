using UnityEngine;

public class SacoPiso : MonoBehaviour
{
    [Header("Péndulo Invertido")]
    public Transform pivote;
    public float amortiguacion = 3f;
    public float fuerzaRestauracion = 25f;
    public float masaSaco = 12f;
    public float inclinacionMaxima = 30f;

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

        // Posición de reposo es arriba del pivote
        Vector3 posReposo = pivote.position + Vector3.up * longitudFija;
        Vector3 fuerzaRestauradora = (posReposo - transform.position) * fuerzaRestauracion;
        velocidad += fuerzaRestauradora * Time.fixedDeltaTime;

        // Amortiguación
        velocidad -= velocidad * amortiguacion * Time.fixedDeltaTime;

        // Mover
        transform.position += velocidad * Time.fixedDeltaTime;

        // Restricción de longitud fija al pivote
        Vector3 dir = (transform.position - pivote.position).normalized;
        transform.position = pivote.position + dir * longitudFija;

        // Limitar inclinación máxima
        float angulo = Vector3.Angle(Vector3.up, dir);
        if (angulo > inclinacionMaxima)
        {
            Vector3 dirLimitada = Vector3.RotateTowards(Vector3.up, dir, inclinacionMaxima * Mathf.Deg2Rad, 0f);
            transform.position = pivote.position + dirLimitada * longitudFija;
            velocidad *= 0.3f;
        }

        // Orientar saco vertical desde la base
        transform.rotation = Quaternion.FromToRotation(Vector3.down, -dir);
    }

    public void RecibirGolpe(Vector3 direccion, float fuerza)
    {
        direccion.y = 0f;
        direccion.Normalize();

        if (Vector3.Dot(velocidad, direccion) < 0)
            velocidad = Vector3.zero;

        velocidad += direccion * (fuerza / masaSaco);
    }
}