using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class FistMovement : MonoBehaviour
{

    private bool fist = false;
    private bool rotate = false;

    [SerializeField]
    GameObject hand;

    [SerializeField]
    GameObject player;

    private Vector3 firstPositionHand;


    GameObject m_mainCam;

    private void Start()
    {
        m_mainCam = player;
        firstPositionHand = Vector3.zero;
    }

    private void Update()
    {

        if (fist || rotate)
        {
            

            //Save first handPosition
            if (firstPositionHand == Vector3.zero)
            {
                firstPositionHand = hand.transform.localPosition;
            }
            //float z = hand.transform.localPosition.z - firstPositionHand.z;
            //float x = hand.transform.localPosition.x - firstPositionHand.x;

            float z = firstPositionHand.z - hand.transform.localPosition.z;
            float x = firstPositionHand.x - hand.transform.localPosition.x;

            if (fist)
            {
                //m_mainCam.transform.position = m_mainCam.transform.position + new Vector3(m_mainCam.transform.forward.x * x ,0, m_mainCam.transform.forward.z*z);
                m_mainCam.transform.position = m_mainCam.transform.position + m_mainCam.transform.forward * (z * 2);

            }

            if (rotate)
            {
                m_mainCam.transform.eulerAngles = m_mainCam.transform.eulerAngles + new Vector3(0, x * 10, 0);
            }
        }

    }

    public void ActivateFist()
    {
        fist = true;
    }

    public void DeactivateFist()
    {
        fist = false;
        firstPositionHand = Vector3.zero;
    }

    public void ActivateRotationFist()
    {
        rotate = true;
    }

    public void DeactivateRotationFist()
    {
        rotate = false;
        firstPositionHand = Vector3.zero;
    }


}
