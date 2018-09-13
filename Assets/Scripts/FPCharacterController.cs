using UnityEngine;

public class FPCharacterController : MonoBehaviour
{
    //Crosshair
    Rect crosshairRect;
    public Texture crosshairTexture;
    public float crosshairPercentage = 0.05f;

    [Header("Movement")]
    public float speedCam;
    public float jumpSpeed;
    public float gravity;
    public float tiltRange;
    public int jumpNum;
    public float dashLength;

    float speedMov;
    float rotV = 0;
    bool duckLock = true;
    float verVelocity;
    int jumpCount;
    float dashTime;

    [Header("Audio")]
    public AudioSource audio1;
    public AudioSource audio2;
    private float stepSpeed;

    CharacterController cc;

    void Start()
    {
        Cursor.visible = false;

        //Character Controller
        cc = GetComponent<CharacterController>();

        //Reset
        jumpCount = jumpNum;
        dashTime = dashLength;

        //Crosshair
        float crosshairSize = Screen.width * crosshairPercentage / 100;
        crosshairRect = new Rect(Screen.width / 2 - crosshairSize / 2,
                                 Screen.height / 2 - crosshairSize / 2,
                                 crosshairSize, crosshairSize);

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Mouse Movement
        float rotH = Input.GetAxis("Mouse X") * speedCam;
        rotV -= Input.GetAxis("Mouse Y") * speedCam;
        rotV = Mathf.Clamp(rotV, -tiltRange, tiltRange);
        transform.Rotate(0, rotH, 0);
        if (Camera.main != null)
        {
            UnityEngine.Camera.main.transform.localRotation = Quaternion.Euler(rotV, 0, 0);
        }

        //Movement
        float forwardSpeed = Input.GetAxis("Vertical") * speedMov;
        float sideSpeed = Input.GetAxis("Horizontal") * speedMov;

        //Slow Walk
        if (Input.GetKey(KeyCode.LeftShift) && duckLock == true)
        {
            speedMov = 4.3f;
        }
        else
        {
            speedMov = 12;
        }

        //Duck
        if (Input.GetKeyDown(KeyCode.LeftControl) && cc.isGrounded)
        {
            duckLock = false;
            Vector3 duck = new Vector3(0, -1.5f, 0);
            UnityEngine.Camera.main.transform.position = Vector3.Lerp(UnityEngine.Camera.main.transform.position, UnityEngine.Camera.main.transform.position + duck, 25f * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && duckLock == false)
        {
            duckLock = true;
            Vector3 duck = new Vector3(0, 1.5f, 0);
            UnityEngine.Camera.main.transform.position = Vector3.Lerp(UnityEngine.Camera.main.transform.position, UnityEngine.Camera.main.transform.position + duck, 25f * Time.deltaTime);
        }

        //Gravity
        verVelocity -= gravity * Time.deltaTime;
        Vector3 speed = new Vector3(sideSpeed, verVelocity, forwardSpeed);

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpNum != 0)
        {
            verVelocity = jumpSpeed;
            jumpNum -= 1;
        }
        speed = transform.rotation * speed;
        cc.Move(speed * Time.deltaTime);
        if (cc.isGrounded)
        {
            jumpNum = jumpCount;
        }

        //Dash
        /*
        if (Input.GetKey(KeyCode.F) && dashLength > 0)
        {
            dashLength -= Time.deltaTime;
            speedMov = 50f;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            dashLength = dashTime;
        }
        */
    }

    void OnGUI()
    {
        GUI.DrawTexture(crosshairRect, crosshairTexture);
    }
}
