using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class PlayerValues : NetworkBehaviour {
    [SyncVar(hook ="SetChangeBallSize")]
    public float CurrentBallSize;
    [SyncVar(hook ="SetChangeLives")]
    public int CurrentLives;
    [SyncVar(hook ="SetChangeSpeed")]
    public float CurrentSpeed;


    // Use this for initialization
    void Start () {
        CurrentBallSize = 1f;
        CurrentLives = 1;
        CurrentSpeed = 500f;
                GameObject canvasObject = GameObject.Find("CanvasText");
        Transform SpeedTextTrans = canvasObject.transform.Find("SpeedText");
        SpeedTextTrans.GetComponent<Text>().text = CurrentSpeed.ToString(); 
	}
	
	// Update is called once per frame
	public void Update () {
        
	}

    public void SetChangeBallSize(float size)
    {
        CurrentBallSize += size;
        transform.localScale = new Vector3(CurrentBallSize,
            CurrentBallSize, CurrentBallSize);
    }

    private void BallSize()
    {
        transform.localScale = new Vector3();
    }

    public void SetChangeSpeed(float speed)
    {
        CurrentSpeed += speed;
        SetSpeedText();
    }


    public void SetChangeLives(int lives)
    {
        CurrentLives += lives;
        SetLivesText();
    }

    public void SetLivesText() {
        GameObject canvasObject = GameObject.Find("CanvasText");
        Transform LifeTextTrans = canvasObject.transform.Find("LivesText");
        LifeTextTrans.GetComponent<Text>().text = "x" + CurrentLives.ToString();
    }

    public void SetSpeedText() {
        GameObject canvasObject = GameObject.Find("CanvasText");
        Transform SpeedTextTrans = canvasObject.transform.Find("SpeedText");
        SpeedTextTrans.GetComponent<Text>().text = CurrentSpeed.ToString(); 
    }

}
