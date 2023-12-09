using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace test11
{
    [CreateAssetMenu(fileName = "New Car", menuName = "T11/CarData")]
    public class Car : ScriptableObject
    {
        [Header("Car Info")]
        public string carIndex;
        public int carNumberIndex;
        public string carName;
        public string carDescription;
        public Sprite vehicleClass;
        public Sprite vehicleBrand;

        [Header("Car Stats")]
        public int carPrice;
        public float carAcceleration;
        public float carSpeed;
        public float carHandling;
        public float carBreaking;
        public float carMaxSpeed;
        public float car0100Time;
        public float carHorsePower;

        [Header("Car References")]
        public GameObject carVisualPrefab;
        public GameObject carPlayablePrefab;

        [Header("Car Personalization Data")]
        [SerializeField] public bool isOwned;
        [SerializeField] public int colorIndex = -1;
        [SerializeField] public Color baseMapColor;
        [SerializeField] public Color firstShadingMapColor;
        [SerializeField] public Color secondShadingMapColor;
        [SerializeField] public Color highlightColor;
        [SerializeField] public Color rimLightColor;
    }
}
