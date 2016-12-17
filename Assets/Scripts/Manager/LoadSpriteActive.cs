using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadSpriteActive : MonoBehaviour {

	public string path;
	public SpriteRenderer[] spriteRender;
	public Image[] ima;
	public bool useName = true;
	public GameObject parent;

	[ContextMenu("LoadParent")]
	public void LoadParent()
	{
		if(parent == null)
		{
			parent = gameObject;
		}
		spriteRender = parent.GetComponentsInChildren <SpriteRenderer> (true);
		ima = parent.GetComponentsInChildren <Image> (false);
	}

	[ContextMenu("FixName")]
	public void FixName()
	{
		foreach(SpriteRenderer ren in spriteRender)
		{
			if(ren.sprite != null)
			ren.gameObject.name = ren.sprite.name;
		}
		foreach(Image ren in ima)
		{
			if(ren.sprite != null)
				ren.gameObject.name = ren.sprite.name;
		}
	}

/*	[ContextMenu("Load")]
	public void Load()
	{
		Sprite[] sprite;
		if (spriteRender.Length > 0) {
			sprite = Resources.LoadAll<Sprite> (path);
			for (int i = 0; i < spriteRender.Length; i++) {
				for(int j = 0; j < sprite.Length; j++)
				{
					if(sprite[j].name == spriteRender[i].gameObject.name)
					{
						spriteRender [i].sprite = sprite[j];
					}
				}
			}
		}
		if (ima.Length > 0) {
			sprite = Resources.LoadAll<Sprite> (path);
			for (int i = 0; i < ima.Length; i++) {
				for(int j = 0; j < sprite.Length; j++)
				{
					if(sprite[j].name == ima[i].gameObject.name)
					{
						ima [i].sprite = sprite[j];
						//SetNativeSizeImage.SetSize (ima[i], TypeSetNativeSize.Width);
					}
				}
			}
		}
	}*/

/*	[ContextMenu("UnLoad")]
	public void UnLoad()
	{
		
		if(spriteRender.Length > 0)
		{
			for(int i = 0; i < spriteRender.Length; i ++)
			{
			spriteRender[i].sprite = null;
			}
		}
		if(ima.Length > 0)
		{
			for (int i = 0; i < ima.Length; i++) {
				ima[i].sprite = null;
			}
		}
	}*/

	void OnEnable()
	{
	//	Load ();
		//print ("loadOb");
	}

	void OnDisable()
	{
//		UnLoad ();
		//print ("unloadOb");
	}

	public void SetTimeScale1()
	{
		Time.timeScale = 1;
	}

}
