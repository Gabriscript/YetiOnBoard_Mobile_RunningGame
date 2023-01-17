using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{

    //keeping trace of the spawn
    public PieceType type;
    private Piece currentPiece;

    public void Spawn() {


        int amtObj = 0;
        switch (type) {
            case PieceType.jump:
                amtObj = LevelManager.Instance.jump.Count;
                break;
            case PieceType.slide:
                amtObj = LevelManager.Instance.slides.Count;
                break;
            case PieceType.longblock:
                amtObj = LevelManager.Instance.longBlocks.Count;
                break;
            case PieceType.ramp:
                amtObj = LevelManager.Instance.ramps.Count;
                break;

        }
        // currentPiece=  //Get me a new piece from the pool
        currentPiece = LevelManager.Instance.GetPiece(type, Random.Range(0, amtObj));   
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    }
    public void Despawn() {// go back in the pool
        currentPiece.gameObject.SetActive(false);
    }

}
