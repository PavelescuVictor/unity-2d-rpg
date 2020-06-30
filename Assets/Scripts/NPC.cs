using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{

	public GameObject dialogBoxPrefab;
	public GameObject canvasPrefab;
	public string dialogText;
	[SerializeField]
	private bool playerInRange;
	[SerializeField]
	private bool dialogBoxOpened;

	private GameObject dialogBox;
	private Canvas _canvas;


	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E) && playerInRange)
		{
			if (dialogBoxOpened == false)
			{
				dialogBox = Instantiate(dialogBoxPrefab, sceneCanvas.transform);
				dialogBoxOpened = true;
				dialogBox.GetComponentInChildren<Text>().text = dialogText;
			}
			else
			{
				Destroy(dialogBox);
				dialogBoxOpened = false;
			}

		}

		if (playerInRange == false && dialogBoxOpened)
		{
			Destroy(dialogBox);
			dialogBoxOpened = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("MainCharacter"))
		{
			playerInRange = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("MainCharacter"))
		{
			playerInRange = false;
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
}
