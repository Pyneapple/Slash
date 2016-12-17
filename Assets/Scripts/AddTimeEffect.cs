using UnityEngine;
using System.Collections;

public class AddTimeEffect : MonoBehaviour {
	public MeshRenderer text;
	public TextMesh textMes;
	public Color timeColorAdd;
	public Color timeColorDelete;
	void OnEnable()
	{
		text.sortingLayerName = "UI";
	}

	public void SetText(int time)
	{
		textMes.text = (time < 0) ? ("-" + Mathf.Abs (time).ToString ()) : (Mathf.Abs (time).ToString ());
		textMes.color = (time < 0) ? (timeColorDelete) : (timeColorAdd);
	}
}
