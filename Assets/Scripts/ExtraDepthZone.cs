using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraDepthZone : MonoBehaviour
{
    public GameObject parent;

    [SerializeField] private bool isTriggered;
    private float timer;
    private float timerMax = .1f;
    private Renderer parentRenderer;
    public bool runOnlyOnce;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        parentRenderer = parent.GetComponent<Renderer>();
        isTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = timerMax;
            if (isTriggered == true)
            {
                parentRenderer.sortingOrder = player.GetComponent<Renderer>().sortingOrder + 1;
            }
            if (runOnlyOnce)
            {
                Destroy(this);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Depth>() != null)
        {
            if (other.GetComponent<Depth>().isPlayer == true)
            {
                isTriggered = true;
                player = other.gameObject;
                parentRenderer.GetComponent<Depth>().setExtraDepthZoneTriggered(true);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Depth>() != null)
        {
            if (other.GetComponent<Depth>().isPlayer == true)
            {
                isTriggered = false;
                player = other.gameObject;
                parentRenderer.GetComponent<Depth>().setExtraDepthZoneTriggered(false);
            }
        }
    }
}
