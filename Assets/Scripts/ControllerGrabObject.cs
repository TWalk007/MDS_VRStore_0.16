using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControllerGrabObject : MonoBehaviour {

    public GameObject parabolicPointer;

    [HideInInspector]
    public GameObject objectInHand;
    [HideInInspector]
    public GameObject collidingObject;
    [HideInInspector]
    public bool menuOpen = false;

    public GameObject contextMenuSystem;
    public enum States { freeRoam, objectHighlighted, objectInHand, menuActive };
    public States myState;
       


    private SteamVR_TrackedObject trackedObj;
    private bool objectInHandCheck = false;
    private bool parabolicPointerOn = true;
    private GameObject navMesh;

    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }


    private void Awake() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Start() {
        navMesh = GameObject.FindGameObjectWithTag("Navmesh");
    }


    private void SetCollidingObject(Collider col) {
        if (collidingObject || !col.GetComponent<Rigidbody>()) {
            return;
        }
        collidingObject = col.gameObject;
    }

    public void OnTriggerEnter(Collider other) {
        SetCollidingObject(other);

        myState = States.objectHighlighted;        
    }

    public void OnTriggerStay(Collider other) {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other) {
        if (!collidingObject) {
            return;
        }
        collidingObject = null;
        myState = States.freeRoam;
    }

    private void GrabObject() {
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        objectInHandCheck = true;
        if (objectInHand.GetComponent<HighlightController>()) {
            objectInHand.GetComponent<HighlightController>().objectInHand = objectInHandCheck;
            myState = States.objectInHand;
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
        myState = States.freeRoam;
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

        if (myState == States.freeRoam) {
            //I placed this event into an if statement because it was happening every frame
            //which caused the pointer to constantly be enabled not allowing it to work correctly
            //(teleporting through the floor).
            if (!parabolicPointerOn) {

                //This state setting here prevents all the popping around when the teleport should be off.


                if (!menuOpen) {
                    TurnOnParabolicPointer();
                }
            }


        } else if (myState == States.objectHighlighted) {
            if (parabolicPointerOn == true) {
                TurnOffParabolicPointer();
            }

            if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
                
                if (!menuOpen) {

                    GameObject menuInScene = GameObject.FindGameObjectWithTag("ContextMenuSystem");

                    if (menuInScene == null) {
                        GameObject contextMenu = (GameObject)Instantiate(contextMenuSystem);
                        Vector3 offsetPos = new Vector3(0, 0.25f, 0.25f);
                        Vector3 newPos = collidingObject.transform.position + offsetPos;
                        contextMenu.transform.position = newPos;

                        myState = States.menuActive;
                        menuOpen = true;
                    } 
                }
            }
                       


        } else if (myState == States.objectInHand) {
            if (parabolicPointerOn == true) {
                TurnOffParabolicPointer();
            }


        } else if (myState == States.menuActive) {
            if (parabolicPointerOn == true) {
                TurnOffParabolicPointer();  
            }

            //Ray raycast = new Ray(transform.position, transform.forward);
            //RaycastHit rayHit;
            //bool bHit = Physics.Raycast(raycast, out rayHit);
            //if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
            //    GameObject hitObject = rayHit.transform.gameObject;
            //    hitObject.GetComponent<Button>().onClick.Invoke();
            //}
        }
    }

    private void TurnOffParabolicPointer() {
        navMesh.SetActive(false);
        parabolicPointer.SetActive(false);
        parabolicPointer.GetComponent<ParabolicPointer>().enabled = false;
        parabolicPointerOn = false;

        //This state setting here prevents all the popping around when the teleport should be off.
        TeleportVive teleportVive = FindObjectOfType<TeleportVive>();
        teleportVive.CurrentTeleportState = TeleportState.None;
    }

    private void TurnOnParabolicPointer() {
        navMesh.SetActive(true);       
        parabolicPointer.SetActive(true);
        parabolicPointer.GetComponent<ParabolicPointer>().enabled = true;

        parabolicPointerOn = true; TeleportVive teleportVive = FindObjectOfType<TeleportVive>();
        teleportVive.CurrentTeleportState = TeleportState.None;
    }
}
