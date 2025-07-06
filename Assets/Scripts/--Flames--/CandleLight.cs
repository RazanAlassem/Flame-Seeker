using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CandleLight : MonoBehaviour
{
    [SerializeField] private Light2D candleLight;
    [SerializeField] private float lightDuration = 10f;       // المدة قبل انطفاء الشمعة
    [SerializeField] private float flickerSpeed = 0.2f;        // سرعة الوميض

    private float timer;
    private bool candleWasLit = true; // لتجنب تكرار الانطفاء

    void Start()
    {
        timer = lightDuration;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            // تأثير وميض خفيف للشمعة
            float intensity = Mathf.Lerp(0.8f, 1.2f, Mathf.PingPong(Time.time * flickerSpeed, 1));
            candleLight.intensity = intensity;
        }
        else
        {
            candleLight.intensity = 0f;

            if (candleWasLit)
            {
                candleWasLit = false;

                // نبلغ اللاعب أن الشعلة انطفأت
                PlayerScript player = FindObjectOfType<PlayerScript>();
                if (player != null)
                    player.OnCandleExtinguished();
            }
        }
    }

    // تعيد تشغيل الشعلة
    public void RefillCandle()
    {
        timer = lightDuration;
        candleWasLit = true;
    }

    // معرفة حالة الشعلة
    public bool IsCandleLit()
    {
        return timer > 0;
    }
}
