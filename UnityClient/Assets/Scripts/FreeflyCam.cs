using UnityEngine;

public class FreeflyCam : MonoBehaviour
{

    /*
	EXTENDED FLYCAM
		Desi Quintans (CowfaceGames.com), 17 August 2012.
		Based on FlyThrough.js by Slin (http://wiki.unity3d.com/index.php/FlyThrough), 17 May 2011.
 
	LICENSE
		Free as in speech, and free as in beer.
 
	FEATURES
		WASD/Arrows:    Movement
		          Q:    Climb
		          E:    Drop
                      Shift:    Move faster
                    Control:    Move slower
                        End:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
	*/

    public float cameraSensitivity = 40;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float speedFactor = 0.25f;
    public float fastMoveFactor = 3;

    public float posLerp = 1;
    public float rotLerp = 1;
    public float speedLerp = 1;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private Vector3 targetPos;
    private float targetSpeed;
    private float speed = 0;

    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        targetPos = transform.position;
        targetSpeed = normalMoveSpeed;
    }

    void Update() {
        rotationX = rotationX + Input.GetAxis("Mouse X") * cameraSensitivity;
        rotationY = rotationY + Input.GetAxis("Mouse Y") * cameraSensitivity;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        Quaternion targetRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        targetRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotLerp);

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        bool q = Input.GetKey(KeyCode.Q);
        bool e = Input.GetKey(KeyCode.E);
        if(v != 0 || h != 0 || q || e) {
            if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                targetSpeed += speedFactor * Time.deltaTime;

            } else if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
                targetSpeed -= speedFactor * Time.deltaTime;
            }
        }

        targetSpeed = Mathf.Clamp(targetSpeed, 0, normalMoveSpeed * fastMoveFactor);
        speed = Mathf.Lerp(speed, (v != 0 || h != 0 || q || e? targetSpeed : 0), (targetSpeed == normalMoveSpeed || targetSpeed == 0? 3 * speedLerp : speedLerp)  * Time.deltaTime);        

        targetPos += transform.forward * speed * v * Time.deltaTime;
        targetPos += transform.right * speed * h * Time.deltaTime;

        if(q) { targetPos += transform.up * speed * 0.3f * Time.deltaTime; }
        if(e) { targetPos -= transform.up * speed * 0.3f * Time.deltaTime; }
        

        transform.position = Vector3.Lerp(transform.position, targetPos, posLerp);
    }
}