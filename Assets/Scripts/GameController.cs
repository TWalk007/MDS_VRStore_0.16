using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Transform cameraTransform;
    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    public Vector3 teleportReticleOffset;
    public LayerMask teleportMask;
    private bool shouldTeleport;

    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;

    private GameObject trackedObj;

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(cameraTransform.position, hitPoint, 0.5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
    }

    private void Teleport()
    {
        shouldTeleport = false;
        reticle.SetActive(false);
        cameraTransform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            RaycastHit hit;


            if (Physics.Raycast(cameraTransform.position, transform.forward, out hit, 100, teleportMask))
            {
                hitPoint = hit.point;
                ShowLaser(hit);
                reticle.SetActive(true);
                teleportReticleTransform.position = hitPoint + teleportReticleOffset;
                shouldTeleport = true;
            }
            else
            {
                laser.SetActive(false);
                reticle.SetActive(false);
            }
        }

        if ( Input.GetMouseButtonUp(0) && shouldTeleport)
        {
            Teleport();
        }
    }
}

