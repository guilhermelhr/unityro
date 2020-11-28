using System;
using UnityEngine;

public class ROCamera : MonoBehaviour {

    public enum DIRECTION {
        NORTH,
        NORTHEAST,
        EAST,
        SOUTHEAST,
        SOUTH,
        SOUTHWEST,
        WEST,
        NORTHWEST
    }

    private const float ZOOM_MIN = 12f;
    private const float ZOOM_MAX = 60f;
    private const float ALTITUDE_MIN = 10f;
    private const float ALTITUDE_MAX = 35f;
    private const float ROTATION_MIN = -360f;
    private const float ROTATION_MAX = 360f;

    [SerializeField] private Transform _target;
    [SerializeField] private DIRECTION pointDirection;
    [SerializeField] private float zoom = 0f;
    [SerializeField] private float distance = 30f;
    [SerializeField] private float altitude = ALTITUDE_MAX;
    [SerializeField] private float rotation = 0f;
    [SerializeField] private float angle;

    private void Start() {
        HandleYawPitch();
        HandleZoom();
    }

    void LateUpdate() {
        float scrollDelta = Input.mouseScrollDelta.y;
        if (Input.GetMouseButton(1)) {
            this.rotation = Mathf.Clamp(this.rotation + Input.GetAxis("Mouse X"), ROTATION_MIN, ROTATION_MAX);
            HandleYawPitch();
        } else if (Input.GetKey(KeyCode.LeftShift)) {
            this.altitude = Mathf.Clamp(this.altitude + scrollDelta, ALTITUDE_MIN, ALTITUDE_MAX);
            HandleYawPitch();
        } else if (scrollDelta != 0) {
            zoom += scrollDelta;
            HandleZoom();
        }

        pointDirection = GetDirection();
    }

    private void HandleYawPitch() {
        var direction = new Vector3(0, 0, -distance);
        var rotation = Quaternion.Euler(this.altitude, this.rotation, 0);
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
    
    public DIRECTION GetDirection() {
        var direction = _target.position - transform.position;
        direction.x = 0;
        direction /= direction.magnitude;

        float a_cos = Vector3.Dot(direction, Vector3.back);
        angle = (float)(Math.Acos(a_cos) * 180 / Math.PI);

        if (direction.x < 0) {
            angle = 360 - angle;
        }

        if (angle <= 22.5)
            return DIRECTION.NORTH;
        else if (angle <= 67.5)
            return DIRECTION.NORTHEAST;
        else if (angle <= 112.5)
            return DIRECTION.EAST;
        else if (angle <= 157.5)
            return DIRECTION.SOUTHEAST;
        else if (angle <= 202.5)
            return DIRECTION.SOUTH;
        else if (angle <= 247.5)
            return DIRECTION.SOUTHWEST;
        else if (angle <= 292.5)
            return DIRECTION.WEST;
        else if (angle <= 337.5)
            return DIRECTION.NORTHWEST;
        else
            return DIRECTION.NORTH;
    }

}
