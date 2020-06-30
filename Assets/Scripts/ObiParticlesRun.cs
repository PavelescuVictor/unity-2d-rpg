using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObiParticlesRun : MonoBehaviour
{

    public GameObject player;
    public float waitTimeMin;
    public float waitTimeMax;
    public float showTime;
    public float showOffsetY;
    public float showOffsetX;
    [SerializeField]private float currentWaitTime;
    [SerializeField]private float currentShowTime;

    private Animator animator;

    private bool showPositionSet;
    private bool showParticles;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentShowTime = showTime;
        currentWaitTime = Random.Range(waitTimeMin, waitTimeMax);
        showParticles = false;
        showPositionSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Animator>().GetBool("PlayerMoving") == true)
        {
            if (currentWaitTime < 0)
            {
                if (showPositionSet == false)
                {
                    if (player.GetComponent<Animator>().GetFloat("MoveX") > 0)
                    {
                        Vector3 finalPosition = new Vector3(player.transform.position.x - showOffsetX, player.transform.position.y - showOffsetY, transform.position.z);
                        transform.position = finalPosition;
                    }
                    else
                    {
                        Vector3 finalPosition = new Vector3(player.transform.position.x + showOffsetX, player.transform.position.y - showOffsetY, transform.position.z);
                        transform.position = finalPosition;
                    }
                    showPositionSet = true;
                }
                if (currentShowTime < 0)
                {
                    showParticles = false;
                    animator.SetBool("ShowParticles", showParticles);
                    currentShowTime = showTime;
                    currentWaitTime = Random.Range(waitTimeMin, waitTimeMax);
                    showPositionSet = false;
                }
                else
                {
                    showParticles = true;
                    animator.SetBool("ShowParticles", showParticles);
                    animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
                    animator.SetFloat("MoveY", Input.GetAxis("Vertical"));
                    currentShowTime -= Time.deltaTime;
                }
            }
            else
            {
                showParticles = false;
                animator.SetBool("ShowParticles", showParticles);
                currentWaitTime -= Time.deltaTime;
            }
        }
        else if (player.GetComponent<Animator>().GetBool("PlayerMoving") == false)
        {
            showParticles = false;
            animator.SetBool("ShowParticles", showParticles);
            currentWaitTime = Random.Range(waitTimeMin, waitTimeMax);
            currentShowTime = showTime;
            showPositionSet = false;
        }
    }
}
