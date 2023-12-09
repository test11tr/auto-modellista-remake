
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using test11;

namespace PG
{
    /// <summary>
    /// Main car controller component. 
    /// It is partial, it also has two parts Engine, Transmission and Steering (For better code readability).
    /// </summary>
    [RequireComponent (typeof (Rigidbody))]
    public partial class CarController :VehicleController, IInteractable
    {
        [Header("CarController")]

        public LayerMask TrailerConnectorMask;
        public Transform TrailerConnectorPosition;

        public float SteerWheelMaxAngle;                                                //Maximum angle of steering wheel rotation (Visual only).
        public Transform SteerWheel;

        float SteerWheelStartXAngle;

        public event System.Action OnConnectTrailer;                                    //Actions to be taken when TrailerController is connected to a vehicle.
        public bool CanConnectTrailer { get; private set; }
        public TrailerController ConnectedTrailer { get; private set; }
        public TrailerController NearestTrailer { get; private set; }
        public ICarControl CarControl { get; set; }                                     //ICarControll controls the car.
        public bool BlockControl { get; protected set; }                                //Blocks input.

        [Header("Interaction Text Data")]
        public InteractionText _interactTextData;
        public Car _CarData;
        string interactionText;

        [Header("PaintData")]
        public VehicleColorSheetData _vehicleColorSheetData;
        public Material paintMaterial;
        Material instancePaintMaterial;
        public bool isTutorialVehicle;
        public int tutorialVehicleColorIndex;

        protected override void Awake ()
        {
            base.Awake ();

            if (CarControl == null)
            {
                CarControl = GetComponent<ICarControl> ();
            }

            //Calling Awake in other parts of the component.
            AwakeTransmition ();
            AwakeEngine ();
            AwakeSteering ();

            if (SteerWheel)
            {
                SteerWheelStartXAngle = SteerWheel.localRotation.eulerAngles.x;
            }

            instancePaintMaterial = paintMaterial;
            paintVehicle();
        }

        protected override void FixedUpdate ()
        {
            base.FixedUpdate ();

            //Calling FixedUpdate in other parts of the component.
            FixedUpdateEngine ();
            FixedUpdateTransmition ();
            FixedUpdateBrakeLogic ();
            FixedUpdateSteering ();

            //Steering wheel rotation.
            if (SteerWheel != null)
            {
                SteerWheel.transform.localRotation = Quaternion.AngleAxis (SteerWheelStartXAngle, Vector3.right);
                SteerWheel.transform.localRotation *= Quaternion.AngleAxis ((CurrentSteerAngle / Steer.MaxSteerAngle) * SteerWheelMaxAngle, Vector3.back);
            }
        }

        protected override void OnTriggerEnter (Collider other)
        {
            if (TrailerConnectorMask.LayerInMask (other.gameObject.layer) && other.attachedRigidbody)
            {
                CanConnectTrailer = true;
                NearestTrailer = other.attachedRigidbody.GetComponent<TrailerController> ();
            }
        }

        protected override void OnTriggerExit (Collider other)
        {
            if (ConnectedTrailer == null && TrailerConnectorMask.LayerInMask (other.gameObject.layer))
            {
                CanConnectTrailer = false;
                NearestTrailer = null;
            }
        }

        public virtual void TryConnectDisconnectTrailer ()
        {
            if (!ConnectedTrailer && !NearestTrailer)
            {
                return;
            }

            if (ConnectedTrailer)
            {
                ConnectedTrailer.ConnectVehicle (null);
                ConnectedTrailer = null;
            }
            else
            {
                ConnectedTrailer = NearestTrailer;
                ConnectedTrailer.ConnectVehicle (this);
            }

            OnConnectTrailer.SafeInvoke ();
        }

        /// <summary>
        /// Reset car logic.
        /// TODO Add a car reset on the way.
        /// </summary>
        public override void ResetVehicle ()
        {
            base.ResetVehicle ();

            EngineRPM = Engine.MinRPM;
            CurrentGear = 0;
        }

        public void EnterInCar ()
        {
            var playerController = PlayerController.GetOrCreatePlayerController ();
            playerController.EnterInCar (this);
        }

