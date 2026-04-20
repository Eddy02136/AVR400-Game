using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed, runSpeed, jumpHeight, gravity, turnSpeed, smooth;

    public Transform character, cameraPivot;

    Camera playerCamera;

    CharacterController controller;

    Animator animator;

    Vector2 input;

    SkinnedMeshRenderer skinnedMeshRenderer;

    float verticalVelo;

    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerCamera = GetComponentInChildren<Camera>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        playerStats = GetComponent<PlayerStats>();
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
                playerStats.RegenStamina(0.3f);
            }
            else if (Input.GetKey(KeyCode.LeftShift) && !playerStats.IsExhausted())
            {
                animator.SetInteger("State", 2);
                playerStats.LoseStamina(0.5f);
            } else
            {
                animator.SetInteger("State", 1);
                playerStats.RegenStamina(0.3f);
            }
        }
        else
        {
            animator.SetInteger("State", 3);
            verticalVelo -= gravity * Time.deltaTime;
        }

        Vector3 forward = cameraPivot.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cameraPivot.right;
        right.y = 0;
        right.Normalize();

        Vector3 move = right * input.x + forward * input.y;
        move = move.normalized;

        //rotate child model
        if (move != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            character.rotation = Quaternion.Slerp(character.rotation, targetRotation, smooth * Time.deltaTime);
        }

        move *= (Input.GetKey(KeyCode.LeftShift) && !playerStats.IsExhausted())  ? runSpeed : walkSpeed;

        move = move + Vector3.up * verticalVelo;
        controller.Move(move * Time.deltaTime);
    }
}
