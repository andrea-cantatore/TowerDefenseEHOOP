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
    private GameObject m_currentTurret;
    [SerializeField] private int[] turretCosts = { 15, 10, 25 };
    private bool m_isPlaced = false;

    private void Update()
    {
        if (m_currentTurret != null && Input.GetMouseButtonDown(1))
        {
            m_currentTurret.TryGetComponent(out ITurret turret);
            int cost = turret.cost;
            Destroy(m_currentTurret);
            m_currentTurret = null;
            EventManager.ChangeCoins?.Invoke(cost);
            return;
        }
        if (Input.GetMouseButtonDown(0) && m_currentTurret == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_layerMaskTurret))
            {
                if (hit.collider.TryGetComponent(out ITurret turret))
                {
                    if (Physics.Raycast(hit.collider.transform.position, Vector3.down, out RaycastHit hit2,
                            Mathf.Infinity, m_layerMaskPlacebleArea))
                    {
                        if (hit2.transform.TryGetComponent(out PlaceableArea placeableArea) && turret.slot == placeableArea.TurretOnMe)
                        {
                            placeableArea.TurretOnMe--;
                            m_currentTurret = hit.collider.gameObject;
                            hit.transform.position = new Vector3(hit.transform.position.x,
                                hit.transform.position.y + 0.5f,
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
                    if (closestPlaceableArea.TryGetComponent(out PlaceableArea placeableArea))
                        if (placeableArea.TurretOnMe <= 2)
                        {
                            PlacePos(placeableArea.TurretOnMe, closestPlaceableArea);
                            if (m_isPlaced)
                            {
                                placeableArea.TurretOnMe++;
                                m_isPlaced = false;
                            }
                        }
                }
            }
        }
    }

    private void PlacePos(int turretQuantity, Collider closestPlaceableArea)
    {
        if (m_currentTurret.TryGetComponent(out IPowerUp powerUp) && turretQuantity <= 0)
        {
            Debug.Log("rip");
            return;
        }
        m_currentTurret.TryGetComponent(out ITurret turret);
        if (turretQuantity == 0)
        {
            m_currentTurret.transform.position = new Vector3(closestPlaceableArea.transform.position.x, 1f,
                closestPlaceableArea.transform.position.z);
            turret.IsPlaced = true;
            m_isPlaced = true;
            m_currentTurret = null;
        }

        else if (turretQuantity == 1)
        {
            m_currentTurret.transform.position = new Vector3(closestPlaceableArea.transform.position.x, 2f,
                closestPlaceableArea.transform.position.z);
            turret.IsPlaced = true;
            m_isPlaced = true;
            m_currentTurret = null;
        }
        else if (turretQuantity == 2)
        {
            m_currentTurret.transform.position = new Vector3(closestPlaceableArea.transform.position.x, 3f,
                closestPlaceableArea.transform.position.z);
            turret.IsPlaced = true;
            m_isPlaced = true;
            m_currentTurret = null;
        }
        turret.slot = turretQuantity + 1;
    }


    public void SpawnTurret(int index)
    {
        if (index >= m_turrets.Length)
            return;
        if (GameManager.instance.CoinCount < turretCosts[index])
            return;
        if (m_currentTurret != null)
        {
            Destroy(m_currentTurret);
            m_currentTurret = null;
            EventManager.ChangeCoins?.Invoke(turretCosts[index]);
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_layerMaskOffset))
        {
            Debug.Log("Spawn turret: " + m_turrets[index].name);
            GameObject newTurret = Instantiate(m_turrets[index],
                new Vector3(hit.transform.position.x, 1f, hit.transform.position.z), Quaternion.identity);
            m_currentTurret = newTurret;
        }

        EventManager.ChangeCoins?.Invoke(-turretCosts[index]);
    }
}
