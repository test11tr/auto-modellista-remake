using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using test11;

namespace PG
{
    /// <summary>
    /// Car light logic.
    /// </summary>
    public class CarLightingT11 :MonoBehaviour
    {
#pragma warning disable 0649

        [SerializeField] float TurnsSwitchHalfRepeatTime = 0.5f;   //Half time light on/off.

#pragma warning restore 0649

        //All light is searched for in child elements, 
        //depending on the set tag, the light gets into the desired list.
        public List<LightObjectT11> MainLights = new List<LightObjectT11>();
        public List<LightObjectT11> LeftTurnLights = new List<LightObjectT11>();
        public List<LightObjectT11> RightTurnLights = new List<LightObjectT11>();
        public List<LightObjectT11> BrakeLights = new List<LightObjectT11>();
        public List<LightObjectT11> ReverseLights = new List<LightObjectT11>();
        public List<LightObjectT11> DiscHeats = new List<LightObjectT11>();

        CarController _Car;
        //Used property, to be able to connect the trailer to the vehicle.
        public CarController Car 
        { 
            get 
            { 
                return _Car; 
            }
            set
            {
                if (_Car != null)
                {
                    Car.OnChangeGearAction -= OnChangeGear;
                    OnChangeGear (0);
                }
                _Car = value;

                if (_Car != null)
                {
                    Car.OnChangeGearAction += OnChangeGear;
                }
            }
        }

        bool InBrake;
        bool InBrakeDisc;
        bool MainLightsIsOn;
        Coroutine TurnsCotoutine;
        public List<LightObjectT11> ActiveTurns = new List<LightObjectT11>();
        TurnsStatesT11 CurrentTurnsState = TurnsStatesT11.Off;

        public event System.Action<CarLightTypeT11, bool> OnSetActiveLight;

        public CarLightingT11 AdditionalLighting { get; set; }

        void Start ()
        {
            //Searching and distributing all lights.
            var lights = GetComponentsInChildren<LightObjectT11>();
            foreach (var l in lights)
            {
                switch (l.CarLightTypeT11)
                {
                    case CarLightType.Main:
                    MainLights.Add (l); break;
                    case CarLightType.TurnLeft:
                    LeftTurnLights.Add (l);
                    break;
                    case CarLightType.TurnRight:
                    RightTurnLights.Add (l);
                    break;
                    case CarLightType.Brake:
                    BrakeLights.Add (l);
                    break;
                    case CarLightType.Reverse:
                    ReverseLights.Add (l);
                    break;
                    case CarLightType.DiscHeat:
                    DiscHeats.Add(l);
                    break;

                }
            }

            Car = GetComponent<CarController> ();

            //Initializing soft light switching.
            InitSoftSwitches (MainLights);
            InitSoftSwitches (ReverseLights);
            InitSoftSwitches (BrakeLights);
            InitSoftSwitches (LeftTurnLights);
            InitSoftSwitches (RightTurnLights);
            InitSoftSwitches(DiscHeats);
        }

        private void Update ()
        {
            bool carInBrake = Car != null && Car.CurrentBrake > 0;
            if (InBrake != carInBrake)
            {
                InBrake = carInBrake;
                SetActiveBrake (InBrake);
            }

            bool carInBrakeDiscHeat = Car != null && Car.CurrentBrake > 0.75f;
            if (InBrakeDisc != carInBrakeDiscHeat)
            {
                InBrakeDisc = carInBrakeDiscHeat;
                SetActiveDiscHeat(InBrakeDisc);
            }
        }

        /// <summary>
        /// Initiates soft switching of the light as needed.
        /// </summary>
        void InitSoftSwitches (List<LightObjectT11> lights)
        {
            foreach (var light in lights)
            {
                light.TryInitSoftSwitch ();
            }
        }

        /// <summary>
        /// Reverse light switch logic.
        /// </summary>
        public void OnChangeGear (int gear)
        {
            SetActiveReverse (gear < 0);
        }

        public void SwithOffAllLights ()
        {
            SetActiveMainLights (false);
            SetActiveBrake (false);
            SetActiveDiscHeat(false);
            SetActiveReverse (false);
            TurnsEnable (TurnsStatesT11.Off);
        }

        /// <summary>
        /// Main light switch.
        /// </summary>
        public void SwitchMainLights ()
        {
            if (MainLights.Count > 0)
            {
                MainLightsIsOn = !MainLightsIsOn;
                SetActiveMainLights (MainLightsIsOn);
            }
        }

