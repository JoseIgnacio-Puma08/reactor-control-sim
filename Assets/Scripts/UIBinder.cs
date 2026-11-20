using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBinder : MonoBehaviour
{
    public ReactorSystem system;

    [Header("Monitoring UI")]
    public Slider waterSlider;
    public TMP_Text tempText;
    public TMP_Text energyText;
    public Image statusBadge;

    [Header("Shutdown UI")]
    public TMP_Text eventLog;

    public Color okColor = new Color(0.2f, 0.8f, 0.3f);
    public Color warnColor = new Color(0.95f, 0.8f, 0.2f);
    public Color dangerColor = new Color(0.95f, 0.25f, 0.25f);

    public void Log(string msg)
    {
        if (!eventLog) return;
        eventLog.text = $"• {msg}\n" + eventLog.text;
    }

    void Update()
    {
        if (!system) return;

        if (waterSlider) waterSlider.value = system.waterLevel;
        if (tempText) tempText.text = $"{system.waterTempC:0} °C";
        if (energyText) energyText.text = $"{system.energyMWe:0} MWe";

        if (statusBadge)
        {
            if (system.waterTempC > 400f)
            {
                statusBadge.color = dangerColor;
                // Para no spamear, solo loguea si quieres (opcional)
            }
            else if (system.waterTempC > 360f)
                statusBadge.color = warnColor;
            else
                statusBadge.color = okColor;
        }
    }
}
