using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewZone : MonoBehaviour
{

	public string placeName;
	public GameObject textPrefab;
	public GameObject canvasPrefab;
	private Canvas _canvas;
	public float showTimeSeconds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("MainCharacter"))
		{
			StartCoroutine(placeNameCo());
		}
	}

	public Canvas sceneCanvas
	{
		get
		{
			// Check if canvas variables is set
			if (_canvas == null)
			{
				// If not set, look in scene for canvas
				_canvas = FindObjectOfType<Canvas>();

				if (_canvas == null)
				{
					// If no canvas in scene, create one
					_canvas = Instantiate(canvasPrefab).GetComponent<Canvas>();
				}
			}

			return _canvas;
		}
	}

	public IEnumerator placeNameCo()
	{
		GameObject newZoneText = Instantiate(textPrefab, sceneCanvas.transform);
		newZoneText.GetComponent<Text>().text = placeName;
		yield return new WaitForSeconds(showTimeSeconds);                                    
		Destroy(newZoneText);                                                    
	}
}
