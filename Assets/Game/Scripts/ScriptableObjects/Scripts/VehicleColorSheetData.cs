using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace test11
{
    [CreateAssetMenu(fileName = "New Color Sheet Data", menuName = "T11/ColorData")]
    public class VehicleColorSheetData : ScriptableObject
    {
        [Header("Color Sheets")]
        public ColorSheetData[] colorSheetData;

        [Serializable]
        public struct ColorSheetData
        {
            public string colorName;
            public Color UIButtonColor;
            public Color baseMapColor;
            public Color firstShadingMapColor;
            public Color secondShadingMapColor;
            public Color highlightColor;
            public Color rimLightColor;
        }
    }
}
