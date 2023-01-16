using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    public int coins = 0;
    public float time = 0;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI timeText;
    public Transform RepeatingPath;
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
}


