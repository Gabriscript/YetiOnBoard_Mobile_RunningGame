using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipingController : MonoBehaviour
{
    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private Vector2 startTouch, swipeDelta;
    private bool isDragging = false;
    private static SwipingController instance;
    public static SwipingController Instance { get { return instance; } }


    private void Awake() {
        instance = this;
        
    }


    private void Update() {

        tap = swipeLeft = swipeRight = swipeDown = swipeUp = false;

        // standAloneInputs
        
            if(Input.GetMouseButtonDown(0)) {
            tap = true;
          //  isDragging = true;
            startTouch = Input.mousePosition;
            
        }else if (Input.GetMouseButtonUp(0)) {
           // isDragging = false;
            Reset();
        }
        //mobile Inputs
        if (Input.touches.Length != 0) {
            if (Input.touches[0].phase == TouchPhase.Began) {
                tap = true;
                startTouch = Input.touches[0].position;
            } else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) {
               startTouch = swipeDelta = Vector2.zero;
             //   isDragging = false;
                Reset();
            }
        }

       swipeDelta = Vector2.zero;
        if (startTouch  != Vector2.zero) {
            if (Input.touches.Length != 0) {
                swipeDelta = Input.touches[0].position - startTouch;
            } else if (Input.GetMouseButton(0)) {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;


            }

            
        }
        //deadzone 
        if( swipeDelta.magnitude > 100) {

            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if(Mathf.Abs(x) > Mathf.Abs(y)) {
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;

                

            } else {
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;

            }




            Reset();
        }


    }

    private void Reset() {
        startTouch = swipeDelta = Vector2.zero;
       // isDragging = false;
    }
    public bool Tap { get { return tap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
}
