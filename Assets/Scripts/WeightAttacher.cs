using UnityEngine;

public class WeightLifter : MonoBehaviour
{
    [Header("Pesas")]
    [HideInInspector] public GameObject pesaIzquierda;
    [HideInInspector] public GameObject pesaDerecha;

    [Header("Huesos de las manos")]
    public Transform huesoDerecho;
    public Transform huesoIzquierdo;

    [Header("Offset de pesas en la mano (ajusta en Play Mode)")]
    public Vector3 offsetPosDer = Vector3.zero;
    public Vector3 offsetRotDer = Vector3.zero;
    public Vector3 offsetPosIzq = Vector3.zero;
    public Vector3 offsetRotIzq = Vector3.zero;

    [Header("Delays (ajusta segun tus clips)")]
    public float delayAgarrar = 0.5f;   // cuando la mano toca la pesa
    public float delaySoltar = 1.8f;   // cuando la deja de vuelta en la mesa
    public float delayFin = 2.2f;   // cuando termina toda la animacion

    [Header("Debug")]
    public bool debugOffsetMode = false;

    // Estado original de las pesas
    private Transform parentInicialIzq, parentInicialDer;
    private Vector3 posInicialIzq, posInicialDer;
    private Quaternion rotInicialIzq, rotInicialDer;

    private Animator animator;
    [HideInInspector] public bool isLifting = false;

    [Header("Test de posicion (solo editor)")]
    public bool testPosicionMano = false;


    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        // Guardar estado inicial
        parentInicialIzq = pesaIzquierda.transform.parent;
        parentInicialDer = pesaDerecha.transform.parent;
        posInicialIzq = pesaIzquierda.transform.position;
        posInicialDer = pesaDerecha.transform.position;
        rotInicialIzq = pesaIzquierda.transform.rotation;
        rotInicialDer = pesaDerecha.transform.rotation;
    }

    void Update()
    {
        // Modo ajuste: pega las pesas a la mano sin necesitar animacion
        if (testPosicionMano)
        {
            pesaDerecha.transform.SetParent(huesoDerecho, false);
            pesaDerecha.transform.localPosition = offsetPosDer;
            pesaDerecha.transform.localRotation = Quaternion.Euler(offsetRotDer);

            pesaIzquierda.transform.SetParent(huesoIzquierdo, false);
            pesaIzquierda.transform.localPosition = offsetPosIzq;
            pesaIzquierda.transform.localRotation = Quaternion.Euler(offsetRotIzq);
        }

        // Debug offset mode: ajusta en tiempo real mientras la animacion corre
        if (debugOffsetMode && isLifting)
        {
            pesaDerecha.transform.localPosition = offsetPosDer;
            pesaDerecha.transform.localRotation = Quaternion.Euler(offsetRotDer);
            pesaIzquierda.transform.localPosition = offsetPosIzq;
            pesaIzquierda.transform.localRotation = Quaternion.Euler(offsetRotIzq);
        }
    }

    public void IniciarRutina()
    {
        isLifting = true;
        animator.SetBool("Pesas", true);   // ← activa la transicion
        Invoke(nameof(AgarrarPesas), delayAgarrar);
        Invoke(nameof(SoltarPesas), delaySoltar);
        Invoke(nameof(TerminarRutina), delayFin);
    }

    void AgarrarPesas()
    {
        pesaDerecha.transform.SetParent(huesoDerecho, false);
        pesaDerecha.transform.localPosition = Vector3.zero;
        pesaDerecha.transform.localRotation = Quaternion.identity;

        pesaIzquierda.transform.SetParent(huesoIzquierdo, false);
        pesaIzquierda.transform.localPosition = Vector3.zero;
        pesaIzquierda.transform.localRotation = Quaternion.identity;
    }

    void SoltarPesas()
    {
        pesaDerecha.transform.SetParent(parentInicialDer);
        pesaDerecha.transform.position = posInicialDer;
        pesaDerecha.transform.rotation = rotInicialDer;

        pesaIzquierda.transform.SetParent(parentInicialIzq);
        pesaIzquierda.transform.position = posInicialIzq;
        pesaIzquierda.transform.rotation = rotInicialIzq;
    }

    void TerminarRutina()
    {
        animator.SetBool("Pesas", false);  // ← vuelve a Idle
        isLifting = false;
        TrainingStats.Instance?.RutinasPesas();
    }

    public void AsignarPesas(GameObject izq, GameObject der)
    {
        pesaIzquierda = izq;
        pesaDerecha = der;

        // Guarda la posicion inicial de ESTAS pesas
        posInicialIzq = pesaIzquierda.transform.position;
        posInicialDer = pesaDerecha.transform.position;
        rotInicialIzq = pesaIzquierda.transform.rotation;
        rotInicialDer = pesaDerecha.transform.rotation;
    }
}