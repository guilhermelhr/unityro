using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
    [Header(":: Refs")]
    public Camera GameCamera;
    [Header(":: User Parameters")]
    public float MouseSensitivity = 10f;
    [Header(":: Settings")]
    public float Distance = 30f;
    public float Altitude = 35f;

    [SerializeField] 
    private Transform m_Target;

    public Direction Direction;
    // Tests
    private float m_CurrentAngle;

    private void Update()
    {
        if ( Input.GetMouseButton(1))
        {
            float yaw = Input.GetAxis("Mouse X");
            m_CurrentAngle -= yaw * Time.deltaTime * MouseSensitivity;

            if (m_CurrentAngle <= 0f)
                m_CurrentAngle += Mathf.PI * 2f;
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

        float x = Mathf.Cos(m_CurrentAngle);
        float z = Mathf.Sin(m_CurrentAngle);

        Vector3 hDir = new Vector3(x, 0f, z);

        if ( m_Target != null )
        {
            pos = m_Target.transform.position;
        }

        // offset by distance
        pos -= hDir * Distance;
        pos.y += Altitude;

        GameCamera.transform.localPosition = pos;
    }

    private void UpdateCameraAngle()
    {
        GameCamera.transform.LookAt(m_Target);
    }
}