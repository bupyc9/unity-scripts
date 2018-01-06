using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[AddComponentMenu("Control Script/Point Click Movement")]

public class PointClickMovement : MonoBehaviour {
    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;

    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    public float pushForce = 3.0f;

    private CharacterController charController;
    private float vertSpeed;
    private ControllerColliderHit contract;
    private Animator animator;

    public float deceleration = 1.0f;
    public float targetBuffer = 1.5f;
    private float curSpeed = 0f;
    private Vector3 targetPos = Vector3.one;

    void Start() {
        charController = GetComponent<CharacterController>();
        vertSpeed = minFall;
        animator = GetComponent<Animator>();
    }

    void Update() {
        var movement = Vector3.zero;

        if(Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject()) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if(Physics.Raycast(ray, out mouseHit)) {
                var hitObject = mouseHit.transform.gameObject;
                if(hitObject.layer == LayerMask.NameToLayer("Ground")) {
                    targetPos = mouseHit.point;
                    curSpeed = moveSpeed;
                }
            }
        }

        if(targetPos != Vector3.one) {
            var adjustedPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            var targetRot = Quaternion.LookRotation(adjustedPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
            movement = curSpeed * Vector3.forward;
            movement = transform.TransformDirection(movement);
        }

        if(Vector3.Distance(targetPos, transform.position) < targetBuffer) {
            curSpeed -= deceleration;
            if(curSpeed <= 0) {
                targetPos = Vector3.one;
            }
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);

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
}
