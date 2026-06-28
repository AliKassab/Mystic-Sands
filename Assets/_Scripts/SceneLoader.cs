using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Loads a scene asynchronously behind a code-built progress bar so the
/// transition feels responsive instead of a frozen frame. No scene wiring
/// required - call SceneLoader.Load(buildIndex).
/// </summary>
public class SceneLoader : MonoBehaviour
{
    private RectTransform fill;

    public static void Load(int buildIndex)
    {
        var go = new GameObject("SceneLoader");
        DontDestroyOnLoad(go);
        go.AddComponent<SceneLoader>().StartCoroutine_Load(buildIndex);
    }

    private void StartCoroutine_Load(int buildIndex) => StartCoroutine(LoadRoutine(buildIndex));

    private IEnumerator LoadRoutine(int buildIndex)
    {
        BuildUI();
        yield return null; // let the loading screen paint one frame first

        var op = SceneManager.LoadSceneAsync(buildIndex);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            // progress maxes at 0.9 until activation is allowed
            float p = Mathf.Clamp01(op.progress / 0.9f);
            SetFill(p);

            if (op.progress >= 0.9f)
            {
                SetFill(1f);
                yield return null; // show a full bar before swapping
                op.allowSceneActivation = true;
            }
            yield return null;
        }

        Destroy(gameObject);
    }

    private void SetFill(float p)
    {
        if (fill != null)
            fill.anchorMax = new Vector2(Mathf.Clamp01(p), 1f);
    }

    private void BuildUI()
    {
        var canvasGO = new GameObject("LoadingCanvas");
        canvasGO.transform.SetParent(transform);
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32760;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // full-screen background
        var bg = NewImage("BG", canvasGO.transform, new Color(0.03f, 0.03f, 0.05f, 1f));
        Stretch(bg);

        // bar track (centered)
        var track = NewImage("BarTrack", canvasGO.transform, new Color(1f, 1f, 1f, 0.12f));
        track.anchorMin = track.anchorMax = new Vector2(0.5f, 0.5f);
        track.pivot = new Vector2(0.5f, 0.5f);
        track.sizeDelta = new Vector2(640, 16);
        track.anchoredPosition = Vector2.zero;

        // fill (driven by anchorMax.x, so no sprite needed)
        fill = NewImage("BarFill", track, new Color(0.85f, 0.7f, 0.4f, 1f));
        fill.anchorMin = new Vector2(0f, 0f);
        fill.anchorMax = new Vector2(0f, 1f);
        fill.offsetMin = Vector2.zero;
        fill.offsetMax = Vector2.zero;
    }

    private static RectTransform NewImage(string name, Transform parent, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        go.GetComponent<Image>().color = color;
        return go.GetComponent<RectTransform>();
    }

    private static void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
