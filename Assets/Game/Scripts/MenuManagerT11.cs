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
        /// <summary>
        /// _im means Inspector Menu
        /// _bs means Buy Screen Menu
        /// </summary>

        [Header("Dealer, Vehicle Inspector")]
        public GameObject DealerVehicleInspectMenu;
        public TMP_Text vehicleName_im;
        public TMP_Text vehicleDescription_im;
        public TMP_Text vehiclePrice_im;
        public Image vehicleClass_im;
        public Image vehicleBrand_im;
        public Image vehicleBrand2_im;
        public Image carAcceleration_im;
        public Image carSpeed_im;
        public Image carHandling_im;
        public Image carBreaking_im;
        public TMP_Text carMaxSpeed_im;
        public TMP_Text car0100Time_im;
        public TMP_Text carHorsePower_im;
        public Button buyButton_im;
        public Button purchasedButton_im;

        [Header("Dealer, Vehicle BuyScreen")]
        public GameObject DealerVehicleBuyScreenMenu;
        public TMP_Text vehicleName_bs;
        public TMP_Text vehiclePrice_bs;
        public Image vehicleClass_bs;
        public Image vehicleBrand_bs;
        public TMP_Text carMaxSpeed_bs;
        public TMP_Text car0100Time_bs;
        public TMP_Text carHorsePower_bs;
        public TMP_Text confirmPriceText_bs;
        public TMP_Text congratsText_bs;
        public Button buyButton_bs;
        
        [Header("Color Page Details")]
        public VehicleColorSheetData _vehicleColorSheetData;
        public GameObject ColorPageParent;
        int selectedColor;

        [HideInInspector] public Car _tempdata;

        public void ManageVehicleInspectorPage(Car CarData)
        {
            _tempdata = (Car)CarData;
            DealerVehicleInspectMenu.SetActive(true);
            vehicleName_im.text = "• " + CarData.carName;
            vehicleDescription_im.text = CarData.carDescription;
            vehiclePrice_im.text = "<style=\"Orange\">$</style>" + CarData.carPrice.ToString();
            vehicleClass_im.sprite = CarData.vehicleClass;
            vehicleBrand_im.sprite = CarData.vehicleBrand;
            vehicleBrand2_im.sprite = CarData.vehicleBrand;
            carAcceleration_im.fillAmount = CarData.carAcceleration / 10;
            carSpeed_im.fillAmount = CarData.carSpeed / 10;
            carHandling_im.fillAmount = CarData.carHandling / 10;
            carBreaking_im.fillAmount = CarData.carBreaking / 10;
            carMaxSpeed_im.text = CarData.carMaxSpeed.ToString() + " Kmh";
            car0100Time_im.text = CarData.car0100Time.ToString() + " Sec.";
            carHorsePower_im.text = CarData.carHorsePower.ToString() + " Bhp";

            if (PlayerPrefs.GetInt(CarData.carIndex) == 1)
            {
                buyButton_im.SetActive(false);
                purchasedButton_im.SetActive(true);
                vehiclePrice_im.text = "- $";
            }
            else if (PlayerPrefs.GetInt(CarData.carIndex) == 0)
            {
                buyButton_im.SetActive(true);
                purchasedButton_im.SetActive(false);
            }
        }

        public void ManageVehicleBuyPagePage()
        {
            ManagePaintSystem();
            //
            DealerVehicleInspectMenu.SetActive(true);
            vehicleName_bs.text = "• " + _tempdata.carName;
            vehiclePrice_bs.text = "<style=\"Orange\">$</style>" + _tempdata.carPrice.ToString();
            vehicleClass_bs.sprite = _tempdata.vehicleClass;
            vehicleBrand_bs.sprite = _tempdata.vehicleBrand;
            carMaxSpeed_bs.text = _tempdata.carMaxSpeed.ToString() + " Kmh";
            car0100Time_bs.text = _tempdata.car0100Time.ToString() + " Sec.";
            carHorsePower_bs.text = _tempdata.carHorsePower.ToString() + " Bhp";
            confirmPriceText_bs.text = "CONFIRM <style=\"Orange\">$</style><style=\"Bold\">" + _tempdata.carPrice.ToString() + "</b> PURCHASE?";
            congratsText_bs.text = "Congratulations! You have purchased <style=\"Orange\">" + _tempdata.carName + "</style>!";

            if (PlayerPrefs.GetInt(_tempdata.carIndex) == 0)
            {
                buyButton_bs.SetActive(true);
            }
        }

        public void ManagePaintSystem()
        {
            for (int i = 0; i < Mathf.Min(_vehicleColorSheetData.colorSheetData.Length, ColorPageParent.transform.childCount); i++)
            {
                ColorPageParent.transform.GetChild(i).GetComponent<Image>().color = _vehicleColorSheetData.colorSheetData[i].UIButtonColor;
                ColorPageParent.transform.GetChild(0).GetComponent<Button>().Select();
            }
        }

        public void ManageSelectedColorIndex(int index)
        {
            selectedColor = index;
            print(_vehicleColorSheetData.colorSheetData[selectedColor].colorName);
        }

        public void HandleGameManagerInputMute(bool boolean)
        {
            GameManagerT11.Instance.inputMute = boolean;
        }

        public void BuyVehicle()
        {
            print("congrats bro, you've made it!");
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - _tempdata.carPrice);
            PlayerPrefs.SetInt(_tempdata.carIndex, 1);
            PlayerPrefs.SetInt("CurrentCar", _tempdata.carNumberIndex);
            // DO YOUR MAGICAL POLISH SPELLS HERE TO FUCK ANYBODY'S MIND
            GameManagerT11.Instance.VehicleBought(_tempdata.carNumberIndex, selectedColor, true);
        }
    }
}
