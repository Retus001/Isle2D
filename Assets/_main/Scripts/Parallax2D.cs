using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax2D : MonoBehaviour
{
    [Range(0f, 1f)]
    public float parallaxAmount;

    private Transform m_cam;
    private Vector3 m_prevCamPos;
    private Vector3 m_targetPos;

    private void Awake()
    {
        m_cam = Camera.main.transform;
        m_prevCamPos = m_cam.position;
    }

    void Update()
    {
        Vector3 movement = m_cam.position - m_prevCamPos;
        m_prevCamPos = m_cam.position;

        if (movement == Vector3.zero) return;

        m_targetPos = new Vector3(transform.position.x + movement.x * parallaxAmount, transform.position.y, transform.position.z);
        transform.position = m_targetPos;
    }
}
