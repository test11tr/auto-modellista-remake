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
        [SerializeField] private bool isOwned;
        [SerializeField] private Color baseMapColor;
        [SerializeField] private Color firstShadingMapColor;
        [SerializeField] private Color secondShadingMapColor;
        [SerializeField] private Color highlightColor;
        [SerializeField] private Color rimLightColor;
    }
}
