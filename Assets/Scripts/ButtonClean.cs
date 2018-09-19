using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClean : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CleanButton")
        {
            SceneManager.LoadScene(0);
        }
    }
}
