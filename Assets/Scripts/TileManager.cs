using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

	public GameObject floorPrefab;

	private Transform playerTransform;
	private float spawnZ = 0;
	
	// Use this for initialization
	void Start ()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerTransform.position.z > spawnZ + 10)
		{
			GenerateRaw();
		}
	}

	private void GenerateRaw()
	{
		for (int i = 0; i < 8; i++)
		{
			GameObject tileCopy = Instantiate(floorPrefab);
			tileCopy.transform.SetParent(transform);
			tileCopy.transform.position += Vector3.left * (i*5) + Vector3.forward * spawnZ;
			tileCopy.GetComponent<TileWallsController>()
				.SetWallActivate(TileWallsController.TileWallName.SouthWall,false);
		}
		spawnZ += 5;
	}
}
