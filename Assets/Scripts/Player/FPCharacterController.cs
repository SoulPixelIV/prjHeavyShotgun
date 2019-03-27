using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using TMPro;

public class FPCharacterController : MonoBehaviour
{
    //Crosshair
    Rect crosshairRect;
    public Texture crosshairTexture;
    public float crosshairScalePercentage = 0.05f;

    [Header("Movement")]
    public float speedCam;
    public float speedMov;
    public float speedMovSlow;
    public float jumpSpeed;
    public float gravity;
    public float tiltRange;
    public int jumpNum;
    public int medSyringes;
    public float healTime;
    public int orbCount;
    public float dashLength;

    public Text gameOverText;
    public TextMeshProUGUI syringeTxt;
    public TextMeshProUGUI orbCountTxt;

    [HideInInspector]
    public float forwardSpeed, sideSpeed;
    [HideInInspector]
    public Vector3 startPos;
    [HideInInspector]
    public bool isMoving = false, aimingGun, dead;

    float speedMovSave;
    float rotV = 0;
    float rotHKeyboard = 0;
    bool duckLock = true;
    bool onLadder = false;
    bool isHealing;
    float verVelocity;
    int jumpCount;
    float dashTime;
    int medSyringesSave;
    float healTimeSave;
    int stepCount = 0;
    string ground;
    float ladderDelay = -1;
    bool ladderExit;

    static int spawnPlace;
    static Quaternion spawnRotation;

    [Header("Audio")]
    public AudioClip stepStone1;
    public AudioClip stepStone2;
    public AudioClip stepMetal1;
    public AudioClip stepMetal2;
    private float stepSpeed;

    CharacterController cc;
    ChromaticAberration chrom = null;
    Vignette vignette = null;
    Animator animSyringe;

    [HideInInspector]
    public GameObject[] enemies, orbs, environment, levers, poisons;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ladder" && ladderDelay == 1)
        {
            onLadder = true;
        }
        if (other.tag == "Stone")
        {
            ground = "Stone";
        }
        if (other.tag == "Metal")
        {
            ground = "Metal";
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ladder")
        {
            onLadder = false;
            ladderExit = true;
            verVelocity = jumpSpeed;
        }
        if (other.tag == "Stone")
        {
            ground = null;
        }
        if (other.tag == "Metal")
        {
            ground = null;
        }
    }

