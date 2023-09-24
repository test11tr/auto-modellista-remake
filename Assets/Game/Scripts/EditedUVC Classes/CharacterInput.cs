using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class CharacterInput :MonoBehaviour
    {
        [Header ("Device input settings")]
        public string HorizontalMoveAxis = "Horizontal";
        public string VerticalMoveAxis = "Vertical";
        public string HorizontalViewAxis = "Mouse X";
        public string VerticalViewAxis = "Mouse Y";
        public bool isRunning;

        public Vector2 MoveInput { get; private set; }
        public Vector2 ViewInput { get; private set; }

        private void Update ()
        {       
            MoveInput = new Vector2 (Input.GetAxis (HorizontalMoveAxis), Input.GetAxis (VerticalMoveAxis));
            ViewInput = new Vector2 (Input.GetAxis (HorizontalViewAxis), Input.GetAxis (VerticalViewAxis));
            isRunning  = Input.GetKey (KeyCode.LeftShift);
        }
    }
}
