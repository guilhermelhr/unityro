using UnityEngine;

namespace UnityRO.GameCamera
{
    [System.Serializable]
    public class CameraControlProfile
    {
        public float LerpTime = 0.5f;
        public float LerpThreshold = 0.1f;
        public float Velocity => m_Velocity * m_Ratio;

        private float m_Clock;
        private float m_Velocity;
        private float m_Ratio;
        private bool m_Changed;
        // cache
        private float m_NegativeThreshold;

        public CameraControlProfile()
        {
            m_NegativeThreshold = -LerpThreshold;
        }

        public void SetInertia(float velocity)
        {
            m_Changed = true;
            m_Velocity = velocity;
            m_Ratio = 1f;
        }

        public bool Update(float dt)
        {
            if ( m_Clock > 0f )
            {
                m_Clock -= dt;
                m_Ratio = Mathf.Clamp01(m_Clock / LerpTime);
                return true;
            }
            if ( m_Changed )
            {
                m_Changed = false;
                return true;
            }
            return false;
        }

        public void Release()
        {
            if ( m_Velocity >= LerpThreshold || m_Velocity <= m_NegativeThreshold)
            {
                m_Clock = LerpTime;
                m_Ratio = 1f;
            }
        }
    }
}