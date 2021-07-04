using System;
using UnityEngine;

namespace UnityRO.GameCamera
{
    /// <summary>
    /// Ingame player camera controller
    /// </summary>
    public class CharacterCamera : MonoBehaviour
    {
        [Header(":: Refs")]
        public Camera GameCamera;
        [Header(":: User Parameters")]
        public Vector2 MouseSensitivity = Vector2.one;
        public float ScrollPitchSensitivity = 1f;
        public float ScrollZoomSensitivity = 1f;
        [Header(":: Settings")]
        public CameraControlProfile YawControl;
        public CameraControlProfile ZoomControl;

        public float LerpTime = 0.5f;
        public float Distance = 30f;
        public Vector2 ZoomConstraint;
        public Vector2 PitchConstraint;

        [SerializeField] 
        private Transform m_Target;

        public Direction Direction;
        public Vector3 HorizontalDirection { get; private set; }

        private float m_Yaw;
        private float m_Pitch;
        private float m_Altitude;
        private float m_SphereSliceRadius;
        // cache
        private readonly float s_PI2 = Mathf.PI * 2f;
        private Vector2 m_PitchConstraintRad;

        //@TODO: Double right tap to reset cam

        public void SetTarget(Transform tr)
        {
            m_Target = tr;
        }

        private void Awake()
        {
            m_PitchConstraintRad = new Vector2(PitchConstraint.x, PitchConstraint.y) * Mathf.Deg2Rad;
            RecomputeCameraAngle();
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            bool shiftModifier = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float mouseScroll = Input.mouseScrollDelta.y;

            if ( Input.GetMouseButton(1))
            {
                float hX = Input.GetAxis("Mouse X");
                float hY = Input.GetAxis("Mouse Y");

                if ( shiftModifier  )
                {
                    float vScroll =  hY * MouseSensitivity.y + mouseScroll * ScrollPitchSensitivity;
                    if (vScroll != 0f )
                    {
                        m_Pitch -= vScroll * dt;
                        RecomputeCameraAngle();
                    }
                }
                else 
                {
                    YawControl.SetInertia(hX * MouseSensitivity.x);
                }
            }
            else if ( Input.GetMouseButtonUp(1))
            {
                if ( !shiftModifier )
                {
                    YawControl.Release();
                }
            }

            if ( !shiftModifier && mouseScroll != 0f )
            {
                ZoomControl.SetInertia(mouseScroll * ScrollZoomSensitivity);
                ZoomControl.Release();
            }

            if ( YawControl.Update(dt) )
            {
                m_Yaw -= YawControl.Velocity * dt;
                if (m_Yaw < 0f)
                {
                    m_Yaw += s_PI2;
                }
                RecomputeHorizontalDirection();
            }
            if (ZoomControl.Update(dt))
            {
                float zoomVel = ZoomControl.Velocity * dt;
                Distance = Mathf.Clamp(Distance + zoomVel, ZoomConstraint.x, ZoomConstraint.y);
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
}
