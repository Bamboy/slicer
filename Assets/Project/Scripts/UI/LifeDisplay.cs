using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeDisplay : MonoBehaviour 
{
	public GameObject heartPrefab;
	public float spacing = 1.5f;
	public float beatSpeed = 1f;
	public float beatSize = 1f;

	GameObject[] hearts;


	// Use this for initialization
	void Start () 
	{
		hearts = new GameObject[0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( GameManager.GameMode != GameModes.Classic || GameManager.GameMode != GameModes.Pitch )
		{
			Debug.LogWarning("Lives not implemented for this gamemode. Destroying UI.");
			Destroy( this.gameObject );
		}


		while( hearts.Length != Mathf.Abs(GameManager.Instance.Lives) ) //Mathf.Abs to keep from infinitely trying to go negative
		{
			if( hearts.Length > GameManager.Instance.Lives )
				DestroyHeart();
			else
				CreateHeart();
		}

		//Heart beat animation
		float size = Mathf.PingPong( GameManager.Instance.GameTime * beatSpeed, beatSize ) + 0.9f; //TODO make beat speed increase based on lives lost.
		for( int i = 0; i < hearts.Length; i++ )
		{
			if( i == 0 )
				hearts[i].transform.localScale = new Vector3( size, size, size );
			else
				hearts[i].transform.localScale = Vector3.one;
		}
	}
	void CreateHeart()
	{
		GameObject newHeart = GameObject.Instantiate( heartPrefab, Vector3.zero, Quaternion.identity ) as GameObject;

		//RectTransform t = newHeart.GetComponent<RectTransform>();


		Vector3 pos = new Vector3( transform.position.x - 1f - (hearts.Length * spacing), transform.position.y, 0f );
		newHeart.transform.position = pos;

		newHeart.transform.SetParent( this.transform );

		hearts = ArrayTools.Push<GameObject>( hearts, newHeart );
	}
	void DestroyHeart()
	{
		if( hearts.Length == 0 )
		{
			Debug.LogError("No more hearts left!", this);
		}
		
		GameObject deadHeart = hearts[0];


		hearts = ArrayTools.Pop<GameObject>( hearts );


		//TODO - animate heart dying.
		Destroy( deadHeart );
	}

}
