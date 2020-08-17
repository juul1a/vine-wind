using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    // public int numBones = 7;
    // private GameObject[] bones;

    // void Awake(){
    //     bones = new GameObject[numBones];
    //     Transform[] allMyChildren = gameObject.GetComponentsInChildren<Transform>();
    //     int i = 0;
    //     foreach(Transform myChild in allMyChildren){
    //         if(myChild.gameObject.name.Contains("bone")){
    //             bones[i] = myChild.gameObject;
    //             i++;
    //         }
    //     }
    // }

    // void OnTriggerEnter2D(Collider2D col){
    //     Debug.Log("ENTERED ROPE TRIGGER "+col.name);
    //     if(col.tag == "Player"){
    //         CaterPlayer cp = col.gameObject.GetComponent<CaterPlayer>();
    //         cp.Attach(GetNearestBone(col.gameObject.transform.position));
    //     }
    // }

    // public Rigidbody2D GetNearestBone(Vector3 pos){
    //     GameObject closest = null;
    //     float closestDist = 1000f;
    //     foreach(GameObject bone in bones){
    //         float currDist = Vector3.Distance(pos, bone.transform.position);
    //         if(currDist<closestDist){
    //             closest = bone;
    //             closestDist = currDist;
    //         }
    //     }
    //     return closest.GetComponent<RigidBody2D>();
    // }
}