    void Start()
    {
        //Misc
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        animSyringe = transform.Find("PlayerCamera").transform.Find("GunCamera").transform.Find("Syringe").GetComponent<Animator>();
        syringeTxt.text = "Med-Syringes x" + medSyringes;

        //Character Controller
        cc = GetComponent<CharacterController>();
        speedMovSave = speedMov;

        //Position
        startPos = transform.position;
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawnpoint");
        transform.position = spawns[spawnPlace].gameObject.transform.position;
        transform.rotation = spawnRotation;

        //Reset
        jumpCount = jumpNum;
        dashTime = dashLength;
        medSyringesSave = medSyringes;
        healTimeSave = healTime;

        orbs = GameObject.FindGameObjectsWithTag("Orb");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        environment = GameObject.FindGameObjectsWithTag("Environment");
        levers = GameObject.FindGameObjectsWithTag("Lever");
        poisons = GameObject.FindGameObjectsWithTag("Poison");

        //Crosshair
        float crosshairSize = Screen.width * crosshairScalePercentage / 100;
        crosshairRect = new Rect(Screen.width / 2 - crosshairSize / 2,
                                 Screen.height / 2 - crosshairSize / 2,
                                 crosshairSize, crosshairSize);

        //Postprocessing
        PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out chrom);
        volume.profile.TryGetSettings(out vignette);
    }

    void Update()
    {
        //Mouse Movement
        float rotH = (Input.GetAxis("Mouse X") * speedCam + rotHKeyboard) * Time.deltaTime;
        rotV -= (Input.GetAxis("Mouse Y") * speedCam) * Time.deltaTime;
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
            rotV -= (1.4f * speedCam) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.K))
        {
            rotV += (1.4f * speedCam) * Time.deltaTime;
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

        //Healing
        if (Input.GetKeyDown(KeyCode.F) && medSyringes > 0 && !animSyringe.GetCurrentAnimatorStateInfo(0).IsName("healSyringe") && !dead)
        {
            Heal();
        }
        if (isHealing)
        {
            healTime -= Time.deltaTime;
        }
        if (healTime > 0 && isHealing)
        {
            speedMov = 1;
        }
        if (healTime < 0 && isHealing)
        {
            speedMov = speedMovSave;
            healTime = healTimeSave;
            isHealing = false;
            if (GetComponent<HealthSystem>().HealthGain(55) > 100)
            {
                GetComponent<HealthSystem>().health = 100;
            }
        }

        //Gravity
        if (!onLadder && ladderDelay < 0)
        {
            verVelocity -= gravity * Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate((Vector3.up * 5) * Time.deltaTime);
            }
            verVelocity = 0;
        }
        Vector3 speed = new Vector3(sideSpeed, verVelocity, forwardSpeed);
        if (sideSpeed > 4 || sideSpeed < -4 || forwardSpeed > 4 || forwardSpeed < -4)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //Ladder
        if (ladderExit)
        {
            ladderDelay -= Time.deltaTime;
        }
        if (ladderDelay < 0 && !onLadder)
        {
            ladderExit = false;
            ladderDelay = -1;
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpNum != 0 && !onLadder)
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

        //Game Over
        if (dead && Input.GetKeyDown(KeyCode.Space))
        {
            Dead();
        }

        //Restart
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(0);
        }

        //Audio
        if (!GetComponent<AudioSource>().isPlaying && isMoving && cc.isGrounded)
        {
            if (ground == "Stone")
            {
                if (stepCount == 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(stepStone1, 1);
                    stepCount = 1;
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(stepStone2, 1);
                    stepCount = 0;
                }
            }
            else if (ground == "Metal")
            {
                if (stepCount == 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(stepMetal1, 1);
                    stepCount = 1;
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(stepMetal2, 1);
                    stepCount = 0;
                }
            }
            else
            {
                if (stepCount == 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(stepStone1, 1);
                    stepCount = 1;
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(stepStone2, 1);
                    stepCount = 0;
                }
            }
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
        if (other.gameObject.tag == "Teleport")
        {
            spawnRotation = transform.rotation;
            if (other.gameObject.name == "TeleportVillageLevel")
            {
                spawnPlace = 1;
                SceneManager.LoadScene(1);
            }
            else if (other.gameObject.name == "TeleportPoisonHybridFactory")
            {
                spawnPlace = 1;
                SceneManager.LoadScene(0);
            }
        }
    }

    void Dead()
    {
        //UI
        gameOverText.gameObject.SetActive(false);

        //Reset Player
        transform.position = startPos;
        GetComponent<HealthSystem>().health = 100;

        //Reset Enemies
        for (int z = 0; z < enemies.Length; z++)
        {
            enemies[z].gameObject.GetComponent<HealthSystem>().health = enemies[z].gameObject.GetComponent<HealthSystem>().healthSave;
            enemies[z].gameObject.GetComponent<HealthSystem>().dead = false;
            enemies[z].gameObject.transform.Find("Sight").gameObject.GetComponent<SightChecking>().aggro = false;
            enemies[z].gameObject.transform.position = enemies[z].GetComponent<EnemyAI>().startPos;
            enemies[z].gameObject.transform.rotation = enemies[z].GetComponent<EnemyAI>().startRot;
            enemies[z].gameObject.GetComponent<NavMeshAgent>().speed = 0;
            enemies[z].gameObject.GetComponent<EnemyAI>().reachedInterestPoint = false;
            if (enemies[z].gameObject.GetComponent<EnemyAI>().bowActive)
            {
                enemies[z].gameObject.GetComponent<EnemyAI>().ToggleBow();
                enemies[z].gameObject.GetComponent<EnemyAI>().InvokeRepeating("BowShoot", enemies[z].gameObject.GetComponent<EnemyAI>().shootTime, enemies[z].gameObject.GetComponent<EnemyAI>().shootTime);
            }
            enemies[z].gameObject.SetActive(true);
        }
        //Reset Orbs
        for (int v = 0; v < orbs.Length; v++)
        {
            orbs[v].gameObject.SetActive(true);
        }
        //Reset Environment
        for (int x = 0; x < environment.Length; x++)
        {
            environment[x].gameObject.SetActive(true);
        }
        //Reset Levers
        for (int l = 0; l < levers.Length; l++)
        {
            levers[l].gameObject.transform.Find("LeverObject").GetComponent<PressInteract>().activated = false;
            levers[l].gameObject.transform.Find("LeverObject").GetComponent<Animator>().Play("leverReset");
        }
        //Reset Poisons
        for (int u = 0; u < poisons.Length; u++)
        {
            poisons[u].gameObject.GetComponent<PoisonMovement>().ResetPos();
        }

        //Reset Processing
        chrom.intensity.value = 0;
        vignette.intensity.value = 0;

        dead = false;
        //SceneManager.LoadScene(0);
    }

    void OnGUI()
    {
        GUI.DrawTexture(crosshairRect, crosshairTexture);
    }

    public void Heal()
    {
        medSyringes--;
        animSyringe.Play("healSyringe");
        isHealing = true;
        syringeTxt.text = "Med-Syringes x" + medSyringes;
    }
}
