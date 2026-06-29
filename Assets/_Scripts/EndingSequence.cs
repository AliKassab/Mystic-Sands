using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Plays a full-screen end-of-game message, then returns to the Main Menu.
/// The screen is built entirely in code, so no prefab/scene wiring is required -
/// call EndingSequence.Play("...").
/// </summary>
public class EndingSequence : MonoBehaviour
{
    private static bool _running;

    public static void Play(string message, float displaySeconds = 4f)
    {
        if (_running) return;            // guard against per-frame re-triggering
        _running = true;

        var host = new GameObject("EndingSequence");
        host.AddComponent<EndingSequence>().StartCoroutine(Routine(host, message, displaySeconds));
    }

    private static IEnumerator Routine(GameObject host, string message, float seconds)
    {
        BuildScreen(host.transform, message);

        // Free the cursor for the menu and freeze gameplay behind the screen.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(seconds); // realtime: survives timeScale 0

        Time.timeScale = 1f;
        _running = false;
        SceneManager.LoadScene(0); // Main Menu (build index 0)
    }

    private static void BuildScreen(Transform parent, string message)
    {
        var canvasGO = new GameObject("EndingCanvas");
        canvasGO.transform.SetParent(parent);
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32760;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        // Full-screen background
        var bg = new GameObject("BG", typeof(RectTransform), typeof(Image));
        bg.transform.SetParent(canvasGO.transform, false);
        bg.GetComponent<Image>().color = new Color(0.03f, 0.03f, 0.05f, 1f);
        Stretch(bg.GetComponent<RectTransform>());

        // Centered message
        var textGO = new GameObject("Message", typeof(RectTransform), typeof(Text));
        textGO.transform.SetParent(canvasGO.transform, false);
        var text = textGO.GetComponent<Text>();
        text.text = message;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 64;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = new Color(0.95f, 0.9f, 0.7f);
        Stretch(text.rectTransform);
    }

    private static void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
