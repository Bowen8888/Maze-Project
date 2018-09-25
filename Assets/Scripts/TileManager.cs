using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

	public GameObject FloorPrefab;

	private Transform _playerTransform;
	private int _spawnZ = 0;
	private readonly int[] _previousRow = new int[8];
	
	// Use this for initialization
	void Start ()
	{
		_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (_playerTransform.position.z > _spawnZ + 10)
		{
			GenerateRaw();
		}
	}

	private void GenerateRaw()
	{
		List<int> southWallOpen = new List<int>();
		if (_spawnZ > 0)
		{
			List<int> indexes = new List<int>();

			int t = -1;
			for (int i = 0; i < 8; i++)
			{
				if (_previousRow[i] != t)
				{
					t = _previousRow[i];
					southWallOpen.AddRange(GetRandomNonEmptySubset(indexes));
					indexes.Clear();
					indexes.Add(i);
				}
				else
				{
					indexes.Add(i);
				}
			}
			
			southWallOpen.AddRange(GetRandomNonEmptySubset(indexes));
		}
		
		System.Random rnd = new System.Random();
		int k = 0;
		_previousRow[0] = k;
		for (int i = 0; i < 8; i++)
		{
			GameObject tileCopy = Instantiate(FloorPrefab);
			tileCopy.transform.SetParent(transform);
			tileCopy.transform.position += Vector3.left * (i*5) + Vector3.forward * _spawnZ;

			if (_spawnZ == 0)
			{
				if (i == 0 || i == 7)
				{
					tileCopy.GetComponent<TileWallsController>()
						.SetWallActivate(TileWallsController.TileWallName.SouthWall,false);
				}
			}

			if (southWallOpen.Contains(i))
			{
				tileCopy.GetComponent<TileWallsController>()
					.SetWallActivate(TileWallsController.TileWallName.SouthWall,false);
			}

			if (i < 7)
			{
				if (rnd.NextDouble() < 0.5)
				{
					tileCopy.GetComponent<TileWallsController>()
						.SetWallActivate(TileWallsController.TileWallName.WestWall,true);
					_previousRow[i+1] = ++k;
				}
				else
				{
					_previousRow[i+1] = k;
				}
			}
			
			if (i == 0)
			{
				tileCopy.GetComponent<TileWallsController>()
					.SetWallActivate(TileWallsController.TileWallName.EastWall,true);
			}
			else if (i == 7)
			{
				tileCopy.GetComponent<TileWallsController>()
					.SetWallActivate(TileWallsController.TileWallName.WestWall,true);
			}
		}
		_spawnZ += 5;
	}

	private List<int> GetRandomNonEmptySubset(List<int> input)
	{
		var result = new List<int>();
		System.Random rnd = new System.Random();

		foreach (var i in input)
		{
			if (rnd.NextDouble() < 0.5)
			{
				result.Add(i);
			}
		}

		if (result.Count == 0 && input.Count > 0)
		{
			result.Add(input[rnd.Next(input.Count)]);
		}
		
		return result;
	} 
}
