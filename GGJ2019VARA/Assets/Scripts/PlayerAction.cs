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

    //Global Variables
    public int queuePos;
    public static bool activePlayer;
    private bool snappable;

    //Audio source for snap
    public AudioSource snap;

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

        //Check if selection has changed
        if(selected == null || !selected.bounds.Contains(player.position)) {
            //Deselect unit
            if(selected != null)
                selected.SendMessage("deselectUnit");

            //See which unit contains the center of the currentBlock
            foreach (Collider unit in touchingUnits)
                if (unit.bounds.Contains(player.position))
                    selected = unit;

            //Select new unit
            if(selected != null)
                selected.SendMessage("selectUnit");
        }

        //Controls
        if (Input.GetKey("d") && !playerCollision.rightWalled)              //Move right     
            player.position += Vector3.right * moveSpeed;

        if (Input.GetKey("a") && !playerCollision.leftWalled)               //Move left
            player.position += Vector3.left * moveSpeed;

        if (Input.GetKeyDown("space") && playerCollision.grounded)          //Jump
            GetComponent<Rigidbody>().AddForce(Vector3.up * JUMP_FORCE);

        if (Input.GetKeyDown("return") && selected != null && snappable == true)               //Select cube
            selectGridUnit();

    }

    //Does selection action
    private void selectGridUnit() {
        player.position = selected.bounds.center;       //Snaps player to nearest block
        selected.SendMessage("deselectUnit");
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;  //Fixes block in place
        activePlayer = false;

        snap.Play();

        //Send a message to your dispenser
        Object.Destroy(GetComponent<PlayerAction>());
    }

    //Checks triggering grid units that touch player
    void OnTriggerEnter(Collider other) {
        if (other.tag == "GridUnit")
        {
            touchingUnits.AddLast(other);
            //r/thanosdidnothingwrong
            snappable = true;
        }
        else if (other.tag == "outside")
        {
            snappable = false;
            touchingUnits.AddLast(other);
        }
    }

    //Checks when grid units no longer touch player
    void OnTriggerExit(Collider other) {
        if (other.tag == "GridUnit" || other.tag == "outside") 
            touchingUnits.Remove(other);
    }
}