        public void ExitFromCar ()
        {
            if (PlayerController.Instance)
            {
                PlayerController.Instance.ExitFromCar ();
            }
        }

        //INTERACT INTERFACE

        public void Interact()
        {
            if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.OpenWorld)
            {
                GameManagerT11.Instance.TryEnterCar();
            }
            else if(GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.VehicleDealer)
            {
                MenuManagerT11.Instance.ManageVehicleInspectorPage(_CarData);
                GameManagerT11.Instance.HandleInputMute(true);
                Interactor.Instance.isInteractCheckable = false;
            }
        }

        public string GetInteractionText()
        {
            if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.Garage)
            {
                return interactionText = _interactTextData.InGarageText;
            }
            else if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.VehicleTuner)
            {
                return interactionText = _interactTextData.InTunerText;
            }
            else if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.VehicleDealer)
            {
                return interactionText = _interactTextData.InDealerText;
            }
            else if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.Race)
            {
                return interactionText = _interactTextData.InRaceAreaText;
            }
            else if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.OpenWorld)
            {
                return interactionText = _interactTextData.InOpenWorldText;
            }

            return interactionText = _interactTextData.defaultText;
        }

        public void paintVehicle()
        {
            if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.VehicleDealer)
            {
                instancePaintMaterial.SetColor(Shader.PropertyToID("_BaseColor"), _vehicleColorSheetData.colorSheetData[0].baseMapColor);
                instancePaintMaterial.SetColor(Shader.PropertyToID("_1st_ShadeColor"), _vehicleColorSheetData.colorSheetData[0].firstShadingMapColor);
                instancePaintMaterial.SetColor(Shader.PropertyToID("_2nd_ShadeColor"), _vehicleColorSheetData.colorSheetData[0].secondShadingMapColor);
                instancePaintMaterial.SetColor(Shader.PropertyToID("_HighColor"), _vehicleColorSheetData.colorSheetData[0].highlightColor);
                instancePaintMaterial.SetColor(Shader.PropertyToID("_RimLightColor"), _vehicleColorSheetData.colorSheetData[0].rimLightColor);
            }
            else
            {
                if (_CarData.isOwned)
                {
                    instancePaintMaterial.SetColor(Shader.PropertyToID("_BaseColor"), _CarData.baseMapColor);
                    instancePaintMaterial.SetColor(Shader.PropertyToID("_1st_ShadeColor"), _CarData.firstShadingMapColor);
                    instancePaintMaterial.SetColor(Shader.PropertyToID("_2nd_ShadeColor"), _CarData.secondShadingMapColor);
                    instancePaintMaterial.SetColor(Shader.PropertyToID("_HighColor"), _CarData.highlightColor);
                    instancePaintMaterial.SetColor(Shader.PropertyToID("_RimLightColor"), _CarData.rimLightColor);
                }
                else if (isTutorialVehicle)
                {
                    instancePaintMaterial.SetColor(Shader.PropertyToID("_BaseColor"), _vehicleColorSheetData.colorSheetData[tutorialVehicleColorIndex].baseMapColor);
                    instancePaintMaterial.SetColor(Shader.PropertyToID("_1st_ShadeColor"), _vehicleColorSheetData.colorSheetData[tutorialVehicleColorIndex].firstShadingMapColor);
                    instancePaintMaterial.SetColor(Shader.PropertyToID("_2nd_ShadeColor"), _vehicleColorSheetData.colorSheetData[tutorialVehicleColorIndex].secondShadingMapColor);
                    instancePaintMaterial.SetColor(Shader.PropertyToID("_HighColor"), _vehicleColorSheetData.colorSheetData[tutorialVehicleColorIndex].highlightColor);
                    instancePaintMaterial.SetColor(Shader.PropertyToID("_RimLightColor"), _vehicleColorSheetData.colorSheetData[tutorialVehicleColorIndex].rimLightColor);
                }
            }
        }
    }

    /// <summary>
    /// Car control interface. Suitable for creating AI.
    /// </summary>
    public interface ICarControl
    {
        float Acceleration { get; }
        float BrakeReverse { get; }
        float Horizontal { get; }
        float Pitch { get; }
        bool HandBrake { get; }
        bool Boost { get; }
    }
}
