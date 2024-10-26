using UnityEngine;
using System.Collections;
using System;

public static class FalloffGenerator {

	public static float[,] GenerateFalloffMap(Vector2Int size, float fallOffStart, float fallOffEnd) {
		float[,] map = new float[size.x,size.y];

		for (int y = 0; y < size.y; y++)
		{
			for(int x = 0; x < size.x; x++)
			{
				Vector2 position = new Vector2(
					(float)x / size.x * 2 - 1,
					(float)y / size.y * 2 - 1
				);

				float t = Mathf.Max(Mathf.Abs(position.x), Mathf.Abs(position.y));
				if(t > fallOffStart)
				{
					map[x,y] = 1;
				}
				else if (t < fallOffEnd)
				{
					map[x, y] = 0;
				}
				else
				{
					map[x, y] = Mathf.SmoothStep(1, 0, Mathf.InverseLerp(fallOffStart, fallOffEnd, t));
				}
			}
		}

		return map;
	}
}
