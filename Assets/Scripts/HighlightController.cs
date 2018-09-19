using UnityEngine;

public class HighlightController : MonoBehaviour
{
    private GameObject child;
    private Renderer rend;

    public bool objectInHand = false;

    public Material[] materials = new Material[2];

    private void Start()
    {
        // Through experimentation you cannot just find the Renderer of the instance and change it further down the code.  It will
        // throw out a "Cannot find an instance of an object error".  So, you have to use a GAMEOBJECT instance and then grab the
        // renderer out of it using the full path every time.. annoying.
        child = this.gameObject.transform.GetChild(0).gameObject;
        //print(child.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Controller (left)" || other.gameObject.name == "Controller (right)")
        {
            if (!objectInHand)
            {
                // This is where you have to use the gameObject instance as a starting point to grab the renderer or it will error out.
                child.GetComponentInChildren<Renderer>().material = materials[1];
            }
            else
            {
                child.GetComponentInChildren<Renderer>().material = materials[0];
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Controller (left)" || other.gameObject.name == "Controller (right)")
        {
            if (objectInHand)
            {
                child.GetComponentInChildren<Renderer>().material = materials[0];
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Controller (left)" || other.gameObject.name == "Controller (right)")
        {
            child.GetComponentInChildren<Renderer>().material = materials[0];
        }
    }
}

