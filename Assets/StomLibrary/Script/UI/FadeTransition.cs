using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Stom.UI
{
    //==============================================================================================
    //======== Class make effect fade transition when load new scene
    //==============================================================================================

    /// <summary>
    /// Use:
    /// ====  FadeTransition.LoadLevel( 1.0f, Color.black);
    /// </summary>
    public class FadeTransition : MonoBehaviour
    {
        private static GameObject canvas;
        private GameObject overlay;

        private const string const_nameCanvas = "TransitionUI";

        /// <summary>
        /// Method public method call start transition
        /// </summary>
        public static void LoadLevel(float duration, Color color)
        {
            var fade = new GameObject("Transition");
            fade.AddComponent<FadeTransition>();
            fade.GetComponent<FadeTransition>().StartFade(duration, color);
            fade.transform.SetParent(canvas.transform, false);
            fade.transform.SetAsLastSibling();
        }

        private void Awake()
        {
            FadeTransition.canvas = new GameObject(const_nameCanvas);
            var canvas = FadeTransition.canvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            DontDestroyOnLoad(FadeTransition.canvas);
        }

        private void StartFade(float duration, Color fadeColor)
        {
            StartCoroutine(RunFade(duration, fadeColor));
        }

        private IEnumerator RunFade(float duration, Color fadeColor)
        {
            var bgTex = new Texture2D(1, 1);
            bgTex.SetPixel(0, 0, fadeColor);
            bgTex.Apply();

            overlay = new GameObject();
            var image = overlay.AddComponent<Image>();
            var rect = new Rect(0, 0, bgTex.width, bgTex.height);
            var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
            image.material.mainTexture = bgTex;
            image.sprite = sprite;
            var newColor = image.color;
            image.color = newColor;
            image.canvasRenderer.SetAlpha(0.0f);

            overlay.transform.localScale = new Vector3(1, 1, 1);
            overlay.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
            overlay.transform.SetParent(canvas.transform, false);
            overlay.transform.SetAsFirstSibling();

            var time = 0.0f;
            var halfDuration = duration / 2.0f;
            while (time < halfDuration)
            {
                time += Time.deltaTime;
                image.canvasRenderer.SetAlpha(Mathf.InverseLerp(0, 1, time / halfDuration));
                yield return new WaitForEndOfFrame();
            }

            image.canvasRenderer.SetAlpha(1.0f);
            yield return new WaitForEndOfFrame();

            time = 0.0f;
            while (time < halfDuration)
            {
                time += Time.deltaTime;
                image.canvasRenderer.SetAlpha(Mathf.InverseLerp(1, 0, time / halfDuration));
                yield return new WaitForEndOfFrame();
            }

            image.canvasRenderer.SetAlpha(0.0f);
            yield return new WaitForEndOfFrame();

            Destroy(canvas);
        }
    }
}
