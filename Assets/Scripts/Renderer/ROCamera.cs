using System;
using UnityEngine;

public class ROCamera : MonoBehaviour {

    public static ROCamera Instance;

    private const float ZOOM_MIN = 25f;
    private const float ZOOM_MAX = 80f;
    private const float ALTITUDE_MIN = 30f;
    private const float ALTITUDE_MAX = 37f;

    [SerializeField] private Transform _target;
    [SerializeField] public static Direction direction;
    [SerializeField] private float zoom = 0f;
    [SerializeField] private float distance = 30f;
    [SerializeField] private float altitude = 35f;
    [SerializeField] private float TargetRotation = 0f;
    [SerializeField] public float Rotation = 0f;
    [SerializeField] public int Angle;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void Start() {
        HandleYawPitch();
        HandleZoom();
    }

    void LateUpdate() {
        float scrollDelta = Input.mouseScrollDelta.y;
        if (Input.GetMouseButton(1)) {
            this.TargetRotation += Input.GetAxis("Mouse X");
            HandleYawPitch();
        } else if (Input.GetKey(KeyCode.LeftShift)) {
            this.altitude = Mathf.Clamp(this.altitude + scrollDelta, ALTITUDE_MIN, ALTITUDE_MAX);
            HandleYawPitch();
        } else if (scrollDelta != 0) {
            zoom += scrollDelta;
            HandleZoom();
        }

        if (TargetRotation > 360)
            TargetRotation -= 360;
        if (TargetRotation < 0)
            TargetRotation += 360;

        if (Rotation > 360)
            Rotation -= 360;
        if (Rotation < 0)
            Rotation += 360;

        Rotation = Mathf.LerpAngle(Rotation, TargetRotation, 7.5f * Time.deltaTime);

        Angle = GetAngleDirection();
        direction = (Direction)Angle;
    }

    private int GetAngleDirection() {
        return (int)(Math.Floor((Math.Abs(TargetRotation) % 360 + 22.5f) / 45) % 8);
    }

    private void HandleYawPitch() {
        var direction = new Vector3(0, 0, -distance);
        var rotation = Quaternion.Euler(this.altitude, this.TargetRotation, 0);
        transform.position = _target.position + rotation * direction;
        transform.LookAt(_target.position);
    }

    private void Update() {

    }

    private void HandleZoom() {
        var direction = _target.position - transform.position;
        distance = direction.magnitude;

        if (zoom > 0.0f) {
            if (distance <= ZOOM_MIN) {
                zoom = 0;
                return;
            }

            zoom -= zoom / 5f;

            if (zoom <= 0f) {
                zoom = 0f;
            } else {
                direction /= distance;
                direction *= zoom;

                transform.position += direction;
            }
        } else if (zoom < 0f) {
            if (distance >= ZOOM_MAX) {
                zoom = 0;
                return;
            }

            zoom -= zoom / 5f;

            if (zoom >= 0f) {
                zoom = 0f;
            } else {
                direction /= distance;
                direction *= zoom;

                transform.position += direction;
            }
        }
    }

    public void SetTarget(Transform target) {
        this._target = target;
    }

}
