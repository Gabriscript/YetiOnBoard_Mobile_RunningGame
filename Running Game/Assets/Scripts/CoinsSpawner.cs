using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    public int maxCoin = 5;
    public float chanceToSpawn = 0.5f;
    public bool forceSpawnAll = false;

    GameManager gm;

    private GameObject[] coins;

    private void Awake() {
        coins = new GameObject[transform.childCount];//the amount of children we gonna have
        for (int i = 0; i < transform.childCount; i++) {//grab everyone of them
            coins[i] = transform.GetChild(i).gameObject;

        }
        OnDisable();

        gm = FindObjectOfType<GameManager>();
    }

    private void OnEnable() {
        if (Random.Range(0f, 1f) > chanceToSpawn)
            return;

        if (forceSpawnAll) {
            for (int i = 0; i < maxCoin; i++) {
                coins[i].SetActive(true);
            }
           


        } else {
            int r = Random.Range(0, maxCoin);//amount of random coins spawn
                for (int i = 0; i < r; i++) {
                coins[i].SetActive(true);

            }
        }
        
    }
   

    public void OnDisable() {

        foreach (GameObject go in coins)
            if(go != null)
            go.SetActive(false);
    }
}
