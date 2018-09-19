using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHighlighter : MonoBehaviour {

    public bool objectInHand = false;
    public Material[] materials = new Material[2];

    private Renderer rend;

    private void Start()
    {
        rend = this.gameObject.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Controller (left)" || other.gameObject.name == "Controller (right)")
        {
            if (!objectInHand)
            {
                // This is where you have to use the gameObject instance as a starting point to grab the renderer or it will error out.
                rend.material = materials[1];
            }
            else
            {
                rend.material = materials[0];
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Controller (left)" || other.gameObject.name == "Controller (right)")
        {
            if (objectInHand)
            {
                rend.material = materials[0];
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Controller (left)" || other.gameObject.name == "Controller (right)")
        {
            rend.material = materials[0];
        }
    }

}
