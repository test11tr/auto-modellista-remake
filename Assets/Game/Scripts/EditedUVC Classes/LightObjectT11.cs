using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace PG
{
    /// <summary>
    /// Component responsible for all light signals.
    /// </summary>
    public class LightObjectT11 : MonoBehaviour
    {
        public CarLightType CarLightTypeT11;
        public Light LightGO;
        public Material OnLightMaterial;                //Material with glow, used for soft and hard switching.

        [Header("Soft Switch settings")]
        public float OnSwitchSpeed = 10f;
        public float OffSwitchSpeed = 2f;
        public float Intensity = 2f;
        public UnityEngine.Color emissiveColor;

        [Header("Main settings")]
        public bool EnableOnStart;
        Material MaterialForSoftSwitch;
        Animator LightsAnimator;
        bool isInited;

        //IDs for accessing properties, so as not to use the string (Optimization).
        int ColorPropertyIDtoChange;
        int AnimatorLightIsOnID;
        Coroutine SoftSwitchCoroutine;

        public bool LightIsOn { get; private set; }

        public void TryInitSoftSwitch ()
        {
            if (!isInited) {
                InitSoftSwitch();
            }    
        }

        public void InitSoftSwitch ()
        {
            MaterialForSoftSwitch = OnLightMaterial;
            ColorPropertyIDtoChange = Shader.PropertyToID ("_Emissive_Color");  
            LightsAnimator = GetComponent<Animator>(); //if Animator needed
            AnimatorLightIsOnID = Animator.StringToHash ("LightIsOn");
        }

        void Start ()
        {
            LightIsOn = !EnableOnStart;
            Switch (EnableOnStart, forceSwitch: true);
        }

        /// <summary>
        /// Switch light LightIsOn =! LightIsOn.
        /// </summary>
        public void Switch ()
        {
            Switch (!LightIsOn, forceSwitch: true);
        }

        /// <summary>
        /// Switch with parameters.
        /// </summary>
        public void Switch (bool value, bool forceSwitch = false)
        {
            if (LightIsOn == value)
            {
                return;
            }

            LightIsOn = value;

            if (SoftSwitchCoroutine != null)
            {
                StopCoroutine (SoftSwitchCoroutine);
            }

            if (MaterialForSoftSwitch != null)
            {
                SoftSwitchCoroutine = StartCoroutine (SoftSwitch (LightIsOn, forceSwitch));
            }
            
            //The animator is needed to turn on the headlights such as those of the AE86 or turn on the off the gameobject.
            if (LightsAnimator != null)
            {
                LightsAnimator.SetBool (AnimatorLightIsOnID, LightIsOn);
            }
        }

        IEnumerator SoftSwitch (bool value, bool forceSwitch = false)
        {
            //Calculation of the start and target Intensity glow.
            UnityEngine.Color targetColor = (value ? emissiveColor * Intensity : UnityEngine.Color.black);
            UnityEngine.Color startColor = (value ? UnityEngine.Color.black : UnityEngine.Color.black);
            var speed = value? OnSwitchSpeed: OffSwitchSpeed;
            float timer = 0;

            if (!value && LightGO)
            {
                LightGO.SetActive (value);
            }

            if (!forceSwitch)
            {
                while (timer < 1)
                {
                    var color = UnityEngine.Color.Lerp (startColor, targetColor, timer);
                    MaterialForSoftSwitch.SetColor(ColorPropertyIDtoChange, color);
                    timer += speed * Time.deltaTime;
                    yield return null;
                }
            }

            if (value && LightGO)
            {
                LightGO.SetActive (value);
            }

            MaterialForSoftSwitch.SetColor(ColorPropertyIDtoChange, targetColor);
            SoftSwitchCoroutine = null;
        }
    }
}
