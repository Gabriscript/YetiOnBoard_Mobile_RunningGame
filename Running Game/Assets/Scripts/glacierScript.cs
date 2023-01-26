using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glacierScript : MonoBehaviour {
    //the algoritmh goona checking things behind us of us
    private const float DISTANCE_TO_RESPAWN = 1f;

    public float scrollSpeed = -0.1f;
    public float totalLenght;
    public bool isScrolling { set; get; }


    private float scrolLocation;
    private Transform playerTransform;

    private void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        isScrolling = true;
    }
    private void Update() {
        if (!isScrolling) {
            return;
        }

        scrolLocation += scrollSpeed + Time.deltaTime/2;//every frame we increament the value
        Vector3 newLocation = (playerTransform.position.z + scrolLocation) * Vector3.forward;//we calculate the next frame where will be


        transform.position = newLocation;

        if (transform.GetChild(0).transform.position.z < playerTransform.position.z - DISTANCE_TO_RESPAWN)//if the first  glacier is less then player position

                {

            transform.GetChild(0).localPosition += Vector3.forward * totalLenght;
            transform.GetChild(0).SetSiblingIndex(transform.childCount);//we push back in the list 

          transform.GetChild(0).localPosition += Vector3.forward * totalLenght;
            transform.GetChild(0).SetSiblingIndex(transform.childCount);//repeated because we gonna move two object at the same time

           
        }
    }
}

