using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour
{
	/*
	SpriteRenderer renderer;

	public float offSet;
	public int sortingOrderBase;
	public float timer;

	private float currentTimer;
	public bool isPlayer;

	void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		currentTimer = timer;
	}

	void Update()
	{

		renderer.sortingOrder = (int) (sortingOrderBase - transform.position.y - offSet);

		currentTimer -= Time.deltaTime;
		if (currentTimer <= 0)
		{
			renderer.color = new Color (1,1,1,1);
			currentTimer = timer;
		}

	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (isPlayer == false && other.GetComponent<Depth>().isPlayer == true)
		{
			renderer.color = new Color(1, 1, 1, 0.5f);
			currentTimer = 1;
		}
	}
	*/
	public int sortingOrderBase;
	public int offset;
	public bool runOnlyOnce;
	public bool isPlayer;

	private float timer;
	private float timerMax = .1f;
	private Renderer myrenderer;
	[SerializeField]
	private bool isTriggered;

	[SerializeField] private bool extraDepthZoneTriggered;

	private void Awake()
	{
		myrenderer = gameObject.GetComponent<Renderer>();
		isTriggered = false;
		extraDepthZoneTriggered = false;
	}

	private void LateUpdate()
	{
		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			timer = timerMax;
			if (extraDepthZoneTriggered == false)
			{
				myrenderer.sortingOrder = (int) (sortingOrderBase - transform.position.y - offset);
			}
			if (runOnlyOnce)
			{
				Destroy(this);
			}
		}
	}

	public void setExtraDepthZoneTriggered(bool token)
	{
		this.extraDepthZoneTriggered = token;
	}
}
