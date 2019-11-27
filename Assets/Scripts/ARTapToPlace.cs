using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Lean.Touch;
using System;

public class ARTapToPlace : MonoBehaviour
{

    private ARRaycastManager m_SessionOriginRaycast;
    private Pose m_PlacementPose;
    private bool m_PlacementPoseIsValid = false;

    public GameObject m_ObjectToPlace; 
    public GameObject m_PlacementIndicator;

    void Start()
    {
        m_SessionOriginRaycast = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (m_PlacementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    public void PlaceObject()
    {
        //Instantiate(m_ObjectToPlace, m_PlacementPose.position, m_PlacementPose.rotation);
        //m_LeanSpawn.Spawn(m_ObjectToPlace.transform, m_PlacementPose.position, m_PlacementPose.rotation);

    }


    private void UpdatePlacementIndicator()
    {
        if (m_PlacementPoseIsValid)
        {
            m_PlacementIndicator.SetActive(true);
            m_PlacementIndicator.transform.SetPositionAndRotation(m_PlacementPose.position, m_PlacementPose.rotation);
        }
        else
        {
            m_PlacementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        m_SessionOriginRaycast.Raycast(screenCenter, hits, TrackableType.Planes);

        m_PlacementPoseIsValid = hits.Count > 0;
        if (m_PlacementPoseIsValid)
        {
            m_PlacementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            // You should always normalise if using vectors to indicate direction
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            m_PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
