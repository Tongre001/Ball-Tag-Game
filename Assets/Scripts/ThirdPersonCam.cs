using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class ThirdPersonCam : NetworkBehaviour {
    public GameObject player;
    public float damping = 1f;

    public float sensitivity = 3f;

    public bool TargetPlayer = false;
    public bool OrbitPlayer = true;
    public float RotationSpeed = 8.0f;

    public static Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - player.transform.position;
	}
	
	// Update is called once at the end per frame
	void LateUpdate () {
        if (OrbitPlayer) {
            float mousex = Input.GetAxis("Mouse X");
            Quaternion camturnangle = Quaternion.AngleAxis(mousex * RotationSpeed, Vector3.up);

            offset = camturnangle * offset;
        }
        Vector3 newPos = player.transform.position + offset;
        transform.position = Vector3.Slerp(transform.position, newPos, damping);

        if (TargetPlayer || OrbitPlayer) {
            transform.LookAt(player.transform);
        }
	}

    public void setTarget(GameObject target) {
        player = target;
    }
}
