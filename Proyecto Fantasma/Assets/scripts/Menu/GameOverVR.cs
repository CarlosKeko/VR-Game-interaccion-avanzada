using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameOverVR : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup canvasGroup;          // CanvasGroup del DeathCanvas
    public Image bloodImage;                // UI Image de sangre
    public TMP_Text deathText;              // Texto "HAS MUERTO"
    public GameObject buttonsPanel;         // Panel con botones

    [Header("Fade")]
    public float fadeTime = 0.6f;
    public float bloodMaxAlpha = 0.85f;

    [Header("Escenas")]
    public string menuSceneName = "MainMenu";

    [Header("Desactivar al morir")]
    public Behaviour[] componentsToDisable; // <-- arrastra aquí scripts/providers
    public GameObject[] gameObjectsToDisable; // <-- opcional (spawners, enemigos, etc.)

    [Header("Opcional")]
    public bool pauseTimeScale = true;      // si lo desactivas, no hace Time.timeScale = 0

    private bool muerto;

    void Awake()
    {
        // Estado inicial UI
        if (canvasGroup) canvasGroup.alpha = 0f;

        if (bloodImage)
        {
            var c = bloodImage.color;
            c.a = 0f;
            bloodImage.color = c;
        }

        if (deathText) deathText.text = "";
        if (buttonsPanel) buttonsPanel.SetActive(false);
    }

    public void Morir()
    {
        if (muerto) return;
        muerto = true;

        // Desactivar gameplay
        DisableStuff();

        // Pausar tiempo (UI sigue si usamos unscaledDeltaTime)
        if (pauseTimeScale)
            Time.timeScale = 0f;

        StartCoroutine(MostrarPantallaMuerte());
    }

    void DisableStuff()
    {
        if (componentsToDisable != null)
        {
            foreach (var b in componentsToDisable)
                if (b) b.enabled = false;
        }

        if (gameObjectsToDisable != null)
        {
            foreach (var go in gameObjectsToDisable)
                if (go) go.SetActive(false);
        }
    }

    IEnumerator MostrarPantallaMuerte()
    {
        // Mostrar canvas
        if (canvasGroup) canvasGroup.alpha = 1f;

        // Fade de sangre (tiempo real)
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(0f, bloodMaxAlpha, t / fadeTime);

            if (bloodImage)
            {
                var c = bloodImage.color;
                c.a = a;
                bloodImage.color = c;
            }

            yield return null;
        }

        if (deathText) deathText.text = "HAS MUERTO";
        if (buttonsPanel) buttonsPanel.SetActive(true);
    }

    // Botón: Reiniciar
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Botón: Menú
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
