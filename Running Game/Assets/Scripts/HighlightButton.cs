using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightButton : MonoBehaviour
{
    Color startColor = Color.white;
    Color endColor = Color.cyan;
    [Range(1, 10)] float speed = 1f;

    Image img;
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        img.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
    }
}
