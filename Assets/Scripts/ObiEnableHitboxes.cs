using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObiEnableHitboxes : MonoBehaviour
{
    public Animator animator;
    public GameObject hitBoxUp;
    public GameObject hitBoxDown;
    public GameObject hitBoxRight;
    public GameObject hitBoxLeft;
    public float delayTime;
    private float delayTimeCounter;
    [SerializeField]
    private bool hitBoxEnabled;
    // Start is called before the first frame update
    void Start()
    {
        hitBoxEnabled = false;
        delayTimeCounter = delayTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("PlayerAttacking") == true && hitBoxEnabled == false)
        {
            if (delayTimeCounter <= 0)
            {
                if (animator.GetFloat("LastMoveX") > 0.5f )
                {
                    hitBoxRight.SetActive(true);
                    hitBoxEnabled = true;
                }
                if (animator.GetFloat("LastMoveX") < -0.5f )
                {
                    hitBoxLeft.SetActive(true);
                    hitBoxEnabled = true;
                }
                if (animator.GetFloat("LastMoveY") > 0.5f)
                {
                    hitBoxUp.SetActive(true);
                    hitBoxEnabled = true;
                }
                if (animator.GetFloat("LastMoveY") < -0.5f)
                {
                    hitBoxDown.SetActive(true);
                    hitBoxEnabled = true;
                }

                if (animator.GetFloat("LastMoveX") > 0.5f && animator.GetFloat("LastMoveY") > 0.5f)
                {
                    hitBoxUp.SetActive(true);
                    hitBoxRight.SetActive(true);
                    hitBoxEnabled = true;
                }
                if (animator.GetFloat("LastMoveX") < -0.5f && animator.GetFloat("LastMoveY") > 0.5f)
                {
                    hitBoxUp.SetActive(true);
                    hitBoxLeft.SetActive(true);
                    hitBoxEnabled = true;
                }
                if (animator.GetFloat("LastMoveX") > 0.5f && animator.GetFloat("LastMoveY") < -0.5f)
                {
                    hitBoxDown.SetActive(true);
                    hitBoxRight.SetActive(true);
                    hitBoxEnabled = true;
                }
                if (animator.GetFloat("LastMoveX") < -0.5f && animator.GetFloat("LastMoveY") < -0.5f)
                {
                    hitBoxDown.SetActive(true);
                    hitBoxLeft.SetActive(true);
                    hitBoxEnabled = true;
                }
            }
            else
            {
                delayTimeCounter -= Time.deltaTime;
            }
        }
        else if (animator.GetBool("PlayerAttacking") == false)
        {
            hitBoxEnabled = false;
            hitBoxRight.SetActive(false);
            hitBoxLeft.SetActive(false);
            hitBoxUp.SetActive(false);
            hitBoxDown.SetActive(false);
            delayTimeCounter = delayTime;
        }
    }
}
