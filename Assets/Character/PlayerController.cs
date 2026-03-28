using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed, runSpeed, jumpHeight, gravity, turnSpeed, smooth;

    public Transform character;

    Camera playerCamera;

    CharacterController controller;

    Animator animator;

    Vector2 input;

    SkinnedMeshRenderer skinnedMeshRenderer;

    float verticalVelo;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerCamera = GetComponentInChildren<Camera>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (controller.isGrounded) 
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelo = jumpHeight;
                animator.SetTrigger("Jump");
            }
            
            if (input == Vector2.zero)
            {
                animator.SetInteger("State", 0);
            }
            else
            {
                animator.SetInteger("State", Input.GetKey(KeyCode.LeftShift) ? 2 : 1);
            }
        }
        else
        {
            animator.SetInteger("State", 3);
            verticalVelo -= gravity * Time.deltaTime;
        }

        float turn = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);

        Vector3 move = transform.right * input.x + transform.forward * input.y;
        move = move.normalized;

        //rotate child model
        if (move != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            character.rotation = Quaternion.Slerp(character.rotation, targetRotation, smooth * Time.deltaTime);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            playerCamera.transform.localPosition = new Vector3(0f, 0.86f, -0.015f);
            skinnedMeshRenderer.enabled = false;
        } else if (scroll < 0)
        {
            playerCamera.transform.localPosition = new Vector3(0f, 1.37f, -3.98f);
            skinnedMeshRenderer.enabled = true;
        }

        move *= Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        move = move + Vector3.up * verticalVelo;
        controller.Move(move * Time.deltaTime);
    }
}
