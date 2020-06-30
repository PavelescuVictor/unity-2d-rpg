using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	// Camera Follow
	public GameObject followTarget;
	private Vector3 targetPosition;
	public float camMoveSpeed;
	
	// Camera bounds
	public bool bounds;
	public Vector3 minCameraPosition;
	public Vector3 maxCameraPosition;
	
	// Camera colider
	public BoxCollider2D boundsBox;
	private Vector3 minBounds;
	private Vector3 maxBounds;
	private Camera theCamera;
	private float halfHeight;
	private float halfWidth;
	
	void Start ()
	{
		minBounds = boundsBox.bounds.min;
		maxBounds = boundsBox.bounds.max;

		theCamera = GetComponent<Camera>();
		halfHeight = theCamera.orthographicSize;
		halfWidth = halfHeight * Screen.width / Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		// Camera follow
		targetPosition = new Vector3(
			followTarget.transform.position.x, 
			followTarget.transform.position.y, 
			transform.position.z
			);
		transform.position = Vector3.Lerp(
			transform.position, 
			targetPosition,
			camMoveSpeed * Time.deltaTime
			);
		
		
		// Camera colider 
		float clampedX = Mathf.Clamp(
			transform.position.x,
			minBounds.x + halfWidth,
			maxBounds.x - halfWidth);
		float clampedY = Mathf.Clamp(
			transform.position.y,
			minBounds.y + halfHeight,
			maxBounds.y - halfHeight
			);
		transform.position = new Vector3(clampedX, clampedY, transform.position.z);
		
		// Camera Bounds

		//if (bounds)
		//{
		//	transform.position = new Vector3(
		//		Mathf.Clamp(transform.position.x, minCameraPosition.x, maxCameraPosition.x), 
		//		Mathf.Clamp(transform.position.y, minCameraPosition.y, maxCameraPosition.y), 
		//		Mathf.Clamp(transform.position.z, minCameraPosition.z, maxCameraPosition.z));
		//}
	}
}
