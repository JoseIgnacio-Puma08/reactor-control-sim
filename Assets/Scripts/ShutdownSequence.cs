using UnityEngine;
using TMPro;

public class ShutdownSequence : MonoBehaviour
{
    public ReactorSystem system;
    public UIBinder ui;

    public TMP_Text timerText;
    public float totalTime = 30f;

    float remaining;
    bool running;
    int expectedStep = 1;

    public int errors;

    void Start()
    {
        ResetSequence();
    }

    void Update()
    {
        if (!running) return;

        remaining -= Time.deltaTime;
        if (timerText) timerText.text = $"Timer: {remaining:0.0}s";

        if (remaining <= 0f)
        {
            running = false;
            ui.Log("⏱ Tiempo agotado. Reinicia la secuencia.");
        }
    }

    public void StartSequence()
    {
        system.ResetFlags();
        ResetSequence();
        running = true;
        ui.Log("Inicio de apagado: completa 1→5 antes de 30s.");
    }

    void ResetSequence()
    {
        remaining = totalTime;
        expectedStep = 1;
        errors = 0;
        if (timerText) timerText.text = $"Timer: {remaining:0.0}s";
    }

    void ValidateStep(int step, System.Action onOk)
    {
        if (!running)
        {
            ui.Log("ℹ️ Presiona 'Iniciar secuencia' para comenzar.");
            return;
        }

        if (step != expectedStep)
        {
            errors++;
            ui.Log($"⚠️ Paso incorrecto. Se esperaba el paso {expectedStep}.");
            ui.Log("Sugerencia: sigue el orden 1→2→3→4→5.");
            return;
        }

        onOk.Invoke();
        expectedStep++;
    }

    public void Step1_StopTurbines()
    {
        ValidateStep(1, () => { system.turbinesStopped = true; ui.Log("✅ Turbinas detenidas."); });
    }

    public void Step2_Vent()
    {
        ValidateStep(2, () => { system.vented = true; ui.Log("✅ Contenido ventilado."); });
    }

    public void Step3_ExchangeWater()
    {
        ValidateStep(3, () => { system.waterExchanged = true; system.AddWater(); ui.Log("✅ Agua intercambiada (enfriando)."); });
    }

    public void Step4_ReactorOff()
    {
        ValidateStep(4, () => { system.reactorOff = true; system.ForceShutdown(); ui.Log("✅ Reactor apagado (Shutdown)."); });
    }

    public void Step5_EmergencyProtocol()
    {
        ValidateStep(5, () =>
        {
            system.ForceEmergencyStop();
            ui.Log("🛑 Protocolo final ejecutado. Sistema detenido con seguridad.");
            running = false;
        });
    }
}
