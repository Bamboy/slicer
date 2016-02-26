using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour 
{
	private static CameraShake instance;
	public static CameraShake Instance
	{
		get{ return instance; }
	}

	public AnimationCurve intensityCurve;

	private Vector3 originalPosition;
	private float maxIntensity;


	void Start()
	{
		instance = this;
		originalPosition = transform.position;
	}


	void Update()
	{
		//For testing...
		//if( Input.GetKeyDown(KeyCode.Backslash) )
		//	ShakeCamera( 1f, 1f );
	}

	public void ShakeCamera( float duration, float intensity = 0.1f )
	{
		StopCoroutine("DoShaking");
		maxIntensity = intensity;
		transform.position = originalPosition;
		StartCoroutine("DoShaking", duration);
	}

	IEnumerator DoShaking( float duration )
	{
		float elapsedTime = 0f;

		while( elapsedTime < duration )
		{
			float thisIntensity = maxIntensity * intensityCurve.Evaluate( elapsedTime / duration );
			transform.position = originalPosition + (Random.insideUnitSphere * thisIntensity);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.position = originalPosition;
	}
}
