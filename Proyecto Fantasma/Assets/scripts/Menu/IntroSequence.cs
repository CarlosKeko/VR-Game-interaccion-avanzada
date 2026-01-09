using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroSequence : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup canvasGroup;   // en el Canvas del overlay
    public TMP_Text storyText;        // el TextMeshPro

    [Header("Escena a cargar")]
    public string escenaJuego = "Forest test"; // nombre exacto en Build Settings

    [Header("Fade")]
    public float fadeOutTime = 1.2f;
    public float fadeInTime = 0.8f;

    [Header("Historia")]
    [TextArea(2, 6)]
    public string[] lineas = new string[]
    {
        "La radio dejó de sonar.",
        "La niebla empezó a moverse entre los árboles...",
        "Y entonces los oíste acercarse."
    };

    public float esperaAntesTexto = 0.4f;
    public float letrasPorSegundo = 35f;
    public float pausaEntreLineas = 1.0f;

    bool running;

    void Awake()
    {
        if (canvasGroup) canvasGroup.alpha = 0f;
        if (storyText) storyText.text = "";
    }

    // Llama a esto desde el botón "Iniciar"
    public void PlayIntroAndStart()
    {
        if (running) return;
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        running = true;

        // Fade a negro
        yield return Fade(0f, 1f, fadeOutTime);

        // Texto (typewriter)
        if (storyText) storyText.text = "";
        yield return new WaitForSeconds(esperaAntesTexto);

        foreach (var linea in lineas)
        {
            yield return TypeLine(linea);
            yield return new WaitForSeconds(pausaEntreLineas);
            if (storyText) storyText.text = ""; // se borra cada linea
        }

        // 3) Cargar escena del juego (dejamos negro mientras carga)
        var op = SceneManager.LoadSceneAsync(escenaJuego);
        while (!op.isDone) yield return null;



        running = false;
    }

    IEnumerator Fade(float from, float to, float time)
    {
        if (!canvasGroup) yield break;

        float t = 0f;
        canvasGroup.alpha = from;

        while (t < time)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / time);
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    IEnumerator TypeLine(string line)
    {
        if (!storyText)
            yield break;

        storyText.text = "";
        float delay = 1f / Mathf.Max(1f, letrasPorSegundo);

        for (int i = 0; i < line.Length; i++)
        {
            storyText.text += line[i];
            yield return new WaitForSeconds(delay);
        }
    }
}
