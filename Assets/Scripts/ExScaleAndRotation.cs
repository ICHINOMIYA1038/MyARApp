using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace UnityAR
{


    [RequireComponent(typeof(ExMakeAppearOnPlane))]


    public class ExScaleAndRotation : MonoBehaviour
    {
        [SerializeField] Text message;
        [SerializeField] Text scaleText;
        [SerializeField] Slider scaleSlider;
        [SerializeField] Text rotationText;
        [SerializeField] Slider rotationSlider;
        bool isReady;

        void Awake()
        {
            if(message == null)
            {
                Application.Quit();
            }
            if(scaleText==null||scaleSlider==null||rotationText==null||rotationSlider==null)
            {
                isReady = false;
            }
            else
            {
                isReady = true;
            }
        }



        void ShowMessage(string text)
        {
            message.text = $"{text}";
        }
        void AddMessage(string text)
        {
            message.text += $"{text}";
        }
        void ShowScale(float scale)
        {
            scaleText.text = $"倍率:{scale:F1}";
        }
        void ShowRotation(float angle)
        {
            rotationText.text = $"回転角:{angle:F0}";
        }

        ExMakeAppearOnPlane makeAppearOnPlane;
        const float MinScale = 0.2f;
        const float MaxScale = 2f;
        const float MinRotation = 0f;
        const float MaxRotation = 360f;




        // Start is called before the first frame update
        void Start()
        {
            makeAppearOnPlane = GetComponent<ExMakeAppearOnPlane>();
            if (!isReady || makeAppearOnPlane == null || makeAppearOnPlane.IsAvailable)
            {
                isReady = false;
                ShowMessage("error:SerializeField");
                return;
            }

            isReady = true;
            ShowMessage("scale&rotation");
            AddMessage("take a photo and detect plane");
            var initScale = 1f;
            ShowScale(initScale);
            scaleSlider.value = (initScale - MinScale) / (MaxScale - MinScale);
            scaleSlider.onValueChanged.AddListener(OnScaleSliderValueChanged);

            var initRotation = Quaternion.identity;
            ShowRotation(initRotation.eulerAngles.y);
            rotationSlider.value = (initRotation.eulerAngles.y - MinRotation) / (MaxRotation - MinRotation);
            rotationSlider.onValueChanged.AddListener(OnRotationSliderValueChanged);
        }

        void OnScaleSliderValueChanged(float value)
        {
            if (!isReady) { return; }

            var scale = value * (MaxScale - MinScale) + MinScale;
            ShowScale(scale);
            makeAppearOnPlane.Scale = scale;
        }

        void OnRotationSliderValueChanged(float value)
        {
            if (!isReady) { return; }

            var rotY = value * (MaxRotation - MinRotation) + MinRotation;
            ShowRotation(rotY);
            makeAppearOnPlane.Rotation = Quaternion.Euler(0f, rotY, 0f);
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}