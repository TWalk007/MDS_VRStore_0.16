using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

	public float rotationSpeed = 50.0f;

	private float currentRot;
	private float newRot;
	private Vector3 angles;

	private SteamVR_TrackedObject trackedObj;
	private GameObject collidingObject;
	private GameObject objectInHand;

	private bool objectInHandCheck = false;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	private void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	private void SetCollidingObject(Collider col)
	{
		if (collidingObject || !col.GetComponent<Rigidbody>())
		{
			return;
		}
		collidingObject = col.gameObject;
	}

	public void OnTriggerEnter(Collider other)
	{
		SetCollidingObject(other);


		//TODO  This does not take into account the current rotation value of the object when starting the rotation.
		//      For example, if it's at 200° it might appear backwards to you but you might want to push it the other way instead
		//      and get the reverse result of your intention.
		if (Controller.GetAxis() != Vector2.zero)
		{
			if (Controller.GetAxis().y < 0f)
			{
				other.transform.Rotate(Vector3.up, rotationSpeed * Controller.GetAxis().y * 10 * Time.deltaTime);
			}
			if (Controller.GetAxis().y > 0f)
			{
				other.transform.Rotate(Vector3.down, rotationSpeed * -Controller.GetAxis().y * 10 * 2 * Time.deltaTime);
			}
		}
	}

	public void OnTriggerStay(Collider other)
	{
		SetCollidingObject(other);

		if (Controller.GetAxis() != Vector2.zero)
		{
			if (Controller.GetAxis().y < 0f)
			{
				other.transform.Rotate(Vector3.up, rotationSpeed * Controller.GetAxis().y * 10 * Time.deltaTime);
			}
			if (Controller.GetAxis().y > 0f)
			{
				other.transform.Rotate(Vector3.down, rotationSpeed * -Controller.GetAxis().y * 10 * 2 * Time.deltaTime);
			}            
		}

	}

	public void OnTriggerExit(Collider other)
	{
		if (!collidingObject)
		{
			return;
		}
		collidingObject = null;
	}

	private void GrabObject()
	{
		objectInHand = collidingObject;
		collidingObject = null;

		var joint = AddFixedJoint();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
		objectInHandCheck = true;
		if (objectInHand.GetComponent<HighlightController>())
		{
			objectInHand.GetComponent<HighlightController>().objectInHand = objectInHandCheck;
		}
	}

	private FixedJoint AddFixedJoint()
	{
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	private void ReleaseObject()
	{
		if (GetComponent<FixedJoint>())
		{
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy(GetComponent<FixedJoint>());

			objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
			objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
		}
		objectInHandCheck = false;
		if (objectInHand.GetComponent<HighlightController>())
		{
			objectInHand.GetComponent<HighlightController>().objectInHand = objectInHandCheck;
		}

		objectInHand = null;
	}

	void Update () {
		if (Controller.GetHairTriggerDown())
		{
			if (collidingObject)
			{
				GrabObject();
			}
		}

		if (Controller.GetHairTriggerUp())
		{
			if (objectInHand)
			{
				ReleaseObject();
			}
		}		      
	}
}
