using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace test11
{
    public class CarDisplay : MonoBehaviour
    {
        [Header("Car Info")]
        [SerializeField] private string carIndex;
        [SerializeField] private int carIndexNumber;
        [SerializeField] private TMP_Text carName;
        [SerializeField] private TMP_Text carDescription;
        [SerializeField] private TMP_Text carPrice;
        [SerializeField] private int carPriceInt;

        [Header("Car Stats")]
        [SerializeField] private Image carSpeed;
        [SerializeField] private Image carAcceleration;
        [SerializeField] private Image carHandling;

        [Header("Car Model")]
        [SerializeField] public Transform carHolder;

        [Header("Menus")]
        [SerializeField] private GameObject BuyButton;
        [SerializeField] private GameObject SelectButton;
        [SerializeField] private GameObject SelectedButton;
        [SerializeField] private GameObject notEnoughtMenu;
        [SerializeField] private GameObject vehicleSelected;
        [SerializeField] private GameObject vehicleBought;
        
        public void DisplayCar(Car _car){
            carIndex = _car.carIndex;
            carIndexNumber = _car.carNumberIndex;
            carName.text = _car.carName;
            carDescription.text = _car.carDescription;
            carPrice.text = _car.carPrice.ToString() + " CR";
            carPriceInt = _car.carPrice;

            carSpeed.fillAmount = _car.carSpeed / 100;
            carHandling.fillAmount = _car.carHandling / 100;
            carAcceleration.fillAmount = _car.carAcceleration / 100;

            if(carHolder.childCount > 0){
                for (int i = 0; i < carHolder.childCount; i++)
                {
                    Destroy(carHolder.GetChild(i).gameObject);
                }
                Instantiate(_car.carVisualPrefab, carHolder.position, carHolder.rotation, carHolder);
            }

            if(PlayerPrefs.GetInt(carIndex) == 1 && PlayerPrefs.GetInt("CurrentCar") == carIndexNumber){
                SelectButton.SetActive(false);
                BuyButton.SetActive(false);
                SelectedButton.SetActive(true);
                carPrice.text = "- CR";
            }else if(PlayerPrefs.GetInt(carIndex) == 1){
                SelectButton.SetActive(true);
                BuyButton.SetActive(false);
                SelectedButton.SetActive(false);
                carPrice.text = "- CR";
            }else if(PlayerPrefs.GetInt(carIndex) == 0){
                SelectButton.SetActive(false);
                BuyButton.SetActive(true);
                SelectedButton.SetActive(false);
                carPrice.text = _car.carPrice.ToString() + " CR";
            }
        }

        public void BuyVehicle(){
            if(PlayerPrefs.GetInt("Coins") >= carPriceInt){
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - carPriceInt);
                PlayerPrefs.SetInt(carIndex, 1); 
                SelectButton.SetActive(true);
                BuyButton.SetActive(false);
                vehicleBought.SetActive(true);
                carPrice.text = "- CR";
            }else{
                notEnoughtMenu.SetActive(true);
            }
        }

        public void SelectVehicle(){
            PlayerPrefs.SetInt("CurrentCar", carIndexNumber);
            SelectButton.SetActive(false);
            SelectedButton.SetActive(true);
            //continue, select vehicle code is not written, select button after purchase not changing immedeatly.
        }
    }
}
