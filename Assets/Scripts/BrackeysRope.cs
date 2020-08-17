using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrackeysRope : MonoBehaviour
{
    public Rigidbody2D hook;
    public GameObject[] linkPrefabs;
    public int numLinks = 5;

    public HingeJoint2D top;

    public CaterPlayer player;
    
    void Start()
    {
        player = GameObject.Find("CaterPlayer").GetComponent<CaterPlayer>();
        GenerateRope();
    }

    void GenerateRope(){
        Rigidbody2D previousRB = hook;
        for(int i = 0; i < numLinks; ++i){
            int index = Random.Range(0, linkPrefabs.Length);
            GameObject newLink = Instantiate(linkPrefabs[index]);
            newLink.transform.parent = transform;
            newLink.transform.position = transform.position;
            HingeJoint2D hj = newLink.GetComponent<HingeJoint2D>();
            hj.connectedBody = previousRB;
            previousRB = newLink.GetComponent<Rigidbody2D>();
            if(i==0){
                top = hj;
            }
        }
    }

    public void addLink(){
        int index = Random.Range(0, linkPrefabs.Length);
        GameObject newLink = Instantiate(linkPrefabs[index]);
        newLink.transform.parent = transform;
        newLink.transform.position = transform.position;
        HingeJoint2D hj = newLink.GetComponent<HingeJoint2D>();
        hj.connectedBody = hook;
        newLink.GetComponent<RopeSegment>().connectedBelow = top.gameObject;
        top.connectedBody = newLink.GetComponent<Rigidbody2D>();
        top.GetComponent<RopeSegment>().ResetAnchor();
        top = hj;
    }

    public void removeLink(){
        if(top.gameObject.GetComponent<RopeSegment>().isPlayerAttached){
            player.Slide(-1);
        }
        HingeJoint2D newTop = top.gameObject.GetComponent<RopeSegment>().connectedBelow.GetComponent<HingeJoint2D>();
        newTop.connectedBody = hook;
        newTop.gameObject.transform.position = hook.gameObject.transform.position;
        Destroy(top.gameObject);
        // Debug.Log("New top is "+newTop);
        top = newTop;
    }
}
