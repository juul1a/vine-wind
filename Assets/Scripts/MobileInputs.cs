using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputs : MonoBehaviour
{
    // float deadZone = 10f;

    // Vector3 startPos;

    // public enum TouchState {Tap, SwipeUp, SwipeDown, SwipeRight, SwipeLeft, NoTouch};

    // TouchState currentState;

    // // Update is called once per frame
    // void Update()
    // {
    //     Touch touch = Input.GetTouch(0);
    //     if(touch.phase == TouchPhase.Began){
    //         startPos = touch.position;
    //     }
    //     if(touch.phase == TouchPhase.Moved){
    //         if(Vector3.Distance(startPos, touch.position) > deadZone){
    //             Vector3 direction = touch.position - startPos;
    //             if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
    //                 //Horizontal Movement
    //                 if(direction.x > 0){
    //                     currentState = SwipeRight;
    //                 }
    //                 else if(direction.x < 0){
    //                     currentState = SwipeLeft;
    //                 }
    //             }
    //             else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y)){
    //                 //Vertical Movement

    //             }
    //         }
    //         else{
    //             //In dead zone = "tap"
    //             currentState = Tap;
    //         }
    //     }
    //      if(touch.phase == TouchPhase.Stationary){
    //         //In dead zone = "tap"
    //         currentState = Tap;
    //      }

    //     //Touch2
    // }

    // Vector3 ScreenToWorld(Vector3 touchPos){
    //     Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);
    //     worldPos.z = 0;
    //     return worldPos;
    // }
}
