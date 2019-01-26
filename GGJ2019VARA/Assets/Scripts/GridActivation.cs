using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridActivation : MonoBehaviour {

    //Highlights block when activated
    private void selectUnit() {
        GetComponent<MeshRenderer>().enabled = true;
    }

    private void deselectUnit() {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
