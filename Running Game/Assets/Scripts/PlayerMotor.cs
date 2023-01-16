using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {
    [Header("Animation")]
   // Animator anim;


    [Header("Move info")]
    const float LANE_DISTANCE = 3f;
    const float TURN_SPEED = 0.05F;
    CharacterController controller;
    float jumpForce = 8.0f;
    float gravity = 12f;
    float verticalVelocity;
    float speed = 7f;
    float sideSpeed = 7f;
    int desiredLane = 0;

    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        controller = GetComponent<CharacterController>();
      //  anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   //inputs on wich lane should be
        if (SwipingController.Instance.SwipeLeft)
            MoveLane(false);

        if (SwipingController.Instance.SwipeRight)
            MoveLane(true);
        //calculate where should be
        
        Vector3 targetPosition = transform.position.z * Vector3.forward + Vector3.right * desiredLane * LANE_DISTANCE;

        //Vector3 targetPosition = transform.position.z * Vector3.forward;
       /* if (desiredLane == 0)
            targetPosition += Vector3.left * LANE_DISTANCE;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * LANE_DISTANCE;
       */

        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * sideSpeed;
        bool isGrounded = IsGrounded();
       // anim.SetBool("Grounded",isGrounded);
        //jump

        if (isGrounded) {
            verticalVelocity = -0.1f;
           
            if (SwipingController.Instance.SwipeUp) {
               // anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;

            }else if (SwipingController.Instance.SwipeDown) {

                StartSliding();
                Invoke("StopSliding", 1f);
            }

        } else {

            verticalVelocity -= (gravity * Time.deltaTime);
            //fastfalling mechaninc
            if (SwipingController.Instance.SwipeDown) {
                verticalVelocity = -jumpForce;
            }
        }

        moveVector.y = verticalVelocity;
        moveVector.z = speed;


        //move player
        controller.Move(moveVector * Time.deltaTime);

        //rotate charater where is going
       Vector3 dir = controller.velocity;
        dir.y = 0;
       transform.forward = Vector3.Lerp(transform.forward,dir,TURN_SPEED);




    }
    void MoveLane(bool goingRight) {
    desiredLane += (goingRight )? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, -1, 1);
    }

    bool IsGrounded() {
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x, (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,
            controller.bounds.center.z), Vector3.down);

        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.red, 1.0f);
        return (Physics.Raycast(groundRay, 0.2f + 0.1f));

        
    }
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Coin") {
            Destroy(other.gameObject);
            gm.Coin();
        }
    }
    void StartSliding() {
        //anim.setbool(sliding,true)
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
    }
    void StopSliding() {
        //anim.setbool(sliding,false)
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }
    void Crash() {
        //anim death

    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        switch (hit.gameObject.tag) {
            case "Obstacle":
                Crash();
                break;
        }
    }
}
