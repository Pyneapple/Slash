using UnityEngine;
using System.Collections;

public class ScaleAutoCamera : MonoBehaviour{
	public enum TypeSetSpriteSize
	{
		Height,
		Width,
		LimitSize,
		FullSize
	}
	public TypeSetSpriteSize typeScale = TypeSetSpriteSize.FullSize;

	void OnEnable()
	{
		SetSize (GetComponent <SpriteRenderer> (), typeScale);
	}

	public static void SetSize(SpriteRenderer spriteRender, TypeSetSpriteSize sizeType, float deltaEdg = 0)
	{
		Sprite sprit = spriteRender.sprite;
		float scaleSpriteX = ((float)Camera.main.orthographicSize * 2 * ((float)Screen.width)/(float)Screen.height) / (float)sprit.bounds.size.x + deltaEdg;
		float scaleSpriteY = ((float)Camera.main.orthographicSize * 2) / (float)sprit.bounds.size.y + deltaEdg;
		if (sizeType == TypeSetSpriteSize.Height) {
			
			spriteRender.transform.localScale = new Vector3 (scaleSpriteY, scaleSpriteY, spriteRender.transform.localScale.z);
		} else if (sizeType == TypeSetSpriteSize.Width) {
			
			spriteRender.transform.localScale = new Vector3 (scaleSpriteX, scaleSpriteX, spriteRender.transform.localScale.z);
		} else if (sizeType == TypeSetSpriteSize.LimitSize) {
			if ((float)sprit.bounds.size.x / (float)sprit.bounds.size.y > ((float)Screen.width / (float)Screen.height)) {
				spriteRender.transform.localScale = new Vector3 (scaleSpriteY, scaleSpriteY, spriteRender.transform.localScale.z);
			} else {
				spriteRender.transform.localScale = new Vector3 (scaleSpriteX, scaleSpriteX, spriteRender.transform.localScale.z);
			}
		} else if (sizeType == TypeSetSpriteSize.FullSize) {
			spriteRender.transform.localScale = new Vector3 (scaleSpriteX, scaleSpriteY, spriteRender.transform.localScale.z);
		}
	}
}
