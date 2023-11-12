using PG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using test11;
using UnityEngine.UI;

namespace test11
{
    public class MenuManagerT11 : Singleton<MenuManagerT11>
    {
        [Header("Dealer, Vehicle Inspector")]
        public GameObject DealerVehicleInspectMenu;
        public TMP_Text vehicleName;
        public TMP_Text vehicleDescription;
        public Image vehicleClass;
        public Image vehicleBrand;
        public Image carAcceleration;
        public Image carSpeed;
        public Image carHandling;
        public Image carBreaking;
        public TMP_Text carMaxSpeed;
        public TMP_Text car0100Time;
        public TMP_Text carHorsePower;
    }
}
