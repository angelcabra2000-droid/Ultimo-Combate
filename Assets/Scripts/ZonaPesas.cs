using UnityEngine;

public class ZonaPesas : MonoBehaviour
{
    [Header("Pesas de esta estacion")]
    public GameObject pesaIzquierda;  // Pesas_1 hermano
    public GameObject pesaDerecha;    // Pesas_2 hermano

    [Header("UI opcional")]
    public GameObject promptUI;

    private WeightLifter weightLifter;
    private bool jugadorCerca = false;

    void Start()
    {
        // Busca el WeightLifter en la escena automaticamente
        weightLifter = FindAnyObjectByType<WeightLifter>();

        // Pasa las pesas de ESTA estacion al WeightLifter cuando el jugador entra
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.R))
        {
            if (!weightLifter.isLifting)
                weightLifter.IniciarRutina();
        }

        if (promptUI != null)
            promptUI.SetActive(jugadorCerca && !weightLifter.isLifting);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorCerca = true;

        // Usa el método en lugar de asignar directo
        weightLifter.AsignarPesas(pesaIzquierda, pesaDerecha);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorCerca = false;
    }
}