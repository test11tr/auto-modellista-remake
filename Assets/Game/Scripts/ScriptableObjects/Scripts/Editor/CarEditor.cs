using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace test11
{
    [CustomEditor(typeof(Car))]
    public class CarEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(10);
            Car car = target as Car;

            if (GUILayout.Button("Reset Car to Default Parameters"))
            {
                ResetCarParameters(car);
            }

            if (GUILayout.Button("Reset Car Ownership and Personalizations"))
            {
                ResetCarOwnershipAndPersonalizations(car);
            }
        }

        private void ResetCarParameters(Car car)
        {
            car.carName = "";
            car.carDescription = "";
            car.carPrice = 0;
            car.carAcceleration = 0;
            car.carSpeed = 0;
            car.carHandling = 0;
            car.carBreaking = 0;
            car.carMaxSpeed = 0;
            car.car0100Time = 0;
            car.carHorsePower = 0;
            car.carVisualPrefab = null;
            car.carPlayablePrefab = null;
            EditorUtility.SetDirty(car);
        }

        private void ResetCarOwnershipAndPersonalizations(Car car)
        {
            car.isOwned = false;
            car.colorIndex = -1;
            car.baseMapColor = Color.black;
            car.firstShadingMapColor = Color.black;
            car.secondShadingMapColor = Color.black;
            car.highlightColor = Color.black;
            car.rimLightColor = Color.black;
            EditorUtility.SetDirty(car);
        }
    }
}