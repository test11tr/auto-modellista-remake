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
        public TMP_Text vehiclePrice;
        public Image vehicleClass;
        public Image vehicleBrand;
        public Image carAcceleration;
        public Image carSpeed;
        public Image carHandling;
        public Image carBreaking;
        public TMP_Text carMaxSpeed;
        public TMP_Text car0100Time;
        public TMP_Text carHorsePower;
        public Button buyButton;
        public Button purchasedButton;

        public void ManageVehiclePage(Car CarData)
        {
            DealerVehicleInspectMenu.SetActive(true);
            vehicleName.text = "• " + CarData.carName;
            vehicleDescription.text = CarData.carDescription;
            vehiclePrice.text = "<style=\"Orange\">$</style>" + CarData.carPrice.ToString();
            vehicleClass.sprite = CarData.vehicleClass;
            vehicleBrand.sprite = CarData.vehicleBrand;
            carAcceleration.fillAmount = CarData.carAcceleration / 10;
            carSpeed.fillAmount = CarData.carSpeed / 10;
            carHandling.fillAmount = CarData.carHandling / 10;
            carBreaking.fillAmount = CarData.carBreaking / 10;
            carMaxSpeed.text = CarData.carMaxSpeed.ToString() + " Kmh";
            car0100Time.text = CarData.car0100Time.ToString() + " Sec.";
            carHorsePower.text = CarData.carHorsePower.ToString() + " Bhp";

            if (PlayerPrefs.GetInt(CarData.carIndex) == 1)
            {
                buyButton.SetActive(false);
                purchasedButton.SetActive(true);
                vehiclePrice.text = "- $";
            }
            else if (PlayerPrefs.GetInt(CarData.carIndex) == 0)
            {
                buyButton.SetActive(true);
                purchasedButton.SetActive(false);
            }
        }
    }
}
