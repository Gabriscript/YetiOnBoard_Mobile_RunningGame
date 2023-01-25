using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMotor : MonoBehaviour {

    [Header("Audio info")]
    [SerializeField] private AudioSource SnowboardMoving;
    [SerializeField] private AudioSource Coin;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource Die;
    [SerializeField] private AudioSource Crowd;
    [SerializeField] private AudioSource MainTheme;


    [Header("Animation")]
    Animator anim;


    [Header("Move info")]
    const float LANE_DISTANCE = 3f;
    const float TURN_SPEED = 0.05F;
    CharacterController controller;
    float jumpForce = 8.0f;
    float gravity = 12f;
    float verticalVelocity;
    float speed = 15f;
    float sideSpeed = 7f;
    int desiredLane = 0;
    bool isMoving = true;
    float speedIncreaseLastTick;
    float speedIncreasTime = 5f;
    float speedIncreaseAmount = 0.5f;
    bool wasGrounded = false;


    CoinsSpawner cs;
    GameManager gm;

    void Start() {

        cs = FindObjectOfType<CoinsSpawner>();
        gm = FindObjectOfType<GameManager>();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update() {   //inputs on wich lane should be
        if (SwipingController.Instance.SwipeLeft || Input.GetKeyDown(KeyCode.LeftArrow)) {
            MoveLane(false);

        }
        if (SwipingController.Instance.SwipeRight || Input.GetKeyDown(KeyCode.RightArrow)) {
            MoveLane(true);

        }



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
        anim.SetBool("IsGrounded", IsGrounded());
        //  anim.SetTrigger("IsMoving");


        // just landed?
        if (!wasGrounded && isGrounded) {
            gm.DisableTrick();
            SnowboardMoving.Play();
        }
        if (isGrounded) {
            verticalVelocity = -0.1f;




            if (SwipingController.Instance.SwipeUp || Input.GetKeyDown(KeyCode.UpArrow)) {
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;
                jumpSound.Play();



            } else if (SwipingController.Instance.SwipeDown || Input.GetKeyDown(KeyCode.DownArrow)) {



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
        if (isMoving) {
            controller.Move(moveVector * Time.deltaTime);




            if (Time.time - speedIncreaseLastTick > speedIncreasTime) {
                speedIncreaseLastTick = Time.time;
                speed += speedIncreaseAmount;
                sideSpeed += speedIncreaseAmount;
            }




        }

        //rotate charater where is going


        Vector3 dir = controller.velocity;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);


        //if(swipe guessed)
        //play animation
        //play audio crowd cheering

        if (gravity == 0 && !isGrounded) {
           
            if (gm.activeTrick == -1) {
                gm.EnableTrick();

                anim.SetInteger("TricksIndex", Random.Range(0, 4));//when you use random range with integer your last max value random index is escluded
                anim.SetTrigger("TricksJump");



                //  var gestureInputs = new bool[] { SwipingController.Instance.SwipeLeft, SwipingController.Instance.SwipeUp, SwipingController.Instance.SwipeRight };
                //while (gestureInputs[gm.activeTrick]) {

                Crowd.PlayOneShot(Crowd.clip);

                gm.coins += 50;
                   


                   
              //  }
            }

        }
        wasGrounded = isGrounded;
     

      if(  FindObjectOfType<PauseMenu>().GameIsPaused == true) {
            SnowboardMoving.Pause();
            FindObjectOfType<glacierScript>().isScrolling = false;
        }
    }
   
    void MoveLane(bool goingRight) {
    desiredLane += (goingRight )? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, -1, 1);
    }

    bool IsGrounded() {
    
      Ray groundRay = new Ray(new Vector3(controller.bounds.center.x, (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,
            controller.bounds.center.z), Vector3.down);

        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.red, 10.0f);
        return (Physics.Raycast(groundRay, 0.2f + 0.1f));
       


    }
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Coin") {
            Coin.Play();
          other.gameObject.SetActive(false);
            gm.Coin();
        }
        if (other.gameObject.tag == "Ramp") {

            RampJump();
            Invoke("StopRampJump", 1.5f);
        }
    }
    void StartSliding() {
         anim.SetBool("IsSliding", true);
        controller.height /= 2;
         controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
      
    }
    void StopSliding() {
        anim.SetBool("IsSliding", false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
      
    }
    void Crash() {
        SnowboardMoving.Stop();
        Die.Play();
        anim.SetTrigger("Death");
        Invoke("CallGameOver", 1);
        FindObjectOfType<glacierScript>().isScrolling = false;
        isMoving = false;

    }
    public void CallGameOver() {
        FindObjectOfType<EndGame>().GameOverMenu();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        switch (hit.gameObject.tag) {
            case "Obstacle":
                Crash();
                break;
        }
    }
    void RampJump() {
        gravity *= 0f; 
        
    }
    void StopRampJump() {
        gravity = 12f;
       

    }
}
