using System;
using UnityEngine;
using UnityCamera = UnityEngine.Camera;

namespace UnityRO.Core.Camera {
    /// <summary>
    /// Ingame player camera controller
    /// </summary>
    public class CharacterCamera : MonoBehaviour {
        #region Serializables

        [Header(":: Refs")]
        public UnityCamera GameCamera;
        
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
        public float Pitch { get; private set; } = 0.7853982f;

        #endregion

        private float m_Yaw = 7.869574f;
        private float m_Altitude = 0.7071068f;
        private float m_SphereSliceRadius = 0.7071068f;
        
        // cache
        private readonly float s_PI2 = Mathf.PI * 2f;
        private Vector2 m_PitchConstraintRad = new(0.5235988f, 0.7853982f);

        //@TODO: Double right tap to reset cam

        public void SetTarget(Transform tr) {
            m_Target = tr;
        }

        private void Awake() {
            m_PitchConstraintRad = new Vector2(PitchConstraint.x, PitchConstraint.y) * Mathf.Deg2Rad;
            RecomputeCameraAngle();
        }

        private void Update() {
            var dt = Time.deltaTime;
            var shiftModifier = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            var mouseScroll = Input.mouseScrollDelta.y;

            if (Input.GetMouseButton(1)) {
                var hX = Input.GetAxis("Mouse X");
                var hY = Input.GetAxis("Mouse Y");

                if (shiftModifier) {
                    var vScroll = hY * MouseSensitivity.y + mouseScroll * ScrollPitchSensitivity;
                    if (vScroll != 0f) {
                        Pitch -= vScroll * dt;
                        RecomputeCameraAngle();
                    }
                } else {
                    YawControl.SetInertia(hX * MouseSensitivity.x);
                }
            } else if (Input.GetMouseButtonUp(1)) {
                if (!shiftModifier) {
                    YawControl.Release();
                }
            }

            if (!shiftModifier && mouseScroll != 0f) {
                ZoomControl.SetInertia(mouseScroll * ScrollZoomSensitivity);
                ZoomControl.Release();
            }

            if (YawControl.Update(dt)) {
                m_Yaw -= YawControl.Velocity * dt;
                if (m_Yaw < 0f) {
                    m_Yaw += s_PI2;
                }
                RecomputeHorizontalDirection();
            }
            
            if (ZoomControl.Update(dt)) {
                var zoomVel = ZoomControl.Velocity * dt;
                Distance = Mathf.Clamp(Distance + zoomVel, ZoomConstraint.x, ZoomConstraint.y);
            }
            
            UpdateCameraPosition();
            UpdateCameraLookAt();
        }

        private void UpdateCameraPosition() {
            var pos = Vector3.zero;
            var hDir = HorizontalDirection;

            if (m_Target != null) {
                pos = m_Target.transform.position;
            }

            hDir.y = -m_Altitude;
            pos -= hDir * Distance;
            GameCamera.transform.localPosition = pos;
        }

        private void UpdateCameraLookAt() {
            if (m_Target != null) {
                GameCamera.transform.LookAt(m_Target);
                //m_Target.transform.rotation = transform.rotation;
            }

            var angle = (float) ((m_Yaw + Math.PI / 8f) / (2f * Math.PI));

            if (angle < 0f)
                angle += 1f;

            var orientedAngle = angle - 1f / 4f;
            var direction = (int) (orientedAngle * 8) % 8;
            Direction = (Direction) direction;
        }

        private void RecomputeHorizontalDirection() {
            HorizontalDirection = new Vector3(Mathf.Cos(m_Yaw) * m_SphereSliceRadius, 0f, Mathf.Sin(m_Yaw) * m_SphereSliceRadius);
        }

        private void RecomputeCameraAngle() {
            Pitch = Mathf.Clamp(Pitch, m_PitchConstraintRad.x, m_PitchConstraintRad.y);
            m_Altitude = Mathf.Sin(Pitch);
            m_SphereSliceRadius = Mathf.Cos(Pitch);
            RecomputeHorizontalDirection();
        }
    }
}