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

        public void CheckForInteraction(int playerState)
        {
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
                    }
                }
                else
                {
                    interactionAvailable = false;
                }
            }
            handleUI(anyInteractable);
        }

        private void handleUI(bool interactable)
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
    }
}
