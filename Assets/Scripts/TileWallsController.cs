using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileWallsController : MonoBehaviour
{
	private GameObject _westWall;
	private GameObject _eastWall;
	private GameObject _southWall;
	private GameObject _northWall;
	private Dictionary<GameObject, int> _projectileResistanceMap = new Dictionary<GameObject, int>();
	
	// Use this for initialization
	void Awake ()
	{
		_westWall = gameObject.transform.GetChild(0).gameObject;
		_eastWall = gameObject.transform.GetChild(1).gameObject;
		_southWall = gameObject.transform.GetChild(2).gameObject;
		_northWall = gameObject.transform.GetChild(4).gameObject;

		_projectileResistanceMap[_westWall] = 3;
		_projectileResistanceMap[_southWall] = 3;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public enum TileWallName
	{
		WestWall,
		EastWall,
		SouthWall,
		NorthWall
	}
	
	public void SetWallActivate(TileWallName tileWallName,bool active)
	{
		switch (tileWallName)
		{
			case TileWallName.WestWall:
				_westWall.SetActive(active);
				break;
			case TileWallName.EastWall:
				_eastWall.SetActive(active);
				break;
			case TileWallName.SouthWall:
				_southWall.SetActive(active);
				break;
			case TileWallName.NorthWall:
				_northWall.SetActive(active);
				break;
			default:
				throw new ArgumentOutOfRangeException("tileWallName", tileWallName, null);
		}
	}

	public void DecreaseResistance(GameObject tileWall)
	{
		if (!_projectileResistanceMap.Keys.Contains(tileWall))
		{
			Debug.Log("This wall is not destroyable.");
			return;
		}
		
		_projectileResistanceMap[tileWall]--;
		if (_projectileResistanceMap[tileWall] <= 0)
		{
			tileWall.SetActive(false);
		}
	}

	public void UntagWestWall()
	{
		_westWall.tag = "Untagged";
	}
}
