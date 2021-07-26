using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crest;
using DestinyEngine;

namespace TravellerTime
{
    public class Boat : VehicleScript
    {
        public BoatAlignNormal boatControl;

        public override void TurnOff_Vehicle()
        {
            try
            {
                var ac_TurnOff = GetActionCommand_TurnOffVehicle();
                ac_TurnOff.unityEvent?.Invoke();
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }

            boatControl._playerControlled = false;
        }
        public override void TurnOn_Vehicle()
        {
            boatControl._playerControlled = true;
        }

        public override void LoadState()
        {

        }

        public override void SaveState()
        {

        }

        public override void CommandExecute(string functionName)
        {
            throw new System.NotImplementedException();
        }

        public override void SaveScriptAsJSON()
        {
            //Implement something
        }
    }
}
