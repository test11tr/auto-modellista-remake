using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace PG
{
    /// <summary>
    /// This component is for demonstration purposes only. You can replace this component with any third party asset.
    /// </summary>
    public class GameManagerT11 : Singleton<GameManagerT11>
    {
        //VehicleList
        [Header("Vehicle References")]
        [SerializeField] private ScriptableObject[] _scriptableObjects;

        //Game Manager Logic
        [Header("Game Scene Settings")]
        public bool startWithVehicle = false;
        public GameObject playerVehicle;
        public Transform VehicleSpawnPoint;
        bool isInVehicle;

        //Player Controller
        [Header("Player Controller Settings")]
        public CharacterController CharacterController;
        public Transform PlayerSpawnPoint;
        public Transform CameraParent;
        public CharacterInput Input;
        public float ChangeCameraSpeed = 5;
        public float CameraSensitivity = 5;
        public float MaxSpeed = 5;
        float CameraVerticlaAngle = 0;

        [Header("Automatic")]
        public Camera Camera;
        public Transform CameraParentInCar;
        public PlayerController PlayerControllerForCar;

        [Header("Editor")]
        [SerializeField] bool drawGizmos = true;

        void Start ()
        {
            if (Input == null)
            {
                Debug.LogError ("Input for CharacterController is null");
            }
            if (startWithVehicle)
            {
                HandleVehicleCharacter();
            }
            else
            {
                HandlePlayerCharacter();
            }  
        }

        void HandleVehicleCharacter()
        {
            isInVehicle = true;
        }

        void HandlePlayerCharacter()
        {
            isInVehicle = false;
            CharacterController.transform.position = PlayerSpawnPoint.position;
            CharacterController.transform.rotation = Quaternion.LookRotation(PlayerSpawnPoint.transform.forward, Vector3.up);
            Input.OnEntrerInCar += TryEnterCar;

            //Search or create camera logic.
            if (Camera == null)
            {
                Camera = Camera.main;
                string mainCameraTag = "MainCamera";

                if (Camera == null)
                {
                    //Search for all cameras, in hidden objects too, if Camera.main == null.
                    var cameras = FindObjectsOfType<Camera>(true);
                    foreach (var camera in cameras)
                    {
                        if (camera.tag == mainCameraTag)
                        {
                            Camera = camera;
                            break;
                        }
                    }
                }

                if (Camera == null)
                {
                    //Create camera
                    Camera = Instantiate(B.ResourcesSettings.UVCMainCamera);
                    Camera.tag = mainCameraTag;
                }

                Camera.transform.SetParent(CameraParent);
                Camera.transform.localPosition = Vector3.zero;
                Camera.transform.localRotation = Quaternion.identity;
            }

            SoundHelper.TryAddAudioListiner(gameObject);
        }

        private void OnEnable ()
        {
            Input.SetActive (true);
        }

        private void OnDisable ()
        {
            Input.SetActive (false);
            CameraVerticlaAngle = 0;
            StopAllCoroutines ();
        }

        void Update ()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            //Move character
            Vector3 moveDelta = Input.MoveInput.y * CharacterController.transform.forward;
            moveDelta += Input.MoveInput.x * CharacterController.transform.right;
            CharacterController.SimpleMove (moveDelta * MaxSpeed);

            //Rotate character and camera
            Vector2 viewDelta = Input.ViewInput;

            CharacterController.transform.rotation *= Quaternion.AngleAxis(viewDelta.x, Vector3.up);

            //Rotate the Camera by Vertical axis
            CameraVerticlaAngle = (CameraVerticlaAngle - viewDelta.y).Clamp (-45, 45);
            Camera.transform.localRotation = Quaternion.AngleAxis (CameraVerticlaAngle, Vector3.right);
        }

        public void TryEnterCar ()
        {
            if (PlayerControllerForCar == null)
            {
                PlayerControllerForCar = PlayerController.GetOrCreatePlayerController();
            }

            if (CameraParentInCar == null)
            {
                var carCameraController = PlayerControllerForCar.GetComponentInChildren<CameraController> (true);
                CameraParentInCar = carCameraController.CameraParentTransform;
            }

            CarController car;
            RaycastHit hit;

            if (Physics.Raycast (Camera.transform.position, Camera.transform.forward, out hit, 2))
            {
                car = hit.collider.GetComponentInParent<CarController> ();
                if (car != null)
                {
                    isInVehicle = false;
                    gameObject.SetActive (false);
                    PlayerControllerForCar.EnterInCar (car);
                    PlayerControllerForCar.OnExitAction += OnExitFromCar;
                    Camera.transform.SetParent (CameraParentInCar);

                    Camera.transform.localPosition = Vector3.zero;
                    Camera.transform.localRotation = Quaternion.identity;
                }
            }
        }

        public void OnExitFromCar (CarController car)
        {
            PlayerControllerForCar.OnExitAction -= OnExitFromCar;

            Vector3 offsetPos = car.transform.right * (car.Bounds.size.x / 2 + CharacterController.radius);

            if (car.SteerWheel != null)
            {
                offsetPos *= Mathf.Sign (car.SteerWheel.localPosition.x);
            }

            offsetPos += Vector3.up * CharacterController.height * 0.5f;

            transform.position = car.transform.position + offsetPos;
            transform.rotation = Quaternion.LookRotation (car.transform.forward.ZeroHeight (), Vector3.up);
            gameObject.SetActive (true);

            if (!Camera)
            {
                Camera = Camera.main;
            }

            if (Camera)
            {
                Camera.transform.SetParent(CameraParent);
                Camera.transform.localPosition = Vector3.zero;
                Camera.transform.localRotation = Quaternion.identity;
            }
        }

        void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(VehicleSpawnPoint.transform.position, 0.1f);
                Gizmos.DrawRay(VehicleSpawnPoint.transform.position, transform.position + VehicleSpawnPoint.transform.forward * 1f);

                Vector3 vehright = Quaternion.LookRotation(VehicleSpawnPoint.transform.forward) * Quaternion.Euler(0, 180 + 20, 0) * new Vector3(0, 0, 1);
                Vector3 vehleft = Quaternion.LookRotation(VehicleSpawnPoint.transform.forward) * Quaternion.Euler(0, 180 - 20, 0) * new Vector3(0, 0, 1);
                Gizmos.DrawRay(VehicleSpawnPoint.transform.position + VehicleSpawnPoint.transform.forward, vehright * 0.25f);
                Gizmos.DrawRay(VehicleSpawnPoint.transform.position + VehicleSpawnPoint.transform.forward, vehleft * 0.25f);
                Handles.Label(VehicleSpawnPoint.transform.position + Vector3.up * .2f, "Vehicle Spawn Position");

                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(PlayerSpawnPoint.transform.position, 0.1f);
                Gizmos.DrawRay(PlayerSpawnPoint.transform.position, transform.position + PlayerSpawnPoint.transform.forward * 1f);

                Vector3 plyright = Quaternion.LookRotation(PlayerSpawnPoint.transform.forward) * Quaternion.Euler(0, 180 + 20, 0) * new Vector3(0, 0, 1);
                Vector3 plyleft = Quaternion.LookRotation(PlayerSpawnPoint.transform.forward) * Quaternion.Euler(0, 180 - 20, 0) * new Vector3(0, 0, 1);
                Gizmos.DrawRay(PlayerSpawnPoint.transform.position + PlayerSpawnPoint.transform.forward, plyright * 0.25f);
                Gizmos.DrawRay(PlayerSpawnPoint.transform.position + PlayerSpawnPoint.transform.forward, plyleft * 0.25f);
                Handles.Label(PlayerSpawnPoint.transform.position + Vector3.up * .2f, "Player Spawn Position");
            }

        }
    }
}
