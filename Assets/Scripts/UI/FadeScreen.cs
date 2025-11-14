using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen Instance;

    [SerializeField] Image fadeImage;
    [SerializeField] float defaultFadeTime = 2f;

    public float FadeTime { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FadeToClear();
    }

    public void FadeToBlack(float fadeTime = 0f)
    {
        StartCoroutine(FadeRoutine(fadeImage.color.a, 1f, fadeTime));
    }

    public void FadeToClear(float fadeTime = 0f)
    {
        StartCoroutine(FadeRoutine(fadeImage.color.a, 0f, fadeTime));
    }

    IEnumerator FadeRoutine(float startValue, float targetValue, float fadeTime = 0f)
    {
        float timePassed = 0f;

        if (fadeTime <= 0f) fadeTime = defaultFadeTime;

        FadeTime = fadeTime;

        while (timePassed < fadeTime)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / fadeTime;
            float newAlpha = Mathf.Lerp(startValue, targetValue, linearT);

            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
            yield return null;
        }
    }
}
