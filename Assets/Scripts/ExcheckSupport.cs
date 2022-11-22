using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;


namespace UnityAR
{

    public class ExcheckSupport : MonoBehaviour
    {
        [SerializeField] Text message;
        [SerializeField] ARSession session;
        bool isReady;

        void ShowMessage(string text)
        {
            message.text = $"{text}\r\n";
        }
        void AddMessage(string text)
        {
            message.text+= $"{text}\r\n";
        }

        private void Awake()
        {
            if(message == null)
            {
                Application.Quit();
            }

            if(session == null)
            {
                isReady = false;
                ShowMessage("?G???[:SerializeField???????s??");
            }
            else
            {
                isReady = true;
                ShowMessage("AR???T?|?[?g????");
            }
        }

        IEnumerator CheckSupport()
        {
            yield return ARSession.CheckAvailability();
            if(ARSession.state == ARSessionState.NeedsInstall)
            {
                AddMessage("");
                yield return ARSession.Install();

            }
            if(ARSession.state == ARSessionState.NeedsInstall || ARSession.state == ARSessionState.Installing){
                AddMessage("");
                AddMessage($"State:{ARSession.state}");
                yield break;

            }
            if(ARSession.state==ARSessionState.Unsupported)
            {
                AddMessage("?????f?o?C?X???T?|?[?g????????????");
                AddMessage($"State:{ARSession.state}");
                yield break;
            }
            AddMessage("?????f?o?C?X??AR???T?|?[?g???????????B");
            AddMessage("AR?Z?b?V????????????");
            session.enabled = true;
            const float interval = 30f;
            var timer = interval;
            while((ARSession.state == ARSessionState.Ready||ARSession.state == ARSessionState.SessionInitializing) && timer >0)
            {
                var waitTime = 0.5f;
                timer -= waitTime;
                yield return new WaitForSeconds(waitTime);
            }
            if (timer <= 0)
            {
                AddMessage("???????^?C???I?[?o?[");
                AddMessage($"State:{ARSession.state}");
                yield break;
            }
            AddMessage("??????????");
            AddMessage($"State:{ARSession.state}");
        }

        private void OnEnable()
        {
            if (!isReady) { return; }
            StartCoroutine(CheckSupport());
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}