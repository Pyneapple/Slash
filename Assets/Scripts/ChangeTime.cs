using UnityEngine;
using System.Collections;

public class ChangeTime {
	
	public static int GetSecond(Vector2 minute)
	{
		return (int)(minute.x * 60 + minute.y);
	}

	public static Vector2 GetMinute(int second)
	{
		return new Vector2 (second / 60, second % 60);
	}

}
