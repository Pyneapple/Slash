using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Stom.NativePlugin
{
    [System.Serializable]
    public class CrossAds
    {
        public string nameGame;
        public string imageHorizontal;
        public string imageVertical;
        public string linkAndroid;
        public string linkiOs;

        public CrossAds(string nameGame, string imageHorizontal, string imageVertical, string linkAndroid, string linkiOs)
        {
            this.nameGame = nameGame;
            this.imageHorizontal = imageHorizontal;
            this.imageVertical = imageVertical;
            this.linkAndroid = linkAndroid;
            this.linkiOs = linkiOs;
        }

        public CrossAds()
        {
            this.nameGame = null;
            this.imageHorizontal = null;
            this.imageVertical = null;
            this.linkAndroid = null;
            this.linkiOs = null;
        }
    }

    [System.Serializable]
    public class DataCrossAds
    {
        public List<CrossAds> elements = new List<CrossAds>();
    }

    [System.Serializable]
    public class AdsStom : MonoBehaviour
    {

        public enum TypeShow
        {
            NEWEST,
            RANDOM
        }

        class AdsElement
        {
            public string game_url;
            public string image_url;

            public AdsElement(string game_url, string image_url)
            {
                this.image_url = image_url;
                this.game_url = game_url;
            }
        }

        public string url = "https://www.dropbox.com/s/l3p9im01hnpka4l/AdsData.txt?dl=0";
        public Image images;
        public TypeShow typeShow;

        private DataCrossAds data;
        private Texture2D currentTexture;

        private string currentLinkGame;
        private string currentLinkImage;

        private bool isLoadData;        // Only return true when data loaded

        void Start()
        {
            StartCoroutine(LoadData());
        }

        IEnumerator LoadData()
        {
            WWW www = new WWW(url);
            yield return www;

            // Load succeed
            if (www.error == null)
            {
                string _textData = www.text;
                data = JsonUtility.FromJson<DataCrossAds>(_textData);

                isLoadData = true;

                // After load succed data, we will load random image
                yield return LoadImage(typeShow);
            }
        }

        /// <summary>
        /// Main method show ads
        /// </summary>
        private void ShowAds()
        {
            if (currentTexture != null)
            {
                var _button = images.GetComponent<AdsButtonImage>();
                if (!_button)
                    _button = images.gameObject.AddComponent<AdsButtonImage>();
                _button.urlImage = currentLinkGame;
                images.sprite = Sprite.Create(currentTexture, new Rect(0, 0, currentTexture.width, currentTexture.height), new Vector2(0.0f, 0.0f));
                //  images.SetNativeSize();
                StartCoroutine(LoadImage(typeShow));
            }
        }

        private IEnumerator LoadImage(TypeShow typeShow)
        {
            // If data not load, try load it again
            if (!isLoadData)
                yield return LoadData();
            // Load url base on type show

            CrossAds ads = null;
            switch (typeShow)
            {
                case TypeShow.NEWEST:
                    ads = data.elements[data.elements.Count - 1];
                    break;
                case TypeShow.RANDOM:
                    ads = data.elements[UnityEngine.Random.Range(0, data.elements.Count)];
                    break;
            }

            if (Application.platform == RuntimePlatform.Android)
                currentLinkGame = ads.linkAndroid;
            else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS)
                currentLinkGame = ads.linkiOs;
            else if (Application.platform == RuntimePlatform.WindowsEditor)
                currentLinkGame = ads.linkAndroid;

            if (Screen.width > Screen.height)
                currentLinkImage = ads.imageHorizontal;
            else
                currentLinkImage = ads.imageVertical;

            // If data loaded, start load image
            if (isLoadData)
            {
                WWW www = new WWW(currentLinkImage);
                yield return www;
                if (www.error == null)
                {
                    currentTexture = www.texture;

                    Resources.UnloadUnusedAssets();
                    // Clear GC from last image
                    System.GC.Collect();
                }
            }
        }

        //void Update()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        ShowAds();
        //    }
        //}
    }

    public class AdsButtonImage : MonoBehaviour, IPointerClickHandler
    {
        public string urlImage;

        public void OnPointerClick(PointerEventData eventData)
        {
            Application.OpenURL(urlImage);
        }
    }
}
