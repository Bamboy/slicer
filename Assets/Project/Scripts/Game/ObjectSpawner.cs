using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour 
{
   
	public Transform objectContainer;
	public Transform leftSpawnPoint;
	public Transform midSpawnPoint;
	public Transform rightSpawnPoint;

	public float forceMultiplier = 2.5f;

	public Vector2[] throwTargets = new Vector2[6];
	
	//animationcurve for increasing spawn rates

	public GameObject[] pointObjectPrefabs = new GameObject[5];
	public int[] pointObjectSpawnWeights = new int[5];

	public float maxTime = 180f;
	public AnimationCurve pointSpawnRateOverTime;



	void Start () 
	{
		if( objectContainer == null || leftSpawnPoint == null || rightSpawnPoint == null || midSpawnPoint == null )
		{
			Debug.LogError("A specified transform was not given!", this);
			Debug.Break();
		}
		else
		{
			StartCoroutine("SpawnLoop");
		}
	}


	IEnumerator SpawnLoop()
	{
		float delay = Mathf.Clamp( pointSpawnRateOverTime.Evaluate( Time.timeSinceLevelLoad / maxTime ), /*maxSpawnratedelay =*/ 0.2f, /*minSpawnratedelay =*/ 2f);
		//Debug.Log( delay );
		yield return new WaitForSeconds( delay );

		int pointObjIndex = ExtRandom<int>.WeightedChoice(new int[5] {0,1,2,3,4}, pointObjectSpawnWeights);

		SpawnPointObject( pointObjIndex );
		StartCoroutine("SpawnLoop");
	}

	void SpawnPointObject( int pointObject )
	{
		int spawnlocindex = Mathf.RoundToInt( Random.value * 2f ); //Random int 0 to 3

		GameObject obj = Instantiate( pointObjectPrefabs[pointObject], GetSpawnLocation(spawnlocindex), Quaternion.identity ) as GameObject;
		obj.transform.SetParent( objectContainer );

		Vector2 forceLoc = new Vector2();
		switch( spawnlocindex ) //Get a position inbetween, based on where we started.
		{
		case 0:
			forceLoc = Vector2.Lerp(throwTargets[0], throwTargets[1], Random.value);
			break;
		case 1:
			forceLoc = Vector2.Lerp(throwTargets[2], throwTargets[3], Random.value);
			break;
		case 2:
			forceLoc = Vector2.Lerp(throwTargets[4], throwTargets[5], Random.value);
			break;
		default:
			Debug.LogError("WTF");
			break;
		}

		Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
		//Get the direction to the inbetween position and apply force to that location.
		rb.AddForce( VectorExtras.Direction( VectorExtras.V2FromV3(GetSpawnLocation(spawnlocindex)), forceLoc ) * forceMultiplier );
		rb.AddTorque( (Random.value - 0.5f) * 650f );

		Destroy( obj, 10f );
       
    }
	
	Vector3 GetSpawnLocation( int val )
	{
		switch( val )
		{
		case 0:
			return leftSpawnPoint.position;
		case 1:
			return midSpawnPoint.position;
		case 2:
			return rightSpawnPoint.position;
		default:
			Debug.LogError("WTF");
			return Vector3.zero;
		}
	}
}
