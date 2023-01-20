using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    public KeyCode keyToPress;
    GameManager gm;
    PlayerMotor player;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        player = FindObjectOfType<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress) && gameObject.CompareTag("Left")) {

           
                gm.DisableTrick();
            
        }
    }
}
