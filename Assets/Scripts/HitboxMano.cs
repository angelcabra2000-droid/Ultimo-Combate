using UnityEngine;

public class HitboxMano : MonoBehaviour
{
    private SphereCollider col;
    private bool golpeActivo = false;

    void Start()
    {
        col = GetComponent<SphereCollider>();
        col.enabled = false;
    }

    public void ActivarGolpe(float duracion = 0.2f)
    {
        CancelInvoke(nameof(DesactivarGolpe));
        golpeActivo = true;
        col.enabled = true;
        Invoke(nameof(DesactivarGolpe), duracion);
    }

    private void DesactivarGolpe()
    {
        col.enabled = false;
        golpeActivo = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        AplicarGolpe(other);
    }

    private void OnTriggerStay(Collider other)
    {
        AplicarGolpe(other);
    }

    private void AplicarGolpe(Collider other)
    {
        if (!golpeActivo) return;

        if (other.CompareTag("Saco"))
        {
            Vector3 direccion = (other.transform.position - transform.position).normalized;

            SacoPendulo sacoPendulo = other.GetComponentInParent<SacoPendulo>();
            if (sacoPendulo != null)
            {
                direccion.y = 0.1f;
                direccion.Normalize();
                sacoPendulo.RecibirGolpe(direccion, 80f);
                return;
            }

            SacoPiso sacoPiso = other.GetComponentInParent<SacoPiso>();
            if (sacoPiso != null)
            {
                sacoPiso.RecibirGolpe(direccion, 60f);
            }

            TrainingStats.Instance?.GolpeSaco();
        }
    }
}