using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

namespace UnityAR
{

    [RequireComponent(typeof(ARPlaneManager))]
    [RequireComponent(typeof(ARRaycastManager))]
    [RequireComponent(typeof(PlayerInput))]

    public class ExPlaneDetection : MonoBehaviour
    {
        [SerializeField] Text message;
        [SerializeField] GameObject placementPrefab;
        ARPlaneManager planeManager;
        ARRaycastManager raycastManager;
        PlayerInput playerInput;
        bool isReady;

        void ShowMessage(string text)
        {
            message.text = $"{text}\r\n";
        }
        void AddMessage(string text)
        {
            message.text += $"{text}\r\n";
        }

        private void Awake()
        {
            if(message == null) { Application.Quit(); }

            planeManager = GetComponent<ARPlaneManager>();
            playerInput = GetComponent<PlayerInput>();
            raycastManager = GetComponent<ARRaycastManager>();
            if(placementPrefab == null||raycastManager==null ||planeManager.planePrefab==null||raycastManager==null||playerInput==null||playerInput.actions==null)
            {
                isReady = false;
                ShowMessage("�G���[:SerializeField�Ȃǂ̐ݒ�s��");
            }
            else
            {
                isReady = true;
                ShowMessage("���ʌ��o");
                AddMessage("�����B�e���Ă�������,���΂炭����ƁA���ʂ����o����܂��B>>>���ʂ��^�b�v����ƈ֎q���\������܂��B");
            }
        }
        GameObject instantiatedObject = null;

        void OnTouch(InputValue touchinfo)
        {
            if (!isReady) { return; }
            var touchPosition = touchinfo.Get<Vector2>();
            var hits = new List<ARRaycastHit>();
            if(raycastManager.Raycast(touchPosition,hits,
                TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                if(instantiatedObject == null)
                {
                    instantiatedObject = Instantiate(placementPrefab, hitPose.position, hitPose.rotation);
                }
                else
                {
                    instantiatedObject.transform.position = hitPose.position;
                }
            }
        }
    }

}
