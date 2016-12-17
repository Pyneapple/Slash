using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Stom.UI
{
    //===========================================================================================
    //===========This class is responsible for popup management. Popups follow the traditional behavior of
    //===========automatically blocking the input on elements behind it and adding a background texture.
    //===========================================================================================
    [RequireComponent(typeof(Animator), typeof(CanvasGroup))]
    public class Popup : MonoBehaviour
    {

        public Color panelColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);
        public float speedCrossFade = 0.5f;

        private GameObject panel;
        private Image image;
        private Canvas canvas;

        void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();
        }

        void OnEnable()
        {
            Open();
        }

        /// <summary>
        /// Method open popup
        /// </summary>
        public void Open()
        {
            AddPanel();
        }

        /// <summary>
        /// method close popup
        /// </summary>
        public void Close()
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                animator.Play("Close");

            RemoveBackground();
            StartCoroutine(CoroutineClosePopUp());
        }

        private IEnumerator CoroutineClosePopUp()
        {
            // Cross faded panel popup when it close
            panel.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.2f, true);
            yield return new WaitForSeconds(0.2f);
            // Deactive popup
            this.gameObject.SetActive(false);
            // Deactive background popup
            panel.SetActive(false);
        }

        /// <summary>
        /// Auto add panel and block input behind panel
        /// </summary>
        private void AddPanel()
        {
            if (panel == null)
            {
                // Creature texture
                var bgTex = new Texture2D(1, 1);
                bgTex.SetPixel(0, 0, panelColor);
                bgTex.Apply();

                // Creature Popup Panel
                panel = new GameObject("Panel_" + this.gameObject.name);
                image = panel.AddComponent<Image>();
                Rect rect = new Rect(0, 0, bgTex.width, bgTex.height);
                Sprite sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
                image.material.mainTexture = bgTex;
                image.sprite = sprite;

                // Set popup panel fix screen size
                panel.transform.localScale = new Vector3(1, 1, 1);
                // Make up fill screen base on canvas rect
                panel.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
                panel.transform.SetParent(transform.parent, false);
                panel.transform.SetSiblingIndex(transform.GetSiblingIndex());
            }
            else panel.SetActive(true);

            // Set alpha color for image texture
            image.canvasRenderer.SetAlpha(0.0f);
            // Ignore time scale
            image.CrossFadeAlpha(1.0f, speedCrossFade, true);
            // Don't ignore time scale
            //image.CrossFadeAlpha(1.0f, speedCrossFade, false);    
        }

        /// <summary>
        /// Make cross fade panel popup disappear
        /// </summary>
        private void RemoveBackground()
        {
            var image = panel.GetComponent<Image>();
            if (image != null)
                image.CrossFadeAlpha(0.0f, 0.2f, false);
        }
    }
}
