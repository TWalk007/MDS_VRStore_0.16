using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour {

    public float resetTime = 0.5f;
    public float resetActiveDelay = 3f;

    public GameObject pins;
    public GameObject ball;

    private GameObject oldPins;
    private GameObject oldBall;

    private bool buttonOn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Button")
        {

            if (!buttonOn)                
            {
                buttonOn = true;
                oldPins = GameObject.Find("Bowling Pins");
                oldBall = GameObject.Find("Ball Parent");

                SearchForBall();
                SearchForPins();

                DeleteOldBowlingGame();

                StartCoroutine(ResetDelay());
            }            
        }
    }

    private void DeleteOldBowlingGame()
    {
        Destroy(oldPins);
        Destroy(oldBall);
    }

    private IEnumerator ResetDelay()
    {
        yield return new WaitForSeconds(resetTime);
        if (oldPins == null)
        {
            Instantiate(pins);
        }
        if (oldBall == null)
        {
            Instantiate(ball);
        }
        yield return new WaitForSeconds(resetActiveDelay);
        buttonOn = false;
        print("Reset Button Active");
    }

    private void SearchForPins()
    {
        if (oldPins == null)
        {
            oldPins = GameObject.Find("Bowling Pins(Clone)");
        }
    }

    private void SearchForBall()
    {
        if (oldBall == null)
        {
            oldBall = GameObject.Find("Ball Parent(Clone)");
        }
    }
}
