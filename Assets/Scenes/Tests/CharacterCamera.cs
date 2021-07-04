﻿using System;
using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
    [Header(":: Refs")]
    public Camera GameCamera;
    [Header(":: User Parameters")]
    public Vector2 MouseSensitivity = Vector2.one;
    [Header(":: Settings")]
    public float LerpTime = 0.5f;
    public float Distance = 30f;
    public Vector2 PitchConstraint;

    [SerializeField] 
    private Transform m_Target;

    public Direction Direction;
    public Vector3 HorizontalDirection { get; private set; }

    // lerp
    private float m_LerpClock;
    private float m_LastVelocity;
    // cache
    private Vector2 m_PitchConstraintRad;
    private float m_Yaw;
    private float m_Pitch;
    private float m_Altitude;
    private float m_SphereSliceRadius;

    private void Awake()
    {
        m_PitchConstraintRad = new Vector2(PitchConstraint.x, PitchConstraint.y) * Mathf.Deg2Rad;
        RecomputeCameraAngle();
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        bool isShiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if ( Input.GetMouseButton(1))
        {
            m_LerpClock = 0f;
            float hX = Input.GetAxis("Mouse X");
            float hY = Input.GetAxis("Mouse Y");

            if ( isShiftDown  )
            {
                if ( hY != 0f )
                {
                    m_Pitch -= hY * dt * MouseSensitivity.y;
                    RecomputeCameraAngle();
                }
            }
            else 
            {
                if (hX != 0f)
                {
                    m_LastVelocity = hX * MouseSensitivity.x;
                    m_Yaw -= m_LastVelocity * dt;
                    if (m_Yaw <= 0f)
                        m_Yaw += Mathf.PI * 2f;

                    RecomputeHorizontalDirection();
                }
                else
                {
                    m_LastVelocity = 0f;
                }
            }
        }
        else if ( Input.GetMouseButtonUp(1) && !isShiftDown)
        {
            if (Mathf.Abs(m_LastVelocity) >= 0.2f)
            {
                m_LerpClock = LerpTime;
            }
        }

        if(m_LerpClock > 0f )
        {
            m_LerpClock -= dt;
            float r = 1f - Mathf.Clamp01(m_LerpClock / LerpTime);

            m_Yaw -= Mathf.Lerp(m_LastVelocity, 0f, r) * dt;
            RecomputeHorizontalDirection();

            if ( m_LerpClock <= 0f )
            {
                m_LerpClock = 0f;
            }
        }
    }

    private void LateUpdate() 
    {
        UpdateCameraPosition();
        UpdateCameraAngle();
    }

    private void UpdateCameraPosition() 
    {
        Vector3 pos = Vector3.zero;
        Vector3 hDir = HorizontalDirection;

        if (m_Target != null)
        {
            pos = m_Target.transform.position;
        }

        hDir.y = -m_Altitude;
        pos -= hDir * Distance;
        GameCamera.transform.localPosition = pos;
    }

    private void UpdateCameraAngle()
    {
        if ( m_Target != null)
        {
            GameCamera.transform.LookAt(m_Target);
        }
    }

    private void RecomputeHorizontalDirection()
    {
        HorizontalDirection = new Vector3(Mathf.Cos(m_Yaw) * m_SphereSliceRadius, 0f, Mathf.Sin(m_Yaw) * m_SphereSliceRadius);
    }

    private void RecomputeCameraAngle()
    {
        m_Pitch = Mathf.Clamp(m_Pitch, m_PitchConstraintRad.x, m_PitchConstraintRad.y);
        m_Altitude = Mathf.Sin(m_Pitch);
        m_SphereSliceRadius = Mathf.Cos(m_Pitch);
        RecomputeHorizontalDirection();
    }
}