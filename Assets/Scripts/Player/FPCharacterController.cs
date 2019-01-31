using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class FPCharacterController : MonoBehaviour
{
    //Crosshair
    Rect crosshairRect;
    public Texture crosshairTexture;
    public Text gameOverText;
    public float crosshairPercentage = 0.05f;

    [Header("Movement")]
    public float speedCam;
    public float speedMov;
    public float speedMovSlow;
    public float jumpSpeed;
    public float gravity;
    public float tiltRange;
    public int jumpNum;
    public float dashLength;
    public bool aimingGun;
    public bool dead;

    public float forwardSpeed;
    public float sideSpeed;

    public Vector3 startPos;

    float speedMovSave;
    float rotV = 0;
    float rotHKeyboard = 0;
    bool duckLock = true;
    float verVelocity;
    int jumpCount;
    float dashTime;

    [Header("Audio")]
    public AudioSource audio1;
    public AudioSource audio2;
    private float stepSpeed;

    CharacterController cc;

    ChromaticAberration chrom = null;
    Vignette vignette = null;

    void Start()
    {
        Cursor.visible = false;

        //Character Controller
        cc = GetComponent<CharacterController>();

        speedMovSave = speedMov;

        //Position
        startPos = transform.position;

        //Postprocessing
        PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out chrom);
        volume.profile.TryGetSettings(out vignette);

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
        float rotH = Input.GetAxis("Mouse X") * speedCam + rotHKeyboard;
        rotV -= Input.GetAxis("Mouse Y") * speedCam;
        rotV = Mathf.Clamp(rotV, -tiltRange, tiltRange);
        transform.Rotate(0, rotH, 0);
        if (Camera.main != null)
        {
            Camera.main.transform.localRotation = Quaternion.Euler(rotV, 0, 0);
        }
        //Keyboard-Mouse Movement
        if (Input.GetKey(KeyCode.J))
        {
            if (rotHKeyboard > -1)
            {
                rotHKeyboard -= 1.4f * speedCam;
            }
        }
        if (Input.GetKey(KeyCode.L))
        {
            if (rotHKeyboard < 1)
            {
                rotHKeyboard += 1.4f * speedCam;
            }
        }
        if (!Input.GetKey(KeyCode.L) && !Input.GetKey(KeyCode.J))
        {
            rotHKeyboard = 0;
        }
        if (Input.GetKey(KeyCode.I))
        {
            rotV -= 1.4f * speedCam;
        }
        if (Input.GetKey(KeyCode.K))
        {
            rotV += 1.4f * speedCam;
        }

        //Movement
        if (!dead)
        {
            if (aimingGun)
            {
                forwardSpeed = Input.GetAxis("Vertical") * speedMov / 6;
                sideSpeed = Input.GetAxis("Horizontal") * speedMov / 6;
            }
            else
            {
                forwardSpeed = Input.GetAxis("Vertical") * speedMov;
                sideSpeed = Input.GetAxis("Horizontal") * speedMov;
            }
        }
        else
        {
            gameOverText.gameObject.SetActive(true);
            chrom.intensity.value = 1;
            vignette.intensity.value = 1;
            forwardSpeed = 0;
            sideSpeed = 0;
        }

        //Slow Walk
        if (Input.GetKey(KeyCode.LeftShift) && duckLock == true)
        {
            speedMov = speedMovSlow;
        }
        else
        {
            speedMov = speedMovSave;
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
            if (!Input.GetKeyDown(KeyCode.Space))
            {
                verVelocity = 0;
                jumpNum = jumpCount;
            }
        }

        //Dash
        if (Input.GetKey(KeyCode.F) && dashLength > 0)
        {
            dashLength -= Time.deltaTime;
            speedMov = 50f;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            dashLength = dashTime;
        }

        //Game Over
        if (dead && Input.GetKeyDown(KeyCode.Space))
        {
            Dead();
        }

        //Restart
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(0);
        }
    }

    //Damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hitbox")
        {
            GetComponent<HealthSystem>().HealthLoss(other.GetComponent<BasicAttack>().damage);
            other.gameObject.SetActive(false);
        }
    }

    void Dead()
    {
        //UI
        gameOverText.gameObject.SetActive(false);

        //Reset Player
        transform.position = startPos;
        GetComponent<HealthSystem>().health = 100;

        //Reset Processing
        chrom.intensity.value = 0;
        vignette.intensity.value = 0;

        dead = false;
    }

    void OnGUI()
    {
        GUI.DrawTexture(crosshairRect, crosshairTexture);
    }
}
