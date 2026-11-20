using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldToConfirm : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public float holdSeconds = 2f;
    public Image progressFill;           // tu HoldProgress
    public UnityEvent onConfirmed;

    bool holding;
    float t;

    void Update()
    {
        if (!holding) return;

        t += Time.deltaTime;
        float p = Mathf.Clamp01(t / holdSeconds);

        if (progressFill) progressFill.fillAmount = p;

        if (p >= 1f)
        {
            holding = false;
            t = 0f;
            if (progressFill) progressFill.fillAmount = 0f;
            onConfirmed?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        holding = true;
        t = 0f;
    }

    public void OnPointerUp(PointerEventData eventData) => CancelHold();
    public void OnPointerExit(PointerEventData eventData) => CancelHold();

    void CancelHold()
    {
        holding = false;
        t = 0f;
        if (progressFill) progressFill.fillAmount = 0f;
    }
}

