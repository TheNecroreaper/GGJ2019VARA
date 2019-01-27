using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem : MonoBehaviour {

    //Main Object collision data
    private Renderer render;
    private Rigidbody rb;
    private Vector3 objectBoundMax;
    private Vector3 objectBoundMin;
    private const float OFFSET = 0.5f;
    private Vector3 OFFSET_VECTOR;

    //Relationship data
    private bool withinXBound;
    private bool withinYBound;

    //Collider Data
    private string colliderTag;
    private Vector3 colliderMaxBounds;
    private Vector3 colliderMinBounds;

    //Behavior enablers
    public bool grounded;
    public bool leftWalled;
    public bool rightWalled;

    //Collider Dictionary
    private Dictionary<string, Collider> touching;

	// Use this for initialization
	void Start () {
        render = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        OFFSET_VECTOR = new Vector3(OFFSET, OFFSET);
        touching = new Dictionary<string, Collider>();
	}

    //Updates by checking if still touching with colliders in "touching" dictionary
    void Update() {
        //Updates offset bound every frame
        Vector3 offsetExtent = render.bounds.extents + OFFSET_VECTOR;
        Bounds touchBound = new Bounds(render.bounds.center, 2 * offsetExtent);

        //Get all keys in a list
        List<string> keyList = new List<string>(touching.Keys);

        //Checks the status of touching colliders. If collider destroyed or not touching anymore, remove it
        foreach(string key in keyList) {
            Collider touchedObj = touching[key];

            if (touchedObj == null || (touchedObj.tag != "Platform" && touchedObj.tag != "Block") || !touchedObj.bounds.Intersects(touchBound))
                touching.Remove(key);
        }

        //Updates condition variables as needed depending if its touching assigned obj
        grounded = touching.ContainsKey("ground");
        leftWalled = touching.ContainsKey("leftWall");
        rightWalled = touching.ContainsKey("rightWall");

        //Checks for corner bug when y velocity == 0 and y isn't fixed
        bool fixedY = (rb.constraints & RigidbodyConstraints.FreezePositionY) != RigidbodyConstraints.None;
        if (!grounded && rb.velocity.y == 0 && !fixedY)
            StartCoroutine(recalibrate());
            
    }

    //Constants for recalibarion
    private const float RECALIBRATE_HOP = 250f;
    private const int NUM_FRAMES = 3;

    //Re-calibrates collision system if bug exists by allowing the player to make a small hop if delta-Y is 0
    private IEnumerator recalibrate() {
        //Gets delta y
        float originalY = GetComponent<Transform>().position.y;

        //Allows fixed number of frames before checking the next y
        for(int i = 0; i < NUM_FRAMES; i++)
            yield return 0;

        float newY = GetComponent<Transform>().position.y;

        //Checks if there's no change in y
        if (newY == originalY && !grounded)
            GetComponent<Rigidbody>().AddForce(Vector3.up * RECALIBRATE_HOP);
    }


    //checks enter collision
    void OnCollisionEnter(Collision collision){
        setColliderData(collision);

        //If player is on a platform, grounded = true. Grounded is automatically false when object jumps
        if (withinXBound && (colliderTag == "Platform" || colliderTag == "Block") && !grounded)
            touching.Add("ground", collision.collider);

        //If object is on a wall (platform) (withinYBound && !withinXBound) and object is right of collider, leftWalled = true
        if (withinYBound && !withinXBound && objectBoundMax.x > colliderMaxBounds.x && (colliderTag == "Platform" || colliderTag == "Block") && !leftWalled)
            touching.Add("leftWall", collision.collider);

        //If object is on a wall (platform) and object is left of collider, rightWalled = true
        if (withinYBound && !withinXBound && objectBoundMin.x < colliderMinBounds.x && (colliderTag == "Platform" || colliderTag == "Block") && !rightWalled)
            touching.Add("rightWall", collision.collider);
    }

    //Sets all collision data after every collision
    private void setColliderData(Collision collision) {
        Collider collider = collision.collider;

        //Set Collider Variables
        colliderTag = collider.tag;
        colliderMaxBounds = collider.bounds.max;
        colliderMinBounds = collider.bounds.min;

        //Set Main Object variables
        objectBoundMax = render.bounds.max;
        objectBoundMin = render.bounds.min;

        //Set relationship Variables: withinXBound
        withinXBound = colliderMaxBounds.x > objectBoundMin.x + OFFSET && colliderMinBounds.x < objectBoundMax.x - OFFSET;
        withinYBound = colliderMaxBounds.y > objectBoundMin.y + OFFSET && colliderMinBounds.y < objectBoundMax.y - OFFSET;

    }
}
