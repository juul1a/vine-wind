using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightMovement : MonoBehaviour
{
    public Vector2 startPos, endPos;
    private Vector2 tempPos = new Vector2(0,0);
    public float timeToLerp;
    private float time;
    public float frequency = 4f;
    public float magnitude = 0.01f;

    public bool moving = false;
    private bool facingRight = false;

    private Transform spritePack;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos;
        spritePack = transform.GetChild(0);
        if(startPos.x > endPos.x){
            facingRight = false;
        }
        else{
            facingRight = true;
            Flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Bob();
        if(moving){
            Move();
        }
    }

    void Bob(){
		spritePack.localPosition = spritePack.localPosition + (spritePack.up * Mathf.Sin(Time.time * frequency) * magnitude);
	}

    void Move(){
        time += Time.deltaTime / timeToLerp;
        transform.position = Vector2.Lerp(startPos, endPos, time);
        if(time>=1.0f){
            Flip();
            tempPos = startPos;
            startPos = endPos;
            endPos = tempPos;
            time = 0f;
        }
    }

    void Flip(){
        spritePack.transform.Rotate(0f, 180f, 0f);
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.transform.parent.tag == "Player"){
            col.transform.parent.SetParent(transform);
        }
    }
    void OnTriggerExit2D(Collider2D col){
        if(col.transform.parent.tag == "Player"){
            col.transform.parent.SetParent(null);
        }
    }
}