        public void SetActiveMainLights (bool value)
        {
            MainLights.ForEach (l => l.Switch (value));

            OnSetActiveLight.SafeInvoke (CarLightTypeT11.Main, value);

            if (AdditionalLighting)
            {
                AdditionalLighting.SetActiveMainLights (value);
            }
        }

        public void SetActiveBrake (bool value)
        {
            BrakeLights.ForEach (l => l.Switch (value));

            OnSetActiveLight.SafeInvoke (CarLightTypeT11.Brake, value);

            if (AdditionalLighting)
            {
                AdditionalLighting.SetActiveBrake (value);
            }
        }

        public void SetActiveDiscHeat(bool value)
        {
            DiscHeats.ForEach(l => l.Switch(value));

            OnSetActiveLight.SafeInvoke(CarLightTypeT11.DiscHeat, value);

            if (AdditionalLighting)
            {
                AdditionalLighting.SetActiveDiscHeat(value);
            }
        }

        public void SetActiveReverse (bool value)
        {
            ReverseLights.ForEach (l => l.Switch (value));

            OnSetActiveLight.SafeInvoke (CarLightTypeT11.Reverse, value);

            if (AdditionalLighting)
            {
                AdditionalLighting.SetActiveReverse (value);
            }
        }

        /// <summary>
        /// Turns lights switch logic.
        /// </summary>
        public void TurnsEnable (TurnsStatesT11 state)
        {
            TurnsDisable ();

            if (CurrentTurnsState != state)
            {
                CurrentTurnsState = state;
                TurnsCotoutine = StartCoroutine (DoTurnsEnable (CurrentTurnsState));
            }
            else
            {
                switch (CurrentTurnsState)
                {
                    case TurnsStatesT11.Left: OnSetActiveLight.SafeInvoke (CarLightTypeT11.TurnLeft, false); break;
                    case TurnsStatesT11.Right: OnSetActiveLight.SafeInvoke (CarLightTypeT11.TurnRight, false); break;
                    case TurnsStatesT11.Alarm: OnSetActiveLight.SafeInvoke (CarLightTypeT11.TurnLeft | CarLightTypeT11.TurnRight, false); break;
                }

                CurrentTurnsState = TurnsStatesT11.Off;
            }

            if (AdditionalLighting)
            {
                AdditionalLighting.TurnsEnable (state);
            }
        }

        /// <summary>
        /// Turn off blinking of turn signals.
        /// </summary>
        void TurnsDisable ()
        {
            if (TurnsCotoutine != null)
            {
                StopCoroutine (TurnsCotoutine);
            }
            ActiveTurns.ForEach (l => l.Switch (false));
        }

        /// <summary>
        /// Turn signals IEnumerator.
        /// </summary>
        IEnumerator DoTurnsEnable (TurnsStatesT11 state)
        {
            ActiveTurns = new List<LightObjectT11> ();

            switch (state)
            {
                case TurnsStatesT11.Left:
                ActiveTurns = LeftTurnLights;
                OnSetActiveLight.SafeInvoke (CarLightTypeT11.TurnLeft, true);
                break;

                case TurnsStatesT11.Right:
                ActiveTurns = RightTurnLights;
                OnSetActiveLight.SafeInvoke (CarLightTypeT11.TurnRight, true);
                break;

                case TurnsStatesT11.Alarm:
                ActiveTurns.AddRange (LeftTurnLights);
                ActiveTurns.AddRange (RightTurnLights);
                OnSetActiveLight.SafeInvoke (CarLightTypeT11.TurnLeft | CarLightTypeT11.TurnRight, true);
                break;
            }

            //Infinite cycle of switching on and off.
            while (true)
            {
                ActiveTurns.ForEach (l => l.Switch (true));
                yield return new WaitForSeconds (TurnsSwitchHalfRepeatTime);
                ActiveTurns.ForEach (l => l.Switch (false));
                yield return new WaitForSeconds (TurnsSwitchHalfRepeatTime);
            }
        }
    }

    public enum TurnsStatesT11
    {
        Off,
        Left,
        Right,
        Alarm
    }

    public enum CarLightTypeT11
    {
        Main,
        Brake,
        TurnLeft,
        TurnRight,
        Reverse,
        DiscHeat
    }
}
