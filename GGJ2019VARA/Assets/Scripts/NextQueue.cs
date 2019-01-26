using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextQueue : MonoBehaviour
{
    private int queuePos;
    private static int size;

    private static ArrayList allStackable;
    // Start is called before the first frame update
    void Start()
    {
        allStackable.Add(this);
    }

    private void setPos()
    {
        queuePos = allStackable.Count;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("return") && PlayerAction.activePlayer == false && queuePos == 0)
        {
            Destroy(gameObject);
            foreach(NextQueue temp in allStackable)
            {
                temp.queuePos--;
            }
            PlayerAction.activePlayer = true;
        }
    }
}
