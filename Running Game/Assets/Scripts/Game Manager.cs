using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public enum Gesture {
    left = 0,
    up = 1,
    right = 2,
    down = 3
}
public class GameManager : MonoBehaviour {

   
    public int coins = 0;
    public float time = 0;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI timeText;
    //public Texture2D[] gestureicons;
    const int nGestures = 3;
    public GameObject[] currentGestureIcons;
    public int activeTrick = -1;

    // public Transform RepeatingPath;
    public Gesture gest;
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        UpdateTime();

       

    }
    public void Coin() {
        coins += 1;
        UpdateCoinText();
    }
    public void UpdateCoinText() {

        string update = "Coin :" + coins;
        if (coins <= 0) {
            update = "";
        }
        coinText.text = update;
    }



    void UpdateTime() {
        time += Time.deltaTime;
        timeText.text = time.ToString("0");
    }


    public void EnableTrick() {


        activeTrick = Random.Range(0, nGestures);
        Gesture gest = (Gesture)activeTrick;

       
                currentGestureIcons[activeTrick].SetActive(true);

      //  if ( SwipingController.Instance.SwipeUp) 
       

        }
    public void DisableTrick() {

        if(activeTrick != -1)
        currentGestureIcons[activeTrick].SetActive(false);

        activeTrick = -1;

    }
  

        } 
      

        //currentGestureIcons.gameObject.SetActive(true);



    
    



