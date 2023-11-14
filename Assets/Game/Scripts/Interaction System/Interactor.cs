using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PG;

namespace test11
{
    interface IInteractable
    {
        string GetInteractionText();
        void Interact();
    }

    public class Interactor : Singleton<Interactor>
    {
        [SerializeField] private GameObject interactUIContainer;
        public float interactRange;
        public TMP_Text _interactionText;
        [HideInInspector] public bool interactionAvailable;
        [HideInInspector] public bool isInteractCheckable = true;

        public void CheckForInteraction(int playerState)
        {
            if (isInteractCheckable) {
                int layerMask = 1 << playerState;
                Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange, ~layerMask);
                bool anyInteractable = false;

                foreach (Collider collider in colliderArray)
                {
                    if (collider.TryGetComponent(out IInteractable interactObj))
                    {
                        interactionAvailable = true;
                        _interactionText.text = interactObj.GetInteractionText();
                        anyInteractable = true;
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            interactObj.Interact();
                            anyInteractable = false;
                        }
                    }
                }
                handleUI(anyInteractable);
            }
            
        }

        public void handleUI(bool interactable)
        {
            if (interactable)
            {
                interactUIContainer.SetActive(true);
            }
            else
            {
                interactUIContainer.SetActive(false);
            }
        }

        public void HandleInteractCheckable(bool boolean)
        {
            isInteractCheckable = boolean;
        }
    }
}
