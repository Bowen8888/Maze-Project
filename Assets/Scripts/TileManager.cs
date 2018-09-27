using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{

	public GameObject FloorPrefab;

	private Transform _playerTransform;
	private int _spawnZ = 0;
	private readonly int[] _previousRow = new int[8];
	private GameObject[] _lastRow = new GameObject[8];
	
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
		Dictionary<int, List<int>> southWallOpenmap = new Dictionary<int, List<int>>();
		if (_spawnZ > 0)
		{
			
			Dictionary<int, List<int>> indexmap = new Dictionary<int, List<int>>();

			for (int i=0; i< 8; i++)
			{
				if (!indexmap.Keys.Contains(_previousRow[i]))
				{
					indexmap[_previousRow[i]] = new List<int>();

				}
				indexmap[_previousRow[i]].Add(i);
			}

			
			foreach (var indexpair in indexmap)
			{
				southWallOpenmap[indexpair.Key] = GetRandomNonEmptySubset(indexpair.Value);
			}
		}
		
		//assume southWallOpenmap contains the mapping from all set id to its corresponding cell index
		
		//translate the map into array
		int[] currentRow = new int[8];
		for (int i = 0 ; i<8 ;i++)
		{
			currentRow[i] = -1;
		}

		int max = -1;
		List<int> southWallOpen = new List<int>();
		foreach (var indexpair in southWallOpenmap)
		{
			foreach (var index in indexpair.Value)
			{
				currentRow[index] = indexpair.Key;
				southWallOpen.Add(index);
				max = Math.Max(max, indexpair.Key);
			}
		}
		
		//flush the cells
		for (int i = 0 ; i<8 ;i++)
		{
			if (currentRow[i] == -1)
			{
				currentRow[i] = ++max;
			}
		}
		var rnd = new System.Random();

		for (int i = 0; i < 8; i++)
		{
			GameObject tileCopy = Instantiate(FloorPrefab);
			tileCopy.transform.SetParent(transform);
			tileCopy.transform.position += Vector3.left * (i*5) + Vector3.forward * _spawnZ;

			//first row open southwall of enter and exit
			if (_spawnZ == 0)
			{
				if (i == 0 || i == 7)
				{
					tileCopy.GetComponent<TileWallsController>()
						.SetWallActivate(TileWallsController.TileWallName.SouthWall,false);
				}
			}
			else {
				if (southWallOpen.Contains(i))//remove southwall if cell is in the same set as the previous cell in the same column
				{
					tileCopy.GetComponent<TileWallsController>()
						.SetWallActivate(TileWallsController.TileWallName.SouthWall,false);
				}
			}
			
			//merge cells of different set and construct walls between two cells in the same row
			if(i < 7)
			{
				int curCellSet = currentRow[i];
				int nextCellSet = currentRow[i + 1];
				//merge
				double randomDouble = rnd.NextDouble();
				if (curCellSet != nextCellSet && randomDouble < 0.5)
				{
					tileCopy.GetComponent<TileWallsController>()
						.SetWallActivate(TileWallsController.TileWallName.WestWall, false);
					int min = Math.Min(curCellSet,nextCellSet);
					for (int j =0; j<8; j++)
					{
						if (currentRow[j] == curCellSet || currentRow[j] == nextCellSet)
						{
							currentRow[j] = min;
						}
					}
				}
			}
			
			//construct borders
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

			_lastRow[i] = tileCopy;
		}
		
		//update previousRow
		for (int j = 0; j< 8; j++)
		{
			_previousRow[j] = currentRow[j];
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

	public int GetScreenLimit()
	{
		return _spawnZ + 10;
	}

	public void ConstructNorthWalls()
	{
		foreach (var tile in _lastRow)
		{
			tile.GetComponent<TileWallsController>().SetWallActivate(TileWallsController.TileWallName.NorthWall, true);
		}
	}
}
