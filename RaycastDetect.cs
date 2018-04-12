/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastDetect : MonoBehaviour {

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    public Vector3 originOffset = Vector3.zero;    
    public float maxDetectDistance = 0.2f;
    public float lineWidth = 0.05f;
    public Color lineColor = Color.red;
    public string objTag = "shared";
    LineRenderer lineRenderer;
    GameObject hitGameObject;
	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;        
        //lineRenderer.startColor = lineColor;
        //lineRenderer.endColor = lineColor;
        lineRenderer.material.color = lineColor;
        lineRenderer.enabled = true;
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 origin = transform.position - originOffset;
        Ray ray = new Ray(origin, transform.forward);
        RaycastHit hitInfo;
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, origin + transform.forward * maxDetectDistance);
        if(Physics.Raycast(ray, out hitInfo, maxDetectDistance))
        {            
            if(hitInfo.collider.tag == objTag)
            {
                print("HIT A SHARED OBJ");
                
                if (hitGameObject != hitInfo.collider.gameObject)
                {
                    hitGameObject = hitInfo.collider.gameObject;
                    print("ASKING FOR AUTHORITY");
                    // Ask for authority
                    //hitInfo.collider.GetComponent<AuthorityManager>().grabbedByPlayer = true;
                    hitGameObject.GetComponent<AuthorityManager>().grabbedByPlayer = true;
                }
            }
            
        }
        else
        {
            print("nothiutting");
            if(hitGameObject != null)
            {
                if(hitGameObject.tag == objTag)
                {
                    // Return authority
                    print("RETURNING AUTHORITY");
                    
                    //hitGameObject.GetComponent<AuthorityManager>().grabbedByPlayer = false;
                    hitGameObject = null;

                }
            }
        }
        
	}
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastDetect : MonoBehaviour
{

    public Vector3 originOffset = Vector3.zero;
    public float maxDetectDistance = 0.2f;
    public float lineWidth = 0.05f;
    public Color lineColor = Color.red;
    public string objTag = "shared";
    LineRenderer lineRenderer;
    GameObject hitGameObject;
    public bool insideASharedObj = false;
    public bool hasAuthority = false;

    public enum WhichController { Left, Right };
    public WhichController whichController;
    public SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObject.index); } }
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    // Use this for initialization
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
        //lineRenderer.startColor = lineColor;
        //lineRenderer.endColor = lineColor;
        lineRenderer.material.color = lineColor;
        lineRenderer.enabled = true;
    }

    // Update is called once per frame
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
                print("HIT A SHARED OBJ");
                //if (hitGameObject != hitInfo.collider.gameObject)
                //{
                    insideASharedObj = true;

               
                //}

            }

            if (insideASharedObj)
            {
                if (device.GetPressDown(triggerButton))
                {
                    hitGameObject = hitInfo.collider.gameObject;
                    print("ASKING FOR AUTHORITY");
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

        

        /*if (hasAuthority)
        {
            if (device.GetPressUp(triggerButton))
            {
                if (hitGameObject != null)
                {
                    if (hitGameObject.tag == objTag)
                    {
                        // Return authority
                        print("RETURNING AUTHORITY");
                        hasAuthority = false;
                        hitGameObject.GetComponent<AuthorityManager>().grabbedByPlayer = false;
                        hitGameObject = null;
                    }
                }
            }
        }*/
    }
}
