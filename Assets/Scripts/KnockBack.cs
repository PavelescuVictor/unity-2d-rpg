using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MainCharacter"))
        {
            Rigidbody2D otherRigidBody = other.GetComponent<Rigidbody2D>();
            if (otherRigidBody != null)
            {
                otherRigidBody.GetComponent<ObiMovementController>().currentState = ObiMovementController.PlayerState.stagger;
                otherRigidBody.GetComponent<ObiMovementController>().Knock(knockTime);
                Vector2 difference = otherRigidBody.transform.position - transform.position;
                difference = difference.normalized * thrust;
                otherRigidBody.AddForce(difference, ForceMode2D.Impulse);
            }
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D otherRigidBody = other.GetComponentInParent<Rigidbody2D>();
            if (otherRigidBody != null)
            {
                otherRigidBody.GetComponent<Enemy>().currentState = Enemy.EnemyState.stagger;
                otherRigidBody.GetComponent<Enemy>().Knock(knockTime, damage);
                Vector2 difference = otherRigidBody.transform.position - transform.position;
                difference = difference.normalized * thrust;
                otherRigidBody.AddForce(difference, ForceMode2D.Impulse);
            }
        }
    }
}
