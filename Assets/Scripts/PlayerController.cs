using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerController : NetworkBehaviour {

    public static float speed = 500f;

    private Rigidbody rb;

    //cameraTransform
    private Camera CamTransform;
    Vector3 camF;
    Vector3 camR;

    //Input
    public Vector2 input;
    //Handlers trigger for item
    public GameObject item;
    public Transform itemspawn;

    //Text 
    GameObject canvasObject;
    Transform TextTrans;

    //Timer
    [SerializeField]
    private float elapsed;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        canvasObject = GameObject.Find("CanvasText");
        TextTrans = canvasObject.transform.Find("PlayerText");
    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer) { return; }

        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        input = new Vector2(moveH, moveV);
        input = Vector2.ClampMagnitude(input, 1);

        //GetCamera Direction
        Vector3 camF = Camera.main.transform.forward;
        Vector3 camR = Camera.main.transform.right;

        camF.y = 0;
        camR.y = 0;

        camF = camF.normalized;
        camR = camR.normalized;

        elapsed += 1f * Time.timeScale;

        if (Input.GetKeyDown(KeyCode.R)) {
            CmdBrake();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CmdDropPOM();
        }

        Debug.Log(elapsed);
        if (elapsed % 200 == 0)
        {
            TextTrans.GetComponent<Text>().text = "";
        }


        rb.AddForce((camF * input.y + camR * input.x) * Time.deltaTime * speed);
        
        //after an amount of time cancel command
    }

    [Command]
    void CmdBrake() {
        var player = GetComponent<PlayerValues>();
        player.SetChangeSpeed(-100);
        speed -= 100;
    }

    [Command]
    void CmdDropPOM() {
        var player = GetComponent<PlayerValues>();

        
        if (player.CurrentLives > 0)
        {
            elapsed = 50;
            GameObject PoM = (GameObject)Instantiate(item, itemspawn.position, itemspawn.rotation);
            PoM.GetComponent<Rigidbody>().velocity = PoM.transform.forward * 1;
            //Spawn in clients
            NetworkServer.Spawn(PoM);
            //start Time
            
            //Changes Player variables
            player.SetChangeSpeed(50);
            player.SetChangeLives(-1);
            player.SetChangeBallSize(-0.5f);
            //speedup
            speed += 50;
   
            TextTrans.GetComponent<Text>().text = "Dropped a Life";

        }
    }

    //Collisions with other Players
    private void OnCollisionEnter(Collision collision)
    {
        //Game Player is destroyed/ GameOver
        if ((collision.gameObject.CompareTag("Player")) &&
            (collision.gameObject.transform.localScale.x >
            gameObject.transform.localScale.x))
        {
            elapsed = 50;
            Destroy(gameObject);
            TextTrans.GetComponent<Text>().text = "You gained a Life";
        }

        //Enemy Player is destroyed/ Extra life
        else if ((collision.gameObject.CompareTag("Player")) &&
            (collision.gameObject.transform.localScale.x <
            gameObject.transform.localScale.x))
        {
            elapsed = 50;
            Destroy(collision.gameObject);
            TextTrans.GetComponent<Text>().text = "Game Over!";
        }
        //if it is the same size
        else if((collision.gameObject.CompareTag("Player")) &&
            (collision.gameObject.transform.localScale.x ==
            gameObject.transform.localScale.x))
        {
            elapsed = 50;
            TextTrans.GetComponent<Text>().text = "Same Size: Can't Eat";}
    }

    //Collision out of bounds
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Destroy") 
        {
            elapsed = 50;
            Destroy(gameObject);
            TextTrans.GetComponent<Text>().text = "Game Over!";
        }
 
    }



    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<ThirdPersonCam>().setTarget(gameObject.gameObject);
        GetComponent<MeshRenderer>().material.color = Color.blue;
        //base.OnStartLocalPlayer();
    }

}
