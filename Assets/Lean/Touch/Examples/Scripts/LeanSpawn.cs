using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component allows you to spawn a prefab at the specified world point.
	/// NOTE: To trigger the prefab spawn you must call the Spawn method on this component from somewhere.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanSpawn")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Spawn")]
	public class LeanSpawn : MonoBehaviour
	{

        private ARRaycastManager m_SessionOriginRaycast;
        private Pose m_PlacementPose;
        private bool m_PlacementPoseIsValid = false;

        /// <summary>The prefab that this component can spawn.</summary>
        [Tooltip("The prefab that this component can spawn.")]
	    public Transform Prefab;
        //public GameObject m_ObjectToPlace;
        public GameObject m_PlacementIndicator;

        private void Start()
        {
            m_SessionOriginRaycast = FindObjectOfType<ARRaycastManager>();
        }

        private void Update()
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }

        /// <summary>This will spawn Prefab at the specified finger based on the ScreenDepth setting.</summary>
        public void Spawn()
		{
			if (Prefab != null)
			{
				var clone = Instantiate(Prefab);

				clone.position = m_PlacementPose.position;
                clone.rotation = m_PlacementPose.rotation;

				clone.gameObject.SetActive(true);
			}
		}

        //public void PlaceObject()
        //{
        //    Instantiate(m_ObjectToPlace, m_PlacementPose.position, m_PlacementPose.rotation);
        //    m_LeanSpawn.Spawn(m_ObjectToPlace.transform, m_PlacementPose.position, m_PlacementPose.rotation);

        //}


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
}