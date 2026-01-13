using UnityEngine;
using TMPro;

public class ShowDisplayFps : MonoBehaviour
{
    public TextMeshProUGUI enemiesText;
    public GameManager gameManager;

    public TextMeshProUGUI fpsText;
    private float deltaTime = 0.0f;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 1000;
    }


    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";

        enemiesText.text = $"{gameManager.EnemiesCount} enemies";
    }
}