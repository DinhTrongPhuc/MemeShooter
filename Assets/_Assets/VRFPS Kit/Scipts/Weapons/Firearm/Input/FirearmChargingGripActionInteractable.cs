using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VRFPSKit
{
    public class FirearmChargingGripActionInteractable : FirearmCyclingActionInteractable
    {
        public float actionReleaseThreshold = 1;
        
        protected override float ActionMovementFromHand()
        {
            float actionMovement = base.ActionMovementFromHand();

            //If action is in the forward position & If action movement
            //didn't exceed release threshold, don't move
            if(cyclingAction.actionPosition01 <= 0 && 
               actionMovement < actionReleaseThreshold * Time.deltaTime * handMovementDirectionSensitivity.magnitude) 
                return 0;
            
            return actionMovement;
        }

        protected override IXRSelectInteractor GetHand()
        {
            if (actionInteractable.interactorsSelecting.Count < 2) return null;
            
            //Return second hand that is grabbing
            return actionInteractable.interactorsSelecting[1];
        }
    }
}
