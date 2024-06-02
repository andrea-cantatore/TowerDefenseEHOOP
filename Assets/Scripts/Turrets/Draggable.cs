using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 m_mouseOffset;

    private Vector3 MousePositionWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        Debug.Log(m_mouseOffset);
        m_mouseOffset = transform.position - MousePositionWorld();   
    }
    
    private void OnMouseDrag()
    {
        transform.position = MousePositionWorld() + m_mouseOffset;
    }
}
