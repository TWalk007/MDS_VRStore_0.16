using UnityEngine;
using UnityEngine.UI;

public class VRInteractableUI : MonoBehaviour {

    public GameObject eventController;

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    private ControllerGrabObject controllerGrabObject;

    private void Awake() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Start() {
          controllerGrabObject = gameObject.GetComponent<ControllerGrabObject>();

    }

    private void Update() {
        if (eventController.GetComponent<EventController>().myState == EventController.States.menuActive) {
            GameObject hitObject = null;
            RaycastHit raycastHit;

            if (Physics.Raycast(transform.position, transform.forward, out raycastHit)) {
                hitObject = raycastHit.transform.gameObject;

                if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
                    if (hitObject.transform.tag == "Button") {
                        print("Hit the button!");
                    }
                }
            }
        }            
    }        
}
