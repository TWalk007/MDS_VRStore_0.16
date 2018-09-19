using UnityEngine;
using UnityEngine.UI;

public class ContextMenuSystem : MonoBehaviour {

    [Header("Buttons")]
    public Button backButton;
    public Button nextButton;
    public Button plusButton;
    public Button minusButton;



    private SteamVR_TrackedObject trackedObj;

    private GameObject canvasObj;
    private EventController eventController;
    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }



    private void Awake() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start() {        
        canvasObj = transform.GetChild(0).gameObject;
    }

    public void ExitButton() {
        eventController.myState = EventController.States.freeRoam;
        eventController.menuOpen = false;
        Destroy(gameObject);
    }


    //This will "grey out" the minus or plus button allowing us to use it or not based upon where it is in the hierarchy.
    //private void SelectionHierarchyCheck() {
    //    if (objectInHand.tag == "PasteHorizontal" || objectInHand.tag == "PasteVertical" || objectInHand.tag == "RinseBottle") {
    //        if (objectInHand.transform.parent.tag != "PasteHorizontal" || objectInHand.transform.parent.tag != "PasteVertical" || objectInHand.transform.parent.tag != "RinseBottle") {
    //            plusButton.interactable = false;
    //        } else {
    //            minusButton.interactable = false;
    //        }
    //    }
    //}

    //When this button is selected, it will make the parent of the object in hand active.  This will show the group of objects switch to the highlighted material all at once.
    //public void ExpandSelection() {
    //    foreach (Transform child in transform.parent) {
    //        Material[] materials = child.GetComponent<HighlightController>().materials;
    //        child.GetComponent<Renderer>().material = materials[1];
    //    }
    //}

    //public void ShrinkSelection() {
    //    foreach (Transform child in transform.parent) {
    //        Material[] materials = child.GetComponent<HighlightController>().materials;
    //        child.GetComponent<Renderer>().material = materials[0];
    //    }
    //    objectInHand.GetComponent<Renderer>().material = objectInHand.GetComponent<HighlightController>().materials[1];
    //}


}
