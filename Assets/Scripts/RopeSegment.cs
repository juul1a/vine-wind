using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    public GameObject connectedAbove;
    public GameObject connectedBelow;
    public bool isPlayerAttached;

    // public Transform anchor;

    void Start(){
        connectedAbove = gameObject.GetComponent<HingeJoint2D>().connectedBody.gameObject;
        RopeSegment aboveRopeSeg = connectedAbove.GetComponent<RopeSegment>();
        if(aboveRopeSeg != null){
            //Set the connection above me to know that I am below it
            aboveRopeSeg.connectedBelow = gameObject;
            float spriteBottom =  connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y;
            // Debug.Log("Setting anchor to spriteBottom = "+spriteBottom+" For sprite "+connectedAbove.GetComponent<SpriteRenderer>().sprite);
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, spriteBottom)*-1;
        }
        else{
             GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);
        }
        
    }
    public void ResetAnchor(){
        connectedAbove = gameObject.GetComponent<HingeJoint2D>().connectedBody.gameObject;
        float spriteBottom =  connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y;
        // Debug.Log("RESETTING anchor to spriteBottom = "+spriteBottom+" For sprite "+connectedAbove.GetComponent<SpriteRenderer>().sprite+" in "+connectedAbove.name);
        GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, spriteBottom)*-1;
    }

    // void OnTriggerEnter2D(Collider2D col){
    //     Debug.Log("ENTERED ROPE TRIGGER "+col.name);
    //     if(col.tag == "Player"){
    //         CaterPlayer cp = col.gameObject.GetComponent<CaterPlayer>();
    //         if(!cp.attached && cp.attachedTo != gameObject.transform.parent){
    //             cp.Attach(gameObject.GetComponent<Rigidbody2D>());
    //         }
    //     }
    // }
}
