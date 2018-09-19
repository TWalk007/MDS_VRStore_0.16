using UnityEngine;

public class EventController : MonoBehaviour {

    #region PUBLIC Variables

    public GameObject contextMenuSystem;
    public GameObject parabolicPointer;

    [HideInInspector]
    public GameObject collidingObject;

    public enum States { freeRoam, objectHighlighted, objectInHand, menuActive };
    public States myState;

    [HideInInspector]
    public bool menuOpen = false;

    #endregion

    #region PRIVATE Variables

    private SteamVR_Controller.Device Controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    private bool parabolicPointerOn = true;
    private bool laserPointerOn = false;

    #endregion

    private void Awake() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Update() {
        if (menuOpen) {
            myState = States.menuActive;
        }

        if (!menuOpen) {
            //IF there is no colliding object in any instance of the ControllerGrabObject script (on either controller)
            //  THEN -> turn the freeRoam state back on.
            GameObject[] controllers = GameObject.FindGameObjectsWithTag("Controllers");
            bool objectInEitherHand = false;
            foreach (GameObject obj in controllers) {
                // IF there is a colliding object
                if (collidingObject) {
                    objectInEitherHand = true;
                }
            }
            if (!objectInEitherHand) {
                myState = States.freeRoam;
            }
        }

        if (myState == States.freeRoam) {
            if (!parabolicPointerOn) {
                if (!menuOpen) {
                    TurnOnParabolicPointer();
                }
            }

        } else if (myState == States.objectHighlighted) {
            if (parabolicPointerOn) {
                TurnOffParabolicPointer();
            }

            if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {

                if (!menuOpen) {

                    GameObject menuInScene = GameObject.FindGameObjectWithTag("ContextMenuSystem");

                    //If there is not menuInScene already, go ahead and create teh contextMenu gameobject.
                    if (menuInScene == null) {
                        GameObject contextMenu = (GameObject)Instantiate(contextMenuSystem);
                        Vector3 offsetPos = new Vector3(0, 0.25f, 0.25f);

                        // the "Controller.transform.pos" was changed from "collidingObject.transform.position".
                        // This could screw up the placement of the "UI Context Menu Sysem".
                        Vector3 newPos = Controller.transform.pos + offsetPos;
                        contextMenu.transform.position = newPos;

                        myState = States.menuActive;
                        menuOpen = true;
                    }
                }
            }


        } else if (myState == States.objectInHand) {
            if (parabolicPointerOn) {
                TurnOffParabolicPointer();
            }


        } else if (myState == States.menuActive) {
            if (parabolicPointerOn) {
                TurnOffParabolicPointer();
            }

            //One time activation of the laser pointer script.
            if (!laserPointerOn) {
                TurnOnLaserPointer();
            }
        }
    }

    private void TurnOffParabolicPointer() {
        parabolicPointer.SetActive(false);
        parabolicPointer.GetComponent<ParabolicPointer>().enabled = false;
        parabolicPointerOn = false;

        //Setting the TeleportState to "None" removes the flashing when trying to warp when it should be off.
        TeleportVive teleportVive = FindObjectOfType<TeleportVive>();
        teleportVive.CurrentTeleportState = TeleportState.None;
    }

    private void TurnOnParabolicPointer() {
        parabolicPointer.SetActive(true);
        parabolicPointer.GetComponent<ParabolicPointer>().enabled = true;
        parabolicPointerOn = true;

        //Setting the TeleportState to "None" reduces all the popping around when it comes back.
        TeleportVive teleportVive = FindObjectOfType<TeleportVive>();
        teleportVive.CurrentTeleportState = TeleportState.None;
    }

    private void TurnOnLaserPointer() {
        laserPointerOn = true;

        // Write a script here that creates an array of the controllers and then loop through
        // to turn on their laser pointer components.

        GameObject[] controllers = GameObject.FindGameObjectsWithTag("Controllers");
        foreach (GameObject obj in controllers) {
            gameObject.GetComponent<SteamVR_LaserPointer>().enabled = true;
            gameObject.GetComponent<VRInteractableUI>().enabled = true;
        }
    }



}
