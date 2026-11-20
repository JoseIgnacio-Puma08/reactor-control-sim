using UnityEngine;

public enum PlantState { Running, Cooling, Shutdown, EmergencyStopped }

public class ReactorSystem : MonoBehaviour
{
    public PlantState state = PlantState.Running;

    [Range(0f, 1f)] public float waterLevel = 0.6f;
    public float waterTempC = 320f;
    public float energyMWe = 800f;

    public float heatUpRate = 8f;   // °C/s
    public float coolRate = 12f;    // °C per click
    public float leakHeatRate = 2f; // °C/s
    public float maxEnergyMWe = 1000f;

    // Flags de pasos
    public bool turbinesStopped;
    public bool vented;
    public bool waterExchanged;
    public bool reactorOff;

    void Update()
    {
        Simulate(Time.deltaTime);
    }

    void Simulate(float dt)
    {
        if (state == PlantState.EmergencyStopped)
        {
            energyMWe = Mathf.MoveTowards(energyMWe, 0f, 2000f * dt);
            waterTempC = Mathf.MoveTowards(waterTempC, 25f, 40f * dt);
            return;
        }

        if (state == PlantState.Shutdown)
        {
            energyMWe = Mathf.MoveTowards(energyMWe, 0f, 1000f * dt);
            waterTempC = Mathf.MoveTowards(waterTempC, 50f, leakHeatRate * dt);
            return;
        }

        float lowWaterFactor = Mathf.InverseLerp(1f, 0.2f, waterLevel);

        if (state == PlantState.Running)
            waterTempC += (heatUpRate * lowWaterFactor) * dt;
        else
            waterTempC -= leakHeatRate * dt;

        waterTempC = Mathf.Clamp(waterTempC, 0f, 1200f);

        if (turbinesStopped || reactorOff)
            energyMWe = Mathf.MoveTowards(energyMWe, 0f, 1200f * dt);
        else
        {
            float tempFactor = Mathf.InverseLerp(200f, 600f, waterTempC);
            energyMWe = Mathf.Lerp(200f, maxEnergyMWe, tempFactor);
        }
    }

    public void AddWater()
    {
        if (state == PlantState.Shutdown || state == PlantState.EmergencyStopped) return;
        waterLevel = Mathf.Clamp01(waterLevel + 0.08f);
        waterTempC = Mathf.Max(0f, waterTempC - coolRate);
        state = PlantState.Cooling;
    }

    public void VentWater()
    {
        if (state == PlantState.Shutdown || state == PlantState.EmergencyStopped) return;
        waterLevel = Mathf.Clamp01(waterLevel - 0.08f);
        waterTempC = Mathf.Max(0f, waterTempC - 2f);
    }

    public void ForceShutdown()
    {
        state = PlantState.Shutdown;
    }

    public void ForceEmergencyStop()
    {
        state = PlantState.EmergencyStopped;
    }

    public void ResetFlags()
    {
        turbinesStopped = vented = waterExchanged = reactorOff = false;
        state = PlantState.Running;
    }
}
