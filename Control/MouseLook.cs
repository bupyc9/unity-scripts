using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Control Script/Mouse Look")]

public class MouseLook : MonoBehaviour {
    public enum RotationAxes {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityHor = 9.0f;
    public float sensitivityVer = 9.0f;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    private float rotationX = 0;

    private void Start() {
        var rigidbody = GetComponent<Rigidbody>();
        if(rigidbody != null) {
            rigidbody.freezeRotation = true;
        }
    }

    private void Update() {
        if(axes == RotationAxes.MouseX) {
            MouseX();
        } else if(axes == RotationAxes.MouseY) {
            MouseY();
        } else {
            MouseXAndY();
        }
    }

    private void MouseX() {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
    }

    private void MouseY() {
        rotationX -= Input.GetAxis("Mouse Y") * sensitivityVer;
        rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);

        float rotationY = transform.localEulerAngles.y;
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }

    private void MouseXAndY() {
        rotationX -= Input.GetAxis("Mouse Y") * sensitivityVer;
        rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);

        float delta = Input.GetAxis("Mouse X") * sensitivityHor;
        float rotationY = transform.localEulerAngles.y + delta;

        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }
}