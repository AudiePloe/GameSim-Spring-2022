// Audie Ploe

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Animation")]
    public Animator FrogController;

    [Header("Sound")]
    public AudioSource walkSound;
    public AudioSource runSound;
    public AudioSource jumpSound;
    public AudioSource croakSound;
    public float timeBetweenCroak;

    [Header("GameObjects")]
    public CharacterController controller;
    public Transform cam;
    public Transform groundCheck;
    public LayerMask groundMask;

    [Header("MovementSettings")]
    public float crouchSpeed;
    public float sprintSpeed;
    public float walkSpeed;
    public float turnSmoothTime = 0.1f;
    public float movementAcceleration;
    float speed;

    [Header("JumpSettings")]
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    public float groundDistance = 0.4f;
    Vector3 velocity;

    [Header("PlayerStates")]
    public bool isGrounded;
    public bool isCrouched;
    public bool canMove = true;

    
    float turnSmoothVelocity;
    float time;
    

    private void Awake()
    {
        speed = walkSpeed;

    }


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;


        if(time >= timeBetweenCroak)
        {
            croakSound.Play();
            time = 0;
        }


        if (canMove)
        {

            // checking if on ground ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }


            // adding gravity ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);




            // player is trying to jump ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                // play jump animation here
                {

                    if (isGrounded && velocity.y < 0)//Added by Humberto
                    {//Added by Humberto
                        FrogController.SetBool("isJumping", true);//Added by Humberto
                        FrogController.SetBool("isFalling", true);//Added by Humberto
                    }//Added by Humberto

                }

                jumpSound.Play();
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
            else//Added by Humberto
            {//Added by Humberto
                FrogController.SetBool("isJumping", false);//Added by Humberto
                FrogController.SetBool("isFalling", false);//Added by Humberto
            }//Added by Humberto






            // player walking ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            

            //walk
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {

                // play walk animation here
                FrogController.SetBool("isWalking", true);

                // walking sound here

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            else
            {
                FrogController.SetBool("isWalking", false);
            }






            // player running ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


            if (Input.GetKey(KeyCode.LeftShift) && isGrounded && direction.magnitude >= 0.1f)
            {
                // running sound here
                FrogController.SetBool("isRunning", true);
                if(speed < sprintSpeed)
                {
                    print("accelerate");
                    speed += movementAcceleration;
                }

            }
            else
            {
                speed = walkSpeed;
                FrogController.SetBool("isRunning", false);
            }





            // player crouching ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            if (Input.GetKey(KeyCode.LeftControl) && direction.magnitude >= 0.1f)
            {
                FrogController.SetBool("isCrouching", true);
                FrogController.SetBool("isWalking", false);
            }
            else
            {
                FrogController.SetBool("isCrouching", false);
            }





            if (Input.GetKey(KeyCode.LeftControl))// && isGrounded)
            {
                // play crouch animation here

                print("Playercrouch");

                FrogController.SetBool("isCrouchingIdle", true);//Added by Humberto
                speed = crouchSpeed;
                isCrouched = true;
            }
            else
            {
                FrogController.SetBool("isCrouchingIdle", false);//Added by Humberto
                isCrouched = false;
            }

        }
    }





}