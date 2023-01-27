using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchoffexpainbutton : MonoBehaviour
{
    public float time;
    public GameObject explainmenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        Invoke("SwitchOffExplaination", 2f);
    }
    public void SwitchOffExplaination() {

       
            explainmenu.SetActive(false);
    }
}
