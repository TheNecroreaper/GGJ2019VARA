using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraOver : MonoBehaviour
{
    public Dispenser;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //what to pass : Camera.main.GetComponent(CameraMover).MoveToTarget(theGameObject.transform);
    IEnumerator MoveToTarget(Transform target)
    {
        Vector3 sourcePos = transform.position;
        Vector3 destPos = target.position - transform.forward * viewDistance;
        float i = 0.0f;
        while (i < 1.0f)
        {
            transform.position = Vector3.Lerp(sourcePos, destPos, Mathf.SmoothStep(0, 1, i));
            i += Time.deltaTime;
            yield return 0;
        }
    }


}
