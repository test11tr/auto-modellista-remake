using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;
using test11;

namespace PG
{
    public class GameManagerT11 : Singleton<GameManagerT11>
    {
        //VehicleList
        [Header("Scene Definiton")]
        [Tooltip("Araçlarla olacak etkileşim burada seçilen sahne tipine göre çalışmaktadır. Doğru ayarlanabilmesi önemlidir.")]
        public SceneTypes sceneType;
        public enum SceneTypes
        {
            OpenWorld,
            Race,
            Garage,
            VehicleDealer,
            VehicleTuner
        }

        //VehicleList
        [Header("Player Vehicles")]
        [SerializeField] private Car[] _playerCars;
        [Header("Override Vehicle")]
        [Tooltip("Buraya bir araç koyulursa sahne açıldığında kullanıcının aracı yerine koyulan araç spawn edilir.")]
        [SerializeField] private Car _overrideVehicle;

        //Game Manager Logic
        [Header("Game Scene Settings")]
        [Tooltip("Seçenek işaretliyse bölüme araç kontrolcüsü ile başlanılır.")]
        public bool startWithVehicle = false;
        [Tooltip("Seçenek işaretliyse bölüme araç spawn edilir ancak first person kontrolcü ile başlanılır.")]
        public bool startInVehicle = false;
        [Tooltip("Seçenek işaretliyse bölümde araçtan inilemez.")]
        public bool canExitVehicle = false;
        public Transform VehicleSpawnPoint;
        [HideInInspector] public bool isInVehicle;
        private int currentCarIndex;
        private GameObject p_spawnedPlayerVehicle;
        public GameObject SpawnedPlayerVehicle => p_spawnedPlayerVehicle;

        //Player Controller
        [Header("Player Controller Settings")]
        public CharacterController CharacterController;
        public Transform PlayerSpawnPoint;
        public Transform CameraParent;
        public CharacterInput Input;
        public HeadbobSystem headbobSystem;
        public float ChangeCameraSpeed = 5;
        public float CameraSensitivity = 5;
        public float WalkSpeed = 5;
        public float RunSpeed = 8;
        float CameraVerticlaAngle = 0;
        public bool inputMute;

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

        void Update()
        {
            if (isInVehicle)
            {
                HandleInteractorInVehicle();
            }
            else
            {
                HandleInteractorAndPlayerControls();
            }
            
            /// FOR DEBUG PURPOSES
            /*if (UnityEngine.Input.GetKeyDown(KeyCode.M))
            {
                inputMute = !inputMute;
                print(inputMute);
            }*/
        }

        public void HandleInputMute(bool boolean)
        {
            inputMute = boolean;
        }

        void HandleInteractorInVehicle()
        {
            Interactor.Instance.transform.position = p_spawnedPlayerVehicle.transform.position;
            Interactor.Instance.CheckForInteraction(6);
        }

        void HandleInteractorAndPlayerControls()
        {
            Interactor.Instance.transform.position = CharacterController.transform.position;
            Interactor.Instance.CheckForInteraction(-1);

            if (CharacterController.gameObject.activeSelf && !inputMute)
            {
                //Move character
                Vector3 moveDelta = Input.MoveInput.y * CharacterController.transform.forward;
                moveDelta += Input.MoveInput.x * CharacterController.transform.right;
                if (Input.isRunning)
                {
                    headbobSystem.isRunning = true;
                    CharacterController.SimpleMove(moveDelta * RunSpeed);
                }
                else
                {
                    headbobSystem.isRunning = false;
                    CharacterController.SimpleMove(moveDelta * WalkSpeed);
                }   

                //Rotate character and camera
                Vector2 viewDelta = Input.ViewInput;
                CharacterController.transform.rotation *= Quaternion.AngleAxis(viewDelta.x, Vector3.up);

                //Rotate the Camera by Vertical axis
                CameraVerticlaAngle = (CameraVerticlaAngle - viewDelta.y).Clamp(-45, 45);
                Camera.transform.localRotation = Quaternion.AngleAxis(CameraVerticlaAngle, Vector3.right);
            }
        }

        void HandleVehicleCharacter()
        {
            isInVehicle = true;
            currentCarIndex = PlayerPrefs.GetInt("CurrentCar");

            if (_overrideVehicle == null)
                p_spawnedPlayerVehicle = Instantiate(_playerCars[currentCarIndex].carPlayablePrefab, VehicleSpawnPoint.position, Quaternion.identity);
            else
                p_spawnedPlayerVehicle = Instantiate(_overrideVehicle.carPlayablePrefab, VehicleSpawnPoint.position, Quaternion.identity);

            p_spawnedPlayerVehicle.transform.rotation = Quaternion.LookRotation(VehicleSpawnPoint.transform.forward, Vector3.up);

            HandlePlayerCharacter();
            if (startInVehicle)
            {
                EnterCarOnStart();
            }
        }

        void HandlePlayerCharacter()
        {
            isInVehicle = false;
            CharacterController.transform.position = PlayerSpawnPoint.position;
            CharacterController.transform.rotation = Quaternion.LookRotation(PlayerSpawnPoint.transform.forward, Vector3.up);

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
            SoundHelper.TryAddAudioListiner(CharacterController.gameObject);
        }

        public void TryEnterCar ()
        {
            Collider[] colliderArray = Physics.OverlapSphere(Camera.transform.position, 2);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out CarController car))
                {
                    isInVehicle = true;

                    if (PlayerControllerForCar == null)
                    {
                        PlayerControllerForCar = PlayerController.GetOrCreatePlayerController();
                    }

                    if (CameraParentInCar == null)
                    {
                        var carCameraController = PlayerControllerForCar.GetComponentInChildren<CameraController>(true);
                        CameraParentInCar = carCameraController.CameraParentTransform;
                    }

                    //gameObject.SetActive (false);
                    CharacterController.gameObject.SetActive(false);
                    Input.SetActive(false);
                    CameraVerticlaAngle = 0;
                    StopAllCoroutines();

                    PlayerControllerForCar.EnterInCar (car);
                    PlayerControllerForCar.OnExitAction += OnExitFromCar;
                    Camera.transform.SetParent (CameraParentInCar);

                    Camera.transform.localPosition = Vector3.zero;
                    Camera.transform.localRotation = Quaternion.identity;
                }
            }
        }

        public void EnterCarOnStart()
        {
            if (PlayerControllerForCar == null)
            {
                PlayerControllerForCar = PlayerController.GetOrCreatePlayerController();
            }

            if (CameraParentInCar == null)
            {
                var carCameraController = PlayerControllerForCar.GetComponentInChildren<CameraController>(true);
                CameraParentInCar = carCameraController.CameraParentTransform;
            }

            //gameObject.SetActive(false);
            CharacterController.gameObject.SetActive(false);
            Input.SetActive(false);
            CameraVerticlaAngle = 0;
            StopAllCoroutines();
            isInVehicle = true;

            PlayerControllerForCar.EnterInCar(p_spawnedPlayerVehicle.GetComponent<CarController>());
            PlayerControllerForCar.OnExitAction += OnExitFromCar;
            Camera.transform.SetParent(CameraParentInCar);

            Camera.transform.localPosition = Vector3.zero;
            Camera.transform.localRotation = Quaternion.identity;
                
            
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

            CharacterController.gameObject.transform.position = car.transform.position + offsetPos;
            CharacterController.gameObject.transform.rotation = Quaternion.LookRotation (car.transform.forward.ZeroHeight (), Vector3.up);
            //gameObject.SetActive (true);
            CharacterController.gameObject.SetActive(true);
            Input.SetActive(true);
            isInVehicle = false;

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
