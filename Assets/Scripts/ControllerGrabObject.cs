using UnityEngine;


public class ControllerGrabObject : MonoBehaviour {

    public EventController eventController;


    [HideInInspector]
    public GameObject objectInHand;
    [HideInInspector]
    public GameObject collidingObject;

    private SteamVR_TrackedObject trackedObj;
    private bool objectInHandCheck = false;

    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void Awake() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void SetCollidingObject(Collider col) {
        if (collidingObject || !col.GetComponent<Rigidbody>()) {
            return;
        }
        collidingObject = col.gameObject;
        eventController.collidingObject = collidingObject;
    }

    public void OnTriggerEnter(Collider other) {
        SetCollidingObject(other);

        eventController.myState = EventController.States.objectHighlighted;
    }

    public void OnTriggerStay(Collider other) {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other) {
        if (!collidingObject) {
            return;
        }
        collidingObject = null;
        eventController.myState = EventController.States.freeRoam;
    }

    private void GrabObject() {
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        objectInHandCheck = true;
        if (objectInHand.GetComponent<HighlightController>()) {
            objectInHand.GetComponent<HighlightController>().objectInHand = objectInHandCheck;
            eventController.GetComponent<EventController>().myState = EventController.States.objectInHand;
        }
    }

    private FixedJoint AddFixedJoint() {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject() {
        if (GetComponent<FixedJoint>()) {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        objectInHandCheck = false;
        if (objectInHand.GetComponent<HighlightController>()) {
            objectInHand.GetComponent<HighlightController>().objectInHand = objectInHandCheck;
        }

        objectInHand = null;        
    }


    void Update() {

        if (Controller.GetHairTriggerDown()) {
            if (collidingObject) {
                GrabObject();
            }
        }

        if (Controller.GetHairTriggerUp()) {
            if (objectInHand) {
                ReleaseObject();
            }
        }
    }


}
