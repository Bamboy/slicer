using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PointDisplay : MonoBehaviour 
{
	Text t;
	void Awake()
	{
		t = GetComponent<Text>();
	}
	void Update()
	{
		t.text = "Points: " + GameManager.Instance.Points;
	}

}
