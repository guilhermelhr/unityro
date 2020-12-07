using System;
using UnityEngine;

public class ROCamera : MonoBehaviour {

    public enum Direction : int {
        NORTH = 0,
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
    private const float ALTITUDE_MIN = 35f;
    private const float ALTITUDE_MAX = 50f;
    private const float ROTATION_MIN = -360f;
    private const float ROTATION_MAX = 360f;

    [SerializeField] private Transform _target;
    [SerializeField] public static Direction direction;
    [SerializeField] private float zoom = 0f;
    [SerializeField] private float distance = 30f;
    [SerializeField] private float altitude = ALTITUDE_MAX;
    [SerializeField] private float rotation = 0f;
    [SerializeField] private float angle;

    public void Start() {
        //HandleYawPitch();
        //HandleZoom();
    }

    void LateUpdate() {
        float scrollDelta = Input.mouseScrollDelta.y;
        if (Input.GetMouseButton(1)) {
            this.rotation += Input.GetAxis("Mouse X");
            HandleYawPitch();
        } else if (Input.GetKey(KeyCode.LeftShift)) {
            this.altitude = Mathf.Clamp(this.altitude + scrollDelta, ALTITUDE_MIN, ALTITUDE_MAX);
            HandleYawPitch();
        } else if (scrollDelta != 0) {
            zoom += scrollDelta;
            HandleZoom();
        }

        angle = GetAngleDirection();
        direction = (Direction) angle;
    }

    private float GetAngleDirection() {
        return (float)Math.Floor((Math.Abs(rotation) % 360 + 22.5f) / 45) % 8;
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
    
    public Direction GetDirection() {
        var direction = _target.position - transform.position;
        direction.y = 0;
        direction /= direction.magnitude;

        float a_cos = Vector3.Dot(direction, Vector3.forward);
        angle = (float)(Math.Acos(a_cos) * 180 / Math.PI);

        if (direction.y < 0) {
            angle = 360 - angle;
        }

        if (angle <= 22.5)
            return Direction.NORTH;
        else if (angle <= 67.5)
            return Direction.NORTHEAST;
        else if (angle <= 112.5)
            return Direction.EAST;
        else if (angle <= 157.5)
            return Direction.SOUTHEAST;
        else if (angle <= 202.5)
            return Direction.SOUTH;
        else if (angle <= 247.5)
            return Direction.SOUTHWEST;
        else if (angle <= 292.5)
            return Direction.WEST;
        else if (angle <= 337.5)
            return Direction.NORTHWEST;
        else
            return Direction.NORTH;
    }

    public void SetTarget(Transform target) {
        this._target = target;
    }

}
