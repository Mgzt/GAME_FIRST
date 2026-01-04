using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FadeUI : MonoBehaviour
{
    public static FadeUI Instance;

    public Image fadeImage;
    public float fadeDuration = 0.5f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void FadeOut(Action onComplete = null)
    {
        StartCoroutine(Fade(1f, onComplete));
    }

    public void FadeIn(Action onComplete = null)
    {
        StartCoroutine(Fade(0f, onComplete));
    }

    IEnumerator Fade(float targetAlpha, Action onComplete)
    {
        float startAlpha = fadeImage.color.a;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, targetAlpha);
        onComplete?.Invoke();
    }
}
