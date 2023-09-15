using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using test11;

namespace test11
{
    public class ScriptableObjectsChanger : MonoBehaviour
    {
        [SerializeField] private ScriptableObject[] _scriptableObjects;
        [SerializeField] private CarDisplay _carDisplay;

        private int currentIndex;

        private void Awake() {
            ChangeScriptableObject(0);
        }

        public void ResetScriptableObject(int _change){
            currentIndex = _change;

            if(_carDisplay != null){
                _carDisplay.DisplayCar((Car)_scriptableObjects[currentIndex]);
            }
        }

        public void ChangeScriptableObject(int _change){
            currentIndex += _change;

            if(currentIndex < 0){
                currentIndex = _scriptableObjects.Length - 1;
            }else if(currentIndex > _scriptableObjects.Length -1){
                currentIndex = 0;
            }

            if(_carDisplay != null){
                _carDisplay.DisplayCar((Car)_scriptableObjects[currentIndex]);
            }
        }
    }
}
