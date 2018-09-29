using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileController : MonoBehaviour
{

	public GameObject TileManager;

	private Func<float,bool> onTriggerAction;
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (onTriggerAction != null)
		{
			onTriggerAction(transform.position.z);
		}
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag("Pick Up"))
		{
			Destroy(gameObject);
		}
		
		if (other.gameObject.CompareTag("TileWall"))
		{
			other.gameObject.transform.parent.GetComponent<TileWallsController>().DecreaseResistance(other.gameObject);
		}
	}

	public void SetOnTriggerAction(Func<float, bool> onTriggerAction)
	{
		this.onTriggerAction = onTriggerAction;
	}

}
