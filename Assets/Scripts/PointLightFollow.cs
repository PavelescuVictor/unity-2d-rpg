using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightFollow : MonoBehaviour
{

    public Camera cam;
    private float initialDistanceX;
    private float initialDistanceY;
    public float offsetX;
    public float offsetY;
    
    // Start is called before the first frame update
    void Start(){
        transform.position = new Vector2(cam.transform.position.x + offsetX, cam.transform.position.y + offsetY);
        initialDistanceX = Vector2.Distance(new Vector2(cam.transform.position.x, 0), new Vector2(transform.position.x, 0));
        initialDistanceY = Vector2.Distance(new Vector2(0, cam.transform.position.y), new Vector2(0, transform.position.y));
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > cam.transform.position.x && transform.position.y > cam.transform.position.y)
        {
            transform.position = new Vector2(cam.transform.position.x + initialDistanceX, cam.transform.position.y + initialDistanceY);
        }
        else if (transform.position.x > cam.transform.position.x && transform.position.y < cam.transform.position.y)
        {
            transform.position = new Vector2(cam.transform.position.x + initialDistanceX, cam.transform.position.y - initialDistanceY);
        }
        else if (transform.position.x < cam.transform.position.x && transform.position.y > cam.transform.position.y)
        {
            transform.position = new Vector2(cam.transform.position.x - initialDistanceX, cam.transform.position.y + initialDistanceY);
        }
        else if (transform.position.x < cam.transform.position.x && transform.position.y < cam.transform.position.y)
        {
            transform.position = new Vector2(cam.transform.position.x - initialDistanceX, cam.transform.position.y - initialDistanceY);
        }
    }
}
