using UnityEngine;
using UnityEngine.UI;

public class TrainingStats : MonoBehaviour
{
    public static TrainingStats Instance;

    [Header("Energía")]
    public float maxEnergia = 100f;
    public float energiaActual = 100f;
    public float drainCaminar = 0.5f;    // era 2f
    public float drainGolpeSaco = 1f;    // era 3f
    public float drainGolpeAire = 0.3f;  // era 1f
    public float drainPesas = 8f;        // era 15f
    public float regenEnergia = 0.8f;    // era 1f       // por segundo en reposo

    [Header("Experiencia")]
    public float maxXP = 100f;
    public float xpActual = 0f;
    public float xpGolpeSaco = 5f;
    public float xpGolpeAire = 1f;
    public float xpPesas = 20f;

    [Header("UI")]
    public Slider sliderEnergia;
    public Slider sliderXP;

    [Header("Agua")]
    public float energiaRecarga = 25f;  // cuánta energía recupera tomar agua

    public void RecargarEnergia()
    {
        ModificarEnergia(energiaRecarga);
    }

    [Header("Fatiga")]
    public float umbralFatiga = 20f;        // % de energía donde empieza la fatiga
    public float multiplicadorFatiga = 0.5f; // qué tan lento camina (0.5 = mitad de velocidad)
    public static float SpeedMultiplier = 1f; // PlayerController lo lee

    private bool estabaCaminando = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        bool caminando = PlayerController.IsGuard ? IsWalkingGuard() : IsWalking();

        if (caminando)
        {
            ModificarEnergia(-drainCaminar * Time.deltaTime);
            estabaCaminando = true;
        }
        else
        {
            if (estabaCaminando) estabaCaminando = false;
            ModificarEnergia(regenEnergia * Time.deltaTime);
        }

        // Actualizar multiplicador de velocidad
        SpeedMultiplier = energiaActual < umbralFatiga ? multiplicadorFatiga : 1f;

        ActualizarUI();
    }

    bool IsWalking()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        return Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;
    }

    bool IsWalkingGuard()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        return Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;
    }

    public void GolpeSaco()
    {
        ModificarEnergia(-drainGolpeSaco);
        ModificarXP(xpGolpeSaco);
    }

    public void GolpeAire()
    {
        ModificarEnergia(-drainGolpeAire);
        ModificarXP(xpGolpeAire);
    }

    public void RutinasPesas()
    {
        ModificarEnergia(-drainPesas);
        ModificarXP(xpPesas);
    }

    void ModificarEnergia(float cantidad)
    {
        energiaActual = Mathf.Clamp(energiaActual + cantidad, 0, maxEnergia);
    }

    void ModificarXP(float cantidad)
    {
        xpActual = Mathf.Clamp(xpActual + cantidad, 0, maxXP);
    }

    void ActualizarUI()
    {
        if (sliderEnergia) sliderEnergia.value = energiaActual / maxEnergia;
        if (sliderXP) sliderXP.value = xpActual / maxXP;
    }


}