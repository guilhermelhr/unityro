using System;
using UnityEngine;

public class ROCamera : MonoBehaviour {
    public Matrix4x4 projection;
    public Matrix4x4 modelView;
    public Matrix4x4 normalMat;

    public float zoom;
    public float zoomFinal;

    public Vector2 angle;
    public Vector2 angleFinal;

    public Vector3 position;
    public GameObject target;

    public static float MAX_ZOOM = 10;
    /// <summary>
    /// time frame in seconds within which we consider a double right click
    /// to be a request to reset zoom and rotation
    /// </summary>
    public static float RESET_ZOOM_DELTA = 0.5f;

    public float direction = 0;
    public float altitudeFrom = -50;
    public float altitudeTo = -65;
    public float rotationFrom = -360;
    public float rotationTo = 360;
    public float range = 240;

    public Action action;

    public struct Action {
        public bool active;
        public float tick;
        public float x, y;

        public Action(bool active, float tick, float x, float y) {
            this.active = active;
            this.tick = tick;
            this.x = x;
            this.y = y;
        }
    }

    public void SetTarget(GameObject target) {
        this.target = target;
    }

    public float GetLatitude() {
        return angle[0] - 180;
    }

    private void Rotate(bool active) {
        var tick = Time.time;

        if(!active) {
            action.active = false;
            return;
        }

        //reset angle and zoom if we detect double right click
        Vector2 mousePos = Conversions.GetMouseTopLeft();
        if(action.tick + RESET_ZOOM_DELTA > tick &&
            Math.Abs(action.x - mousePos.x) < 10 &&
            Math.Abs(action.y - mousePos.y) < 10) {

            if(Input.GetKeyDown(KeyCode.LeftShift)) {
                angleFinal[0] = range;
            }

            if(Input.GetKeyDown(KeyCode.LeftControl)) {
                zoomFinal = 0;
            } else {
                angleFinal[1] = 0;
            }
        }

        action.x = mousePos.x;
        action.y = mousePos.y;
        action.tick = tick;
        action.active = true;
    }

    private void ProcessMouseAction() {
        Vector2 mouse = Conversions.GetMouseTopLeft();
        //rotate Z
        if(Input.GetKey(KeyCode.LeftShift)) {
            angleFinal[0] += (mouse.y - action.y) / Screen.height * 300;
            angleFinal[0] = Math.Max(angleFinal[0], 190);
            angleFinal[0] = Math.Min(angleFinal[0], 270);
        }

        //zoom
        else if(Input.GetKey(KeyCode.LeftControl)) {
            zoomFinal -= (mouse.y - action.y) / Screen.height * 150;
            zoomFinal = Math.Min(zoomFinal, Math.Abs(altitudeTo - altitudeFrom) * MAX_ZOOM);
            zoomFinal = Math.Max(zoomFinal, 2);
        }

        //rotate
        else {
            angleFinal[1] -= (mouse.x - action.x) / Screen.width * 720;

            if(angle[1] > 180 && angleFinal[1] > 180) {
                angle[1] -= 360;
                angleFinal[1] -= 360;
            }else if(angle[1] < -180 && angleFinal[1] != 0) {
                angle[1] += 360;
                angleFinal[1] += 360;
            }

            angleFinal[1] = Math.Max(angleFinal[1], rotationFrom);
            angleFinal[1] = Math.Min(angleFinal[1], rotationTo);
        }

        action.x = mouse.x;
        action.y = mouse.y;
    }
    
    private void OnMouseWheel(float delta) {
        zoomFinal += delta;
        zoomFinal = Math.Min(zoomFinal, Math.Abs(altitudeTo - altitudeFrom) * MAX_ZOOM);
        zoomFinal = Math.Min(zoomFinal, 2);
    }

    // Use this for initialization
    void Start() {
        projection = new Matrix4x4();
        modelView = new Matrix4x4();
        normalMat = new Matrix4x4();
        zoomFinal = zoom = PlayerPrefs.GetFloat("Camera.zoom", 50f);
        angle = new Vector2();
        angleFinal = new Vector2();
        position = new Vector3();

        action = new Action(false, 0, 0, 0);
    }


    // Update is called once per frame
    void Update() {
        if(Input.GetMouseButtonDown(1)) {
            Rotate(true);
        }

        if(Input.GetMouseButtonUp(1)) {
            Rotate(false);
        }

        float scrollDelta = Input.mouseScrollDelta.y;
        if(scrollDelta > 0) {
            OnMouseWheel(scrollDelta / 8);
        }

        //update camera from mouse movement
        if(action.x != -1 && action.y != -1 && action.active) {
            ProcessMouseAction();
        }

        //move camera
        if(target != null) {
            Vector3 targetPos = target.transform.position;
            if(PlayerPrefs.GetInt("Camera.smooth", 1) != 0) {
                position[0] = Mathf.Lerp(position[0], -targetPos[0], Time.deltaTime);
                position[1] = Mathf.Lerp(position[1], -targetPos[1], Time.deltaTime);
                position[2] = Mathf.Lerp(position[2], targetPos[2], Time.deltaTime);
            } else {
                position[0] = -targetPos[0];
                position[1] = -targetPos[1];
                position[2] = targetPos[2];
            }
        }

        //zoom
        zoom = Mathf.Lerp(zoom, zoomFinal, 2 * Time.deltaTime);

        //angle
        Vector2.Lerp(angle, angleFinal, 2 * Time.deltaTime);
        angle[0] %= 360;
        angle[1] %= 360;

        //find camera direction (for npc direction)
        direction = (float) Math.Floor((angle[1] + 22.5f) / 45) % 8;

        //calculate new modelView matrix
        // HACK
        var matrix = modelView = Matrix4x4.identity;
        Conversions.TranslateZ(matrix, (altitudeFrom - zoom) / 2, null);
        Quaternion rot = Quaternion.Euler(angle[0], angle[1], 0);
        matrix *= Matrix4x4.Rotate(rot);

        //center of the cell and inversed Y-Z axis
        Vector3 _position = new Vector3(position[0] - 0.5f, position[2], position[1] - 0.5f);
        Conversions.Translate(matrix, matrix, _position);
        Conversions.toInverseMat3(matrix, normalMat);
        normalMat = normalMat.transpose;

        //GL.
    }

}
