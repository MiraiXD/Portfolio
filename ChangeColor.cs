using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {

    Material m;
    Color defaultColor;

    LocalPlayerController lpc;
    Actor actor;
	// Use this for initialization
	void Start () {

        m = GetComponentInChildren<SkinnedMeshRenderer>().material;
        defaultColor = m.color;

        //actor = GetComponentInParent<LocalPlayerController>().actor;
	}
	
	// Update is called once per frame
	void Update () {        
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "shared")
        {
            
            if (m)
                m.color = Color.blue;
            else
            {
                m = GetComponentInChildren<SkinnedMeshRenderer>().material;
                defaultColor = m.color;
                m.color = Color.blue;
            }
        }
    }

    void OnTriggerExit()
    {
        m.color = defaultColor;
    }

    public void ChangeToColor(Color color)
    {
        if (m)
            m.color = color;
        else
        {
            m = GetComponentInChildren<SkinnedMeshRenderer>().material;
            defaultColor = m.color;
            m.color = color;
        }
    }
}
