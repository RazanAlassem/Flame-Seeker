using UnityEngine;

public class FlameUI : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // استدعِ هذه الدالة عندما يأخذ اللاعب الشعلة
    public void LightTheFlame()
    {
        animator.SetTrigger("LightUp");
    }
}
