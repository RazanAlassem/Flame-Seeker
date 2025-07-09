using UnityEngine;
using UnityEngine.UI;

public class PlayerFlameTracker : MonoBehaviour
{
    [SerializeField] private Image[] flameImages; // صور الشعلة في الـ UI
    [SerializeField] private Animator[] flameAnimators; // لو فيها أنميشن
    private int flameCount = 0;

    public void CollectFlame()
    {
        if (flameCount < flameImages.Length)
        {
            // شغّل أنميشن أو غير الصورة
            flameAnimators[flameCount].Play("FlameOnUI", 0, 0); // يشغل الأنميشن من بدايته
            flameCount++;
        }
    }
}
