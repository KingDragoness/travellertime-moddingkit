using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crest;
using DestinyEngine;

namespace TravellerTime
{
    public class Boat : Vehicle
    {
        public BoatAlignNormal boatControl;
        public override void TurnOff_Vehicle()
        {
            boatControl._playerControlled = false;
        }
        public override void TurnOn_Vehicle()
        {
            boatControl._playerControlled = true;
        }
    }
}
