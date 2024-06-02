using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject[] m_turrets;
    [SerializeField] private LayerMask m_layerMaskOffset;
    [SerializeField] private LayerMask m_layerMaskPlacebleArea;
    [SerializeField] private LayerMask m_layerMaskTurret;
    private int m_currentTurretIndex = 0;
    private GameObject m_currentTurret;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_currentTurret == null)
        {
            Debug.Log("Mouse button pressed");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_layerMaskTurret))
            {
                Debug.Log("Raycast hit: " + hit.collider.name);
                if (hit.collider.TryGetComponent(out ITurret turret))
                {
                    if(Physics.Raycast(hit.collider.transform.position, Vector3.down, out RaycastHit hit2, Mathf.Infinity, m_layerMaskPlacebleArea))
                    {
                        if (hit2.transform.TryGetComponent(out PlaceableArea placeableArea))
                        {
                            placeableArea.TurretOnMe--;
                            m_currentTurret = hit.collider.gameObject;
                            hit.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 0.5f,
                                hit.transform.position.z);
                            turret.IsPlaced = false;
                            return;
                        }
                    }
                }
            }
        }
        if (m_currentTurret != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_layerMaskOffset))
            {
                Debug.Log("Current turret: " + m_currentTurret.name);
                m_currentTurret.transform.position =
                    new Vector3(hit.point.x, m_currentTurret.transform.position.y, hit.point.z);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Collider[] placeableAreas =
                    Physics.OverlapSphere(m_currentTurret.transform.position, 10f, m_layerMaskPlacebleArea);
                Collider closestPlaceableArea = null;
                float closestDistance = Mathf.Infinity;
                foreach (Collider placeableArea in placeableAreas)
                {
                    float distance = Vector3.Distance(m_currentTurret.transform.position,
                        placeableArea.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlaceableArea = placeableArea;
                    }
                }
                if (closestPlaceableArea != null)
                {
                    if(closestPlaceableArea.TryGetComponent(out PlaceableArea placeableArea))
                        if (placeableArea.TurretOnMe <= 2)
                        {
                            PlacePos(placeableArea.TurretOnMe, closestPlaceableArea);
                            placeableArea.TurretOnMe++;
                        }
                }
            }
        }
    }
    
    private void PlacePos(int turretQuantity, Collider closestPlaceableArea)
    {
        if(turretQuantity == 0)
        {
            m_currentTurret.transform.position = new Vector3(closestPlaceableArea.transform.position.x, 1f,
                closestPlaceableArea.transform.position.z);
            m_currentTurret.TryGetComponent(out ITurret turret);
            turret.IsPlaced = true;
            m_currentTurret = null;
        }
        else if(turretQuantity == 1)
        {
            m_currentTurret.transform.position = new Vector3(closestPlaceableArea.transform.position.x, 2f,
                closestPlaceableArea.transform.position.z);
            m_currentTurret.TryGetComponent(out ITurret turret);
            turret.IsPlaced = true;
            m_currentTurret = null;
        }
        else if(turretQuantity == 2)
        {
            m_currentTurret.transform.position = new Vector3(closestPlaceableArea.transform.position.x, 3f,
                closestPlaceableArea.transform.position.z);
            m_currentTurret.TryGetComponent(out ITurret turret);
            turret.IsPlaced = true;
            m_currentTurret = null;
        }
    }
    

    public void SpawnTurret(int index)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_layerMaskOffset))
        {
            Debug.Log("Spawn turret: " + m_turrets[index].name);
            GameObject newTurret = Instantiate(m_turrets[index],
                new Vector3(hit.transform.position.x, 1f, hit.transform.position.z), Quaternion.identity);
            m_currentTurret = newTurret;
        }
    }
}
