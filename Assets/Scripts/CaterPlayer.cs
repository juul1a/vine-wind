using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterPlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    public float pushForce = 2f;
    public float movementSpeed = 2f;

    private HingeJoint2D hj;

    public bool attached = false;
    public bool facingRight = false;
    public Transform attachedTo;
    private GameObject disregard;
    public bool grounded = false;

    public GameObject pulleySelected;

    public GameObject swingBody, crawlBody;

    private Animator anim;

    private float countdown = 2f;
    
    private float timer = 0;

    private Vector3 startPos;

    private bool winState = false;
    private float winStateTime = 1f;
    private float winTimer = 0;

    private Vector3 camOrigin;
    private float camZoomOrigin;

    public bool startGrounded = false;

    private MobileInputs.TouchState currentState;
    private MobileInputs mobileInputs;

    void Awake(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        hj = gameObject.GetComponent<HingeJoint2D>();
        anim = crawlBody.GetComponent<Animator>();
        startPos = transform.position;
        mobileInputs = gameObject.GetComponent<MobileInputs>();
        currentState = mobileInputs.currentState;
        pulleySelected = null;
    }

    void Update()
    {

        if(StateManager.smInstance.IsPlaying()){
            if(timer <=0 && winState){ //give it a sec before we win
                WinZoom();
            }
            if(timer>0){
                timer -= Time.deltaTime;
            }
            else if(timer<0){
                disregard = null;
                timer = 0;
            }
            if(grounded){
                anim.SetFloat("Velocity",Mathf.Abs(rb.velocity.x));
            }

            //KEYS
            //GetKeyboardInputs();

            //Mobile
            if(pulleySelected == null && (currentState == MobileInputs.TouchState.SwipeUp || currentState == MobileInputs.TouchState.SwipeDown) && mobileInputs.currentState == currentState){
                //don't do the thing
                //This is to prevent the swipe up & down from executing more than one time on a swipe
            } 
            else{
                currentState = mobileInputs.currentState;
                GetMobileInputs();
            }
           
        }
    }

    private void GetMobileInputs(){
        if(currentState == MobileInputs.TouchState.SwipeLeft){
            if(attached){
                rb.AddRelativeForce(new Vector3(-1, 0,0) * pushForce);
                AudioManager.audioManager.Play("Swing");
            }
            else if(grounded){
                if(facingRight){
                    Flip();
                }
                rb.velocity = new Vector2(-1*movementSpeed, rb.velocity.y);
            }
        }
        if(currentState == MobileInputs.TouchState.SwipeRight){
            if(attached){
                rb.AddRelativeForce(new Vector3(1, 0,0) * pushForce);
                AudioManager.audioManager.Play("Swing");
            }
            else if(grounded){
                if(!facingRight){
                    Flip();
                }
                rb.velocity = new Vector2(1*movementSpeed, rb.velocity.y);
            }
        }
        if(currentState == MobileInputs.TouchState.SwipeUp){
            // Debug.Log("CRANKIN lean distance worked");
            // Debug.Log("slide up");
            if(pulleySelected != null){
                AudioManager.audioManager.Play("Crank");
                pulleySelected.GetComponent<Crank>().Rotate(-1);
            }
            else if (attached){
                Slide(1);
            }
            
        }
        if(currentState == MobileInputs.TouchState.SwipeDown){
           if(pulleySelected != null){
                AudioManager.audioManager.Play("Crank");
                pulleySelected.GetComponent<Crank>().Rotate(1);
            }
            else if (attached){
                Slide(-1);
            }
        }
        if(currentState == MobileInputs.TouchState.Tap)
        {
                Vector3 tapPos = mobileInputs.GetTapPos();
                Vector2 tapPos2D = new Vector2(tapPos.x, tapPos.y);
                RaycastHit2D hit = Physics2D.Raycast(tapPos2D, Vector2.zero);
                if(hit.collider != null && hit.transform.gameObject.tag == "Crank")
                {
                    if(pulleySelected != hit.transform.gameObject)
                    {
                        Debug.Log("Hit something that isnt what is selected already");
                        if(pulleySelected != null){
                            Debug.Log("Deselecting whats currently selected");
                             pulleySelected.GetComponent<Crank>().Deselect();
                        }
                        Debug.Log("Setting pulley to "+hit.transform.gameObject.name);
                        pulleySelected = hit.transform.gameObject;
                        pulleySelected.GetComponent<Crank>().Select();
                    }
                    else if (pulleySelected == hit.transform.gameObject){
                        Debug.Log("hit a crank and its the one we already hit so deselecting it");
                        pulleySelected.GetComponent<Crank>().Deselect();
                        pulleySelected = null;
                    }
                }
                else{
                    if(pulleySelected != null){
                         Debug.Log("tapped somewhere that had no hit so deselecting the crank");
                        pulleySelected.GetComponent<Crank>().Deselect();
                        pulleySelected = null;
                    }
                    if(attached){
                        AudioManager.audioManager.Play("Detach");
                        Detach();
                    }
                }
        }
    }

    private void GetKeyboardInputs(){
         if(Input.GetKey("a")|| Input.GetKey("left")){
                if(attached){
                    rb.AddRelativeForce(new Vector3(-1, 0,0) * pushForce);
                }
                else if(grounded){
                    if(facingRight){
                        Flip();
                    }
                    rb.velocity = new Vector2(-1*movementSpeed, rb.velocity.y);
                }
            }
            if(attached && (Input.GetKeyDown("d") || Input.GetKeyDown("right") || Input.GetKeyDown("a")|| Input.GetKeyDown("left"))){
                AudioManager.audioManager.Play("Swing");
            }
            if(Input.GetKey("d") || Input.GetKey("right")){
                if(attached){
                    rb.AddRelativeForce(new Vector3(1, 0,0) * pushForce);
                }
                else if(grounded){
                    if(!facingRight){
                        Flip();
                    }
                    rb.velocity = new Vector2(1*movementSpeed, rb.velocity.y);
                }
            }
            if((Input.GetKeyDown("w")|| Input.GetKeyDown("up"))&&attached){
                // Debug.Log("slide up");
                Slide(1);
            }
            if((Input.GetKeyDown("s")|| Input.GetKeyDown("down"))&&attached){
                // Debug.Log("slide down");
                Slide(-1);
            }
            if(Input.GetKeyDown("q")&&attached){
                Flip();
            }
            if(Input.GetKeyDown(KeyCode.Space)){
                AudioManager.audioManager.Play("Detach");
                Detach();
            }
            if( Input.GetMouseButtonDown(0) )
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if(hit.collider != null)
                {
                    if(hit.transform.gameObject.tag == "Crank"){
                        if(pulleySelected != hit.transform.gameObject && pulleySelected != null)
                        {
                            pulleySelected.GetComponent<Crank>().Deselect();
                        }
                        pulleySelected = hit.transform.gameObject;
                        pulleySelected.GetComponent<Crank>().Select();

                    }
                }
                else{
                    if(pulleySelected != null){
                        pulleySelected.GetComponent<Crank>().Deselect();
                        pulleySelected = null;
                    }
                }
            }
            if(Input.GetKeyDown("f")&&pulleySelected != null){
                // Debug.Log("slide up");
                AudioManager.audioManager.Play("Crank");
                pulleySelected.GetComponent<Crank>().Rotate(1);
            }
            if(Input.GetKeyDown("r")&&pulleySelected != null){
                // Debug.Log("slide down");
                AudioManager.audioManager.Play("Crank");
                pulleySelected.GetComponent<Crank>().Rotate(-1);
            }
    }

    public void Attach(Rigidbody2D ropeBone){
        ropeBone.gameObject.GetComponent<RopeSegment>().isPlayerAttached = true;
        hj.connectedBody = ropeBone;
        hj.enabled = true;
        attached = true;
        attachedTo = ropeBone.gameObject.transform.parent;
        // Debug.Log("ATACHAED");
    }

    void Detach(){
        hj.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttached = false;
        attached = false;
        hj.enabled = false;
        hj.connectedBody = null;
        // Debug.Log("DETACHAED");
    }

    public void Slide(int direction){
        RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();
        GameObject newSeg = null;
        if(direction > 0){
            if(myConnection.connectedAbove != null){
                if(myConnection.connectedAbove.gameObject.GetComponent<RopeSegment>() != null){
                    newSeg = myConnection.connectedAbove;
                    AudioManager.audioManager.Play("Slide Up");
                }
            }
        }
        else{
            if(myConnection.connectedBelow != null){
                newSeg = myConnection.connectedBelow;
                AudioManager.audioManager.Play("Slide Down");
            }
        }
        if(newSeg != null){
            transform.position = newSeg.transform.position;
            myConnection.isPlayerAttached = false;
            newSeg.GetComponent<RopeSegment>().isPlayerAttached = true;
            hj.connectedBody = newSeg.GetComponent<Rigidbody2D>();
        }
        else{
            if(direction > 0){
                // Debug.Log("We at the toppest");
            }
            else{
                // Debug.Log("We at the bottomest");
            }
            
        }  
    }

    void Flip(){
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    void GroundMode(){
        // Debug.Log("in ground mode lets go");
        grounded = true;
        swingBody.SetActive(false);
        crawlBody.SetActive(true);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        timer = countdown;
        if(attachedTo != null){
            disregard = attachedTo.gameObject;
        }
        attachedTo = null;
    }

    void SwingMode(){
        // Debug.Log("swing mode lets go");
        grounded = false;
        swingBody.SetActive(true);
        crawlBody.SetActive(false);
        rb.constraints = RigidbodyConstraints2D.None;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Golden Leaf"){
            col.gameObject.SetActive(false);
            GameObject platform = col.gameObject.transform.parent.Find("Platform").gameObject;
            col.gameObject.transform.parent.transform.position = new Vector3(transform.position.x, transform.position.y-1, transform.position.z);
            platform.SetActive(true);
            GameObject.Find("GameItems").SetActive(false);
            AudioManager.audioManager.Play("Win");
            Win();
        }
        if(!attached){
            if(col.gameObject.tag == "Rope"){
                if(attachedTo != col.gameObject.transform.parent){
                    if(disregard == null || col.gameObject.transform.parent.gameObject != disregard){
                        if(grounded){
                            SwingMode();
                        }
                        AudioManager.audioManager.Play("Attach");
                        Attach(col.gameObject.GetComponent<Rigidbody2D>());
                    }
                }
            }
            if(!grounded && col.gameObject.tag == "Ground"){
                AudioManager.audioManager.Play("Ground");
                GroundMode();
            }
        }
        if(col.gameObject.tag == "Respawn"){
            AudioManager.audioManager.Play("Respawn");
            Respawn();
        }
        
    }

    void WinZoom(){
        winTimer += Time.deltaTime / winStateTime;
        Camera.main.orthographicSize = Mathf.Lerp(camZoomOrigin, 7f, winTimer);
        Camera.main.gameObject.transform.position = new Vector3(Mathf.Lerp( camOrigin.x, transform.position.x, winTimer), Mathf.Lerp( camOrigin.y, transform.position.y, winTimer), -10);
        if(winTimer > 1.0f){
            AudioManager.audioManager.Play("Win2");
            StateManager.smInstance.SetState(StateManager.State.Win);
        }
    }

    void Win(){
        winState = true;
        camOrigin = Camera.main.gameObject.transform.position;
        camZoomOrigin = Camera.main.orthographicSize;
        // timer = 0.5f;
        rb.simulated = false; //stop the player wherever it is
        swingBody.SetActive(false);
        crawlBody.SetActive(true);
        anim.SetTrigger("Win");
        AudioManager.audioManager.Play("Dance");
        // AudioManager.audioManager.Play("Win2");
        InvokeRepeating("Flip", 1f, 1f);
    }

    void Respawn(){
        if(attached){
            Detach();
        }
        if(startGrounded == true){
            GroundMode();
        }
        else{
            SwingMode();
        }
        rb.velocity = new Vector2(0,0);
        transform.position = startPos;
    }
}
