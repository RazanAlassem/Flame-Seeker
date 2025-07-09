using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // مهم لتعامل مع TextMeshPro

public class SceneChangerWithUI : MonoBehaviour
{
    public string sceneToLoad = "GameScene";
    public KeyCode interactionKey = KeyCode.E;
    public TextMeshProUGUI interactionText; // اسحب العنصر من الـ Inspector هنا

    private bool isPlayerInRange = false;

    void Start()
    {
        if (interactionText != null)
            interactionText.enabled = false; // نأكد إنه مخفي في البداية
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (interactionText != null)
                interactionText.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            if (interactionText != null)
                interactionText.enabled = false;
        }
    }
}
