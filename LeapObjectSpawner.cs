using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using UnityEngine.Networking;

public class LeapObjectSpawner : NetworkBehaviour
{

    [SerializeField]
    private GameObject pinchDetectorLeft;
    [SerializeField]
    private GameObject pinchDetectorRight;

    //public Texture tex;

    public GameObject ballPrefab;
    GameObject go;
    private bool scaling = false;
    private float distance = 0;
    private float maxCubeSize = 0.5f; //in meters
    private float minCubeSize = 0.1f;
    private float scaleMultiplier = 0.7f;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       /* if (scaling)
        {
            distance = Vector3.Distance(pinchDetectorLeft.transform.position, pinchDetectorRight.transform.position);
            go.transform.localScale = Vector3.one * distance * scaleMultiplier;
            if (go.transform.localScale.x > maxCubeSize) go.transform.localScale = new Vector3(maxCubeSize, maxCubeSize, maxCubeSize);
            if (go.transform.localScale.x < minCubeSize) go.transform.localScale = new Vector3(minCubeSize, minCubeSize, minCubeSize);
        }
        */
    }

    public void SpawnACube()
    {
        /*scaling = true;
        distance = Vector3.Distance(pinchDetectorLeft.transform.position, pinchDetectorRight.transform.position);

        go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.GetComponent<Renderer>().material.mainTexture = tex;
        go.GetComponent<Renderer>().material.SetTextureScale(go.GetComponent<Renderer>().material.mainTexture.name, new Vector2(3.0f, 1.0f));
        go.transform.localScale = Vector3.one * distance * scaleMultiplier;
        go.tag = "shared";
        if (go.transform.localScale.x > maxCubeSize) go.transform.localScale = new Vector3(maxCubeSize, maxCubeSize, maxCubeSize);
        if (go.transform.localScale.x < minCubeSize) go.transform.localScale = new Vector3(minCubeSize, minCubeSize, minCubeSize);

        go.transform.position = (pinchDetectorRight.transform.position + pinchDetectorLeft.transform.position) / 2;
        */

        /*go = (GameObject)Instantiate(ballPrefab, transform.position, transform.rotation);
        go.AddComponent<Leap.Unity.Interaction.InteractionBehaviour>();
        NetworkServer.Spawn(go);*/

        GameObject.Find("LeapHands").GetComponent<SpawnBasketball>().Spawn();


    }


    public void DeactivatePinch()
    {
        //go.AddComponent<SphereCollider>();
        
        //go.GetComponent<Rigidbody>().useGravity = false;
    }

    public void rightPinch()
    {

    }

    public void leftPinch()
    {
    }
}
