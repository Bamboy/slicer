using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ObjectSpawner))]
[ExecuteInEditMode]
public class ObjectSpawnerVisualizer : MonoBehaviour 
{

	#if UNITY_EDITOR
	private ObjectSpawner spawner;
	void Awake()
	{
		spawner = GetComponent<ObjectSpawner>();
	}
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine( spawner.leftSpawnPoint.position, spawner.throwTargets[0] );
		Gizmos.DrawLine( spawner.leftSpawnPoint.position, spawner.throwTargets[1] );

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine( spawner.midSpawnPoint.position, spawner.throwTargets[2] );
		Gizmos.DrawLine( spawner.midSpawnPoint.position, spawner.throwTargets[3] );

		Gizmos.color = Color.red;
		Gizmos.DrawLine( spawner.rightSpawnPoint.position, spawner.throwTargets[4] );
		Gizmos.DrawLine( spawner.rightSpawnPoint.position, spawner.throwTargets[5] );
	}

	#endif
}
