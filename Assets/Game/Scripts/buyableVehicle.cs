using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using test11;
using PG;

namespace test11
{
    public class buyableVehicle : MonoBehaviour, IInteractable
    {
        [Header("Interaction Text Data")]
        public InteractionText _interactTextData;
        public Car _CarData;
        string interactionText;

        //INTERACT INTERFACE

        public void Interact()
        {
            if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.OpenWorld)
            {
                //
            }
            else if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.VehicleDealer)
            {
                MenuManagerT11.Instance.ManageVehicleInspectorPage(_CarData);
                GameManagerT11.Instance.HandleInputMute(true);
                Interactor.Instance.isInteractCheckable = false;
            }
        }

        public string GetInteractionText()
        {
            if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.Garage)
            {
                return interactionText = _interactTextData.InGarageText;
            }
            else if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.VehicleTuner)
            {
                return interactionText = _interactTextData.InTunerText;
            }
            else if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.VehicleDealer)
            {
                return interactionText = _interactTextData.InDealerText;
            }
            else if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.Race)
            {
                return interactionText = _interactTextData.InRaceAreaText;
            }
            else if (GameManagerT11.Instance.sceneType == GameManagerT11.SceneTypes.OpenWorld)
            {
                return interactionText = _interactTextData.InOpenWorldText;
            }

            return interactionText = _interactTextData.defaultText;
        }
    }
}
