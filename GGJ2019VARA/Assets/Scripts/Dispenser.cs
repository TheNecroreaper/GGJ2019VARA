using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{

    private Queue<GameObject> nextBlocks;
    private GameObject curBlock;
    public AudioClip snapSound;
    // Start is called before the first frame update
    void Start()
    {
        nextBlocks = new Queue<GameObject>();
        foreach (Transform child in transform)
        {
            nextBlocks.Enqueue(child.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (curBlock != null)
            if (curBlock.transform.localScale.x <= 2)
                curBlock.transform.localScale += new Vector3(0.05F, 0.00625F, 0.05F);
            else
            {
                curBlock.transform.localScale = new Vector3(2, 0.25F, 2);
                curBlock = null;
            }
    }
    void dispense()
    {
        curBlock = nextBlocks.Dequeue();
        curBlock.transform.Translate(0, 0, 2);
        PlayerAction curAction = curBlock.AddComponent<PlayerAction>();
        curBlock.AddComponent<CollisionSystem>();
        AudioSource curAudSource = curBlock.AddComponent<AudioSource>();
        curAudSource.clip = snapSound;
        curAction.curDisp = gameObject.GetComponent<Dispenser>();
        curAction.snap = curAudSource;
        curAction.setVars();
        
    }
    
}
