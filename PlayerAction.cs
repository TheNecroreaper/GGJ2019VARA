using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour {

    //Reference Variables
    private CollisionSystem playerCollision;
    private Transform player;

    //Constants and variables for horizontal movement
    private const float GROUNDED_MOVEMENT = 0.15f;
    private const float AIR_MOVEMENT = 0.1f;
    private float moveSpeed;

    //Constant for jumping
    private const float JUMP_FORCE = 400f;

    //Grid snapping
    private Collider selected;
    private LinkedList<Collider> touchingUnits;

	// Use this for initialization
	void Start () {
        touchingUnits = new LinkedList<Collider>();
        playerCollision = GetComponent<CollisionSystem>();
        player = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        //Set status
        if (playerCollision.grounded)
            moveSpeed = GROUNDED_MOVEMENT;
        else
            moveSpeed = AIR_MOVEMENT;

        //Controls
        if (Input.GetKey("d") && !playerCollision.rightWalled)
            player.position += Vector3.right * moveSpeed;

        if (Input.GetKey("a") && !playerCollision.leftWalled)
            player.position += Vector3.left * moveSpeed;

        if (Input.GetKeyDown("space") && playerCollision.grounded)
            GetComponent<Rigidbody>().AddForce(Vector3.up * JUMP_FORCE);
	}

    //Checks triggering grid units that touch player
    void OnTriggerEnter(Collider other) {
        if (other.tag == "GridUnit")
            touchingUnits.AddLast(other);
    }

    //Checks when grid units no longer touch player
    void OnTriggerExit(Collider other) {
        if (other.tag == "GridUnit")
            touchingUnits.Remove(other);
    }
}
