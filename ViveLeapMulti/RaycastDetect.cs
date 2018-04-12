// This script detects a shared object has been touched
//For some reason, colliders don't work here, so this is a workaround
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastDetect : MonoBehaviour
{
    //Offset of the detecting point, from the center of the controller
    public Vector3 originOffset = Vector3.zero;
    // Max distance an object can detected at
    public float maxDetectDistance = 0.2f;
    // Only for visualizing the ray
    LineRenderer lineRenderer;
    // Width of the line renderer 
    public float lineWidth = 0.05f;
    // COlor of the line
    public Color lineColor = Color.red;
    // Tag of the shared object
    public string objTag = "shared";
    // GO hit by the ray
    GameObject hitGameObject;
    // Is the controller inside a shared obj?
    public bool insideASharedObj = false;
    // Do I have authority over this object?
    public bool hasAuthority = false;
    // On which controller does this script sit?
    public enum WhichController { Left, Right };
    public WhichController whichController;
    public SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObject.index); } }
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    
    void Start()
    {
        if (whichController == WhichController.Left)
        {
            trackedObject = GameObject.Find("VRTK/VRTK_SDKSetup/PlayerController/Controller (left)").GetComponent<SteamVR_TrackedObject>();
        }
        else if (whichController == WhichController.Right)
        {
            trackedObject = GameObject.Find("VRTK/VRTK_SDKSetup/PlayerController/Controller (right)").GetComponent<SteamVR_TrackedObject>();
        }

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;        
        lineRenderer.material.color = lineColor;
        lineRenderer.enabled = true;
    }
   
    void Update()
    {
        Vector3 origin = transform.position - originOffset;
        Ray ray = new Ray(origin, transform.forward);
        RaycastHit hitInfo;
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, origin + transform.forward * maxDetectDistance);

        if (Physics.Raycast(ray, out hitInfo, maxDetectDistance))
        {
            if (hitInfo.collider.tag == objTag)
            {                
                    insideASharedObj = true;                
            }

            if (insideASharedObj)
            {
                if (device.GetPressDown(triggerButton))
                {
                    hitGameObject = hitInfo.collider.gameObject;                    
                    // Ask for authority                            
                    hitGameObject.GetComponent<AuthorityManager>().grabbedByPlayer = true;
                    hasAuthority = true;
                }
            }            
        }
        else
        {
            if(!hasAuthority)
            insideASharedObj = false;
        }        
    }
}
