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
        private float movementVelocity;
        private Vector3 movementDirection;

        public float horizontalMove;
        public float verticalMove;
        public Vector3 lastLookDirection;

        [SerializeField] private Transform right;

        //todo Add Animator for movement animations.
        // todo MOBILE INPUT

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
            //todo Add animation checks into here
        }

        /// <summary>
        /// Gets the input for rotating the character.
        /// </summary>
        private void GetRotationInput()
        {
            if(Input.GetAxisRaw("Horizontal")>0)
            {
                horizontalMove = 1;
            }
            else if(Input.GetAxisRaw("Horizontal") < 0)
            {
                horizontalMove = -1;
            }
            else
            {
                horizontalMove = 0;
            }
            
            if(Input.GetAxisRaw("Vertical")>0)
            {
                verticalMove = 1;
            }
            else if(Input.GetAxisRaw("Vertical") < 0)
            {
                verticalMove = -1;
            }
            else
            {
                verticalMove = 0;
            }
        }
        
        /// <summary>
        /// Rotates the character based on the input and saves the last look direction.
        /// </summary>
        private void RotateCharacter()
        {
            switch(horizontalMove)
            {
                case 1:
                    transform.rotation = Quaternion.LookRotation(Vector3.right);
                    lastLookDirection = Vector3.right;
                    break;
                case -1:
                    transform.rotation = Quaternion.LookRotation(Vector3.left);
                    lastLookDirection = Vector3.left;

                    break;
                case 0:
                    transform.rotation = Quaternion.LookRotation(lastLookDirection);
                    break;
            }
            
            switch(verticalMove)
            {
                case 1:
                    transform.rotation = Quaternion.LookRotation(Vector3.forward);
                    lastLookDirection = Vector3.forward;
                    break;
                case -1:
                    transform.rotation = Quaternion.LookRotation(Vector3.back);
                    lastLookDirection = Vector3.back;

                    break;
                case 0:
                    transform.rotation = Quaternion.LookRotation(lastLookDirection);
                    break;
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
            GetRotationInput();
        }
        
        private void FixedUpdate()
        {
            Move();
            RotateCharacter();
        }
    }
}