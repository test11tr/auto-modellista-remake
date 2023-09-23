using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

       [Header("Car Stats")]
       public int carPrice;
       public float carSpeed;
       public float carAcceleration;
       public float carHandling;

       [Header("Car References")]
       public GameObject carVisualPrefab;
       public GameObject carPlayablePrefab;
    }
}
