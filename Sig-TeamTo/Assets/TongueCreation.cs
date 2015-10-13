using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TongueCreation : MonoBehaviour {

    public GameObject tongue_Begin, tongue_Middle, tongue_End;
    private LinkedList<GameObject> fullTongue;
    private bool isTongueOut = false;
    private Vector3 position;
    private Quaternion rotation = new Quaternion(0,0,0,0);
    private static float magicY = 1.35f;
    private static float magicX = 0.14f;
    private float magicJointAnchorX = -3.965f;
    private float magicJointConnectedAnchorX = 3.965f;

	// Use this for initialization
	void Start () {
        fullTongue = new LinkedList<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown("space"))
        {
            position = this.gameObject.transform.position;
            isTongueOut = !isTongueOut;
            if (isTongueOut)
            {
                createBeginingTongue();
                createEndTongue();

            }
            else
            {
                destroyTongue();
            }
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            createMiddleTongue();
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            LinkedListNode<GameObject> it = fullTongue.Last;
            it = it.Previous;
            if (it.Previous != null)
            {
                reduceTongueSize();
            }
            else
            {
                destroyTongue();
                isTongueOut = !isTongueOut;
            }
        }
	}

    void createBeginingTongue()
    {
        fullTongue.AddFirst((GameObject)Instantiate(tongue_Begin, new Vector3(position.x, position.y + magicY, position.z), rotation));
        fullTongue.First.Value.AddComponent<HingeJoint2D>();
        HingeJoint2D hinge = fullTongue.First.Value.GetComponent<HingeJoint2D>();
        hinge.connectedBody = this.gameObject.GetComponent<Rigidbody2D>();
        hinge.anchor = new Vector2(magicJointAnchorX, 0);
        hinge.connectedAnchor = new Vector2(magicX, magicY);
    }

    void createMiddleTongue()
    {
        GameObject lastLink = fullTongue.Last.Value;
        GameObject newLink = (GameObject)Instantiate(tongue_Middle, new Vector3(position.x, position.y + magicY, position.z), rotation);
        newLink.AddComponent<HingeJoint2D>();
        HingeJoint2D lastHinge = lastLink.GetComponent<HingeJoint2D>();
        HingeJoint2D newHinge = newLink.GetComponent<HingeJoint2D>();

        newHinge.connectedBody = fullTongue.Last.Previous.Value.GetComponent<Rigidbody2D>();
        newHinge.anchor = new Vector2(magicJointAnchorX, 0);
        newHinge.connectedAnchor = new Vector2(magicJointConnectedAnchorX, 0);

        lastLink.transform.position = new Vector3(lastLink.transform.position.x, lastLink.transform.position.y, lastLink.transform.position.z);

        lastHinge.connectedBody = newLink.GetComponent<Rigidbody2D>();
        lastHinge.anchor = new Vector2(magicJointAnchorX, 0);
        lastHinge.connectedAnchor = new Vector2(magicJointConnectedAnchorX, 0);

        fullTongue.AddBefore(fullTongue.Last, newLink);
    }

    void createEndTongue()
    {
        fullTongue.AddLast((GameObject)Instantiate(tongue_End, new Vector3(position.x, position.y + magicY, position.z), rotation));
        fullTongue.Last.Value.AddComponent<HingeJoint2D>();
        HingeJoint2D hinge = fullTongue.Last.Value.GetComponent<HingeJoint2D>();
        hinge.connectedBody = fullTongue.Last.Previous.Value.GetComponent<Rigidbody2D>();
        hinge.anchor = new Vector2(magicJointAnchorX, 0);
        hinge.connectedAnchor = new Vector2(magicJointConnectedAnchorX, 0);
    }

    void reduceTongueSize()
    {
        GameObject lastLink = fullTongue.Last.Value;
        GameObject link2Remove = fullTongue.Last.Previous.Value;
        GameObject newPenultimateLink = fullTongue.Last.Previous.Previous.Value;

        HingeJoint2D lastHinge = lastLink.GetComponent<HingeJoint2D>();
        lastLink.transform.position = new Vector3(lastLink.transform.position.x, lastLink.transform.position.y, lastLink.transform.position.z);
        lastHinge.connectedBody = newPenultimateLink.GetComponent<Rigidbody2D>();

        Destroy(link2Remove);
        fullTongue.Remove(fullTongue.Last.Previous);
    }

    void destroyTongue()
    {
        LinkedListNode<GameObject> it = fullTongue.Last;
        GameObject removed;
        while (it != null)
        {
            removed = it.Value;
            it = it.Previous;
            Destroy(removed);
            fullTongue.RemoveLast();
        }
    }
}
