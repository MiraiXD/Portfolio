// This script changes the color of a touched shared object
using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {
    // Material of the object
    Material m;
    // Its default color
    Color defaultColor;

    LocalPlayerController lpc;
    Actor actor;
	
	void Start () {
        m = GetComponentInChildren<SkinnedMeshRenderer>().material;
        defaultColor = m.color;        
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
