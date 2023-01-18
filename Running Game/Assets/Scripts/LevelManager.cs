using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
   public static LevelManager Instance { set; get; }
    public  bool SHOW_COLLIDER = true;

    //level Spawning
    private const float DISTANCE_BEFORE_SPAWN = 100f;
    private const int INITIAL_SEGMENTS = 10;
    private const int INITIAL_TRANSITIONI_SEGMENTS = 2;//some empty room befor go in to the obsatcle
    private const int MAX_SEGMENTS_ON_SCREEN = 15;
    public Transform cameraContainer;
    private int amountOfActiveSegments;
    private int continuosSegments;
    private int currentSpawnz;
    private int currentLevel;
    private int y1, y2, y3;

    //list of pieces
    public List<Piece> ramps = new List<Piece>();
    public List<Piece> longBlocks = new List<Piece>();
    public List<Piece> jump = new List<Piece>();
    public List<Piece> slides = new List<Piece>();
    [HideInInspector]
    public List<Piece> pieces = new List<Piece>();// all the pieces in the pool

    //list of segments
    public List<Segment> availableSegments = new List<Segment>();
    public List<Segment> availableTransition = new List<Segment>();//break for the player
    [HideInInspector]
    public List<Segment> segments = new List<Segment>();
    // gAMEpLAY
    private bool isMoving = false;

    private void Awake() {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnz = 0;
        currentLevel = 0;
    }
    private void Start() {
        for (int i = 0; i < INITIAL_SEGMENTS; i++) {
            if (i < INITIAL_TRANSITIONI_SEGMENTS)
                SpawnTransition();
            else
            GenerateSegment();
        }
    }

    private void Update() {
        if(currentSpawnz - cameraContainer.position.z< DISTANCE_BEFORE_SPAWN) {
            GenerateSegment();

        }
        if(amountOfActiveSegments >= MAX_SEGMENTS_ON_SCREEN) {
            segments[amountOfActiveSegments - 1].Despawn();//we spawn in a order that we we removre the last and wee add as new
            amountOfActiveSegments--;
        }
    }

    private void GenerateSegment() {
        SpawnSegment();

        if (Random.Range(0f, 1f) < (continuosSegments * 0.25f)) {

            continuosSegments = 0;
            SpawnTransition();
        }else

        continuosSegments++;
    }
    private void SpawnSegment() {
        List<Segment> possibleSeg = availableSegments.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleSeg.Count);

        Segment s = GetSegment(id, false);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnz;// moving the object ,spawninig in front of us

        currentSpawnz += s.lenght;//adjust how far object spanw

        amountOfActiveSegments++;
        s.Spawn();






   
    }
    void SpawnTransition() {
        List<Segment> possibleTransition = availableTransition.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleTransition.Count);

        Segment s = GetSegment(id, true);// is true cause we are pulling from transition list

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnz;// moving the object ,spawninig in front of us

        currentSpawnz += s.lenght;//adjust how far object spanw

        amountOfActiveSegments++;
        s.Spawn();

    }
    public Segment GetSegment(int id,bool transition) {
        Segment s = null;
        s = segments.Find(x => x.SegId == id && x.transition == transition && !x.gameObject.activeSelf);//searching inactive and  transition
        if (s == null) {// if there is none we spawn them
            GameObject go = Instantiate((transition) ? availableTransition[id].gameObject : availableSegments[id].gameObject) as GameObject;//depending on our boolean transition we pick  transition or segments list and  spawn as GameObject
            s = go.GetComponent<Segment>();
            s.SegId = id;
            s.transition = transition;
            segments.Insert(0, s);// we have added to our list



        } else {
            segments.Remove(s);  // we removing
            segments.Insert(0,s);// we readding ad the begin of the list  keep track add last and remove first
        }
        return s;
    }

    public Piece GetPiece(PieceType pt,int visualIndex) {//we looking for is not active and same visual index--
        Piece p = pieces.Find(x => x.type == pt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if(p == null) {
            GameObject go = null;//must be assign because we go trough and if else statement
            if (pt == PieceType.ramp)
                go = ramps[visualIndex].gameObject;
            else if (pt == PieceType.longblock)
                go = longBlocks[visualIndex].gameObject;
            else if (pt == PieceType.jump)
                go = jump[visualIndex].gameObject;
            else if (pt == PieceType.slide)  // we grab the prefab from the other script
                go = slides[visualIndex].gameObject;

            go = Instantiate(go);

            p = go.GetComponent<Piece>();
            pieces.Add(p);
        }


        return p;
    }
}
