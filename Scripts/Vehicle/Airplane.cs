using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFlight.Demo;
using DestinyEngine;

namespace TravellerTime
{
    public class Airplane : Vehicle
    {
        public MFlight.Demo.Plane planeControl;
        public override void TurnOff_Vehicle()
        {
            planeControl.enabled = false;
        }
        public override void TurnOn_Vehicle()
        {
            planeControl.enabled = true;
        }
    }
}
