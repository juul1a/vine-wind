using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputs : MonoBehaviour
{
    float deadZone = 50f;

    Vector2 startPos, tapPos;

    public enum TouchState {Tap, SwipeUp, SwipeDown, SwipeRight, SwipeLeft, NoTouch};

    public TouchState currentState;

    void Awake(){
        currentState = TouchState.NoTouch;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount>0){
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began){
                startPos = touch.position;
            }
            if(touch.phase == TouchPhase.Moved){
                if(Vector2.Distance(startPos, touch.position) > deadZone){
                    Vector2 direction = new Vector2(touch.position.x - startPos.x, touch.position.y - startPos.y);
                    if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
                        //Horizontal Movement
                        if(direction.x > 0){
                            currentState = TouchState.SwipeRight;
                            // Debug.Log("Swipe Right");
                        }
                        else if(direction.x < 0){
                            currentState = TouchState.SwipeLeft;
                            // Debug.Log("Swipe Left");
                        }
                    }
                    else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y)){
                        //Vertical Movement
                        if(direction.y > 0){
                            currentState = TouchState.SwipeUp;
                            // Debug.Log("Swipe Up");
                        }
                        else if(direction.y < 0){
                            currentState = TouchState.SwipeDown;
                            // Debug.Log("Swipe Down");
                        }

                    }
                }
                // else{
                //     //In dead zone = "tap"
                //     currentState = TouchState.Tap;
                //     Debug.Log("Tap dead zone");
                // }
            }
            if(touch.phase == TouchPhase.Ended){
                if(Vector2.Distance(startPos, touch.position) <= deadZone){
                    currentState = TouchState.Tap;
                    tapPos = touch.position;
                    Debug.Log("Tap Stationary");
                }
            }
        }
        else{
            currentState = TouchState.NoTouch;
        }
    }

    public Vector3 GetTapPos(){
        return (ScreenToWorld(tapPos));
    }

    Vector3 ScreenToWorld(Vector2 touchPos){
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x,touchPos.y,0));
        worldPos.z = 0;
        return worldPos;
    }
}
