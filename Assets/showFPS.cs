using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class showFPS : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public float deltaTime;

	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;
		fpsText.text = Mathf.Ceil(fps).ToString();
	}
}
