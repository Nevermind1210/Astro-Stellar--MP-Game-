using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A1.Player
{
    /// <summary>
    /// Handles the movement of the player objects.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMotor : MonoBehaviour
    {
        [Header("Player Variables")]
        [SerializeField, Tooltip("Player Speed")] private float speed = 10;
        private GameObject cameraGameObject;

        private Rigidbody rb;
        [SerializeField] public Animator animator;
        private float movementVelocity;
        private Vector3 movementDirection;

        public Vector3 lastLookDirection;

        //todo Add Animator for movement animations.
        

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            cameraGameObject = FindObjectOfType<Camera>().gameObject;
        }

        /// <summary>
        /// Handles Input for the character movement and direction. Called in Update.
        /// </summary>
        private void GetMovementInput()
        {
            movementDirection = Quaternion.Euler(0, cameraGameObject.transform.eulerAngles.y, 0) 
                                * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
            {
                movementVelocity = speed;
            }
            else
            {
                movementVelocity = 0;
            }
            
        }

        
        /// <summary>
        /// Moves the character by applying force to the rigidbody. Called in Fixed Update.
        /// </summary>
        private void Move()
        {
            Vector3 travelDirection = rb.velocity;
            float friction = travelDirection.magnitude *0.5f;
            Vector3 frictionDirection = new Vector3(-travelDirection.x, 0, -travelDirection.z).normalized;
            Vector3 frictionForce = friction * frictionDirection;
            
            Vector3 force = movementDirection * movementVelocity;
            
            rb.AddForce(frictionForce);
            rb.AddForce(force,ForceMode.Force);
            
        }
        
        
        // Update is called once per frame
        void Update()
        {   
            GetMovementInput();
        }
        
        private void FixedUpdate()
        {
            Move();
            // Sets the character facing direction when moving
            if(movementVelocity != 0)
            {
                // todo Trigger run animation here. Maybe with bool to trigger run and hold.
                animator.SetBool("isWalking", true);
                
                transform.rotation = Quaternion.LookRotation(movementDirection);
                // Saves the direction to use when stopped.
                lastLookDirection = movementDirection;
            }
            // Sets the character facing direction to the last direction it was moving in.
            if(movementVelocity == 0)
            {
                //todo trigger idle animation here
                animator.SetBool("isWalking",false);
                transform.rotation = Quaternion.LookRotation(lastLookDirection);
            }
            

            
        }
    }
}