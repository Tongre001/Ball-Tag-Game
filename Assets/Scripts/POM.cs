using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class POM : MonoBehaviour {
    private void OnCollisionEnter(Collision collision)
    { 
        var pickup = collision.gameObject;
        var player = pickup.GetComponent<PlayerValues>();
        if (player != null && player.CurrentLives < 6) {
            
            player.SetChangeBallSize(0.5f);
            player.SetChangeLives(1);
            player.SetChangeSpeed(-50f);
            Destroy(gameObject);
            GameObject canvasObject = GameObject.Find("CanvasText");
            Transform TextTrans = canvasObject.transform.Find("PlayerText");
            TextTrans.GetComponent<Text>().text = "Picked up a Life";

        }        
        else if (player != null  && player.CurrentLives == 6){
            GameObject canvasObject = GameObject.Find("CanvasText");
            Transform TextTrans = canvasObject.transform.Find("PlayerText");
            TextTrans.GetComponent<Text>().text = "Max amount of Lives!";
        }
    }
        
    

}
