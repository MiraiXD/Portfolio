using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LiftButton : MonoBehaviour {
   
    public bool buttonPressed = false;
    // Distance in x axis to move by pressing
    float xDistance = -0.1f;
    public float pressTime = 1f;
    public IEnumerator currentCoroutine;
    public Lift lift;
	// Use this for initialization
	
    public void PressButton()
    {        
        buttonPressed = true;
        currentCoroutine = ButtonCoroutine(true, pressTime);
        StartCoroutine(currentCoroutine);
    }

    public void UnpressButton()
    {
        buttonPressed = false;
        currentCoroutine = ButtonCoroutine(false, pressTime);
        StartCoroutine(currentCoroutine);
    }

    IEnumerator ButtonCoroutine(bool isBeingPressed, float time)
    {
        float nextXPos;
        if (isBeingPressed)
            nextXPos = transform.localPosition.x + xDistance;
        else
            nextXPos = transform.localPosition.x - xDistance;
        
        Vector3 nextPos = new Vector3(nextXPos, transform.localPosition.y, transform.localPosition.z);
        float elapsedTime = 0;
        while(elapsedTime < time)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, nextPos, elapsedTime*Time.deltaTime / time);
            elapsedTime += Time.deltaTime;            
            yield return null;
        }
        print("Button elapsed time: " + elapsedTime);
        transform.localPosition = nextPos;
        currentCoroutine = null;
        
    }
    
}
