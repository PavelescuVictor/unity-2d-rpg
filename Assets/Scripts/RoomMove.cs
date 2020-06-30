using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMove : MonoBehaviour
{

	public Vector3 cameraChange;
	public Vector3 playerChange;
	private CameraController cam;
	[SerializeField]
	private bool movingLeft;
	[SerializeField]
	private bool movingRight;
	private Vector3 startPosition;
	
	// Use this for initialization
	void Start ()
	{
		cam = Camera.main.GetComponent<CameraController>();
		movingLeft = false;
		movingRight = false;
		startPosition = cam.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (cam.transform.position.x < startPosition.x)
		{
			movingLeft = true;
			movingRight = false;
		}
		else if (cam.transform.position.x > startPosition.x)
		{
			movingRight = true;
			movingLeft = false;
		}

		startPosition = cam.transform.position;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("MainCharacter"))
		{
			if (movingRight == true)
			{
				cam.minCameraPosition += cameraChange;
				cam.maxCameraPosition += cameraChange;
				other.transform.position += playerChange;
			}
			else if(movingLeft == true)
			{
				cam.minCameraPosition -= cameraChange;
				cam.maxCameraPosition -= cameraChange;
				other.transform.position -= playerChange;
			}
		}
	}
}
