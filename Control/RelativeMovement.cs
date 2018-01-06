using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class RelativeMovement : MonoBehaviour {
    [SerializeField] private Transform target;

    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;
    public float runSpeed = 3.0f;

    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    public float pushForce = 3.0f;

    private CharacterController charController;
    private float vertSpeed;
    private ControllerColliderHit contract;
    private Animator animator;

    private void Start() {
        charController = GetComponent<CharacterController>();
        vertSpeed = minFall;
        animator = GetComponent<Animator>();
    }

    private void Update() {
        var movement = Vector3.zero;

        var horInput = Input.GetAxis("Horizontal");
        var verInput = Input.GetAxis("Vertical");

        if(Input.GetButtonDown("Run")) {
            moveSpeed *= runSpeed;
        }

        if(Input.GetButtonUp("Run")) {
            moveSpeed /= runSpeed;
        }

        if(horInput != 0 || verInput != 0) {
            movement.x = horInput * moveSpeed;
            movement.z = verInput * moveSpeed;

            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            var tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            var direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);

        var hitGround = CheckGround();

        if(hitGround) {
            if(Input.GetButtonDown("Jump")) {
                vertSpeed = jumpSpeed;
            } else {
                vertSpeed = minFall;
                animator.SetBool("Jumping", false);
            }
        } else {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if(vertSpeed < terminalVelocity) {
                vertSpeed = terminalVelocity;
            }

            if(contract != null) {
                animator.SetBool("Jumping", true);
            }

            if(charController.isGrounded) {
                if(Vector3.Dot(movement, contract.normal) < 0) {
                    movement = contract.normal * moveSpeed;
                } else {
                    movement += contract.normal * moveSpeed;
                }
            }
        }

        movement.y = vertSpeed;
        movement *= Time.deltaTime;
        charController.Move(movement);
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        contract = hit;

        var body = hit.collider.attachedRigidbody;
        if(body != null && !body.isKinematic) {
            body.velocity = hit.moveDirection * pushForce;
        }
    }

    private bool CheckGround() {
        RaycastHit hit;
        if(vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) {
            var check = (charController.height + charController.radius) / 1.9f;
            return hit.distance <= check;
        }

        return false;
    }
}
