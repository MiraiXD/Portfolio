// This script moves the actual lifting platform of the elevator
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {

    float yDistance = 3.882f;    
    public float liftTime = 6f;
    public bool liftIsUp = false;
	
    public void LiftMove()
    {
        if(!liftIsUp)
        StartCoroutine(MoveCoroutine(true, liftTime));
        else
        StartCoroutine(MoveCoroutine(false, liftTime));
    }
    
    IEnumerator MoveCoroutine(bool isGoingUp, float time)
    {
        float nextYPos;
        if (isGoingUp)
            nextYPos = transform.localPosition.y + yDistance;
        else
            nextYPos = transform.localPosition.y - yDistance;

        Vector3 nextPos = new Vector3(transform.localPosition.x, nextYPos, transform.localPosition.z);
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, nextPos, elapsedTime*Time.deltaTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        print("Lift elapsed time: " + elapsedTime);
        transform.localPosition = nextPos;
        if (isGoingUp) liftIsUp = true;
        else liftIsUp = false;        
    }
    
}
