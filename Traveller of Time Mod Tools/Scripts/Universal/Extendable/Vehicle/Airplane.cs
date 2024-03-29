﻿using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using MFlight.Demo;
using DestinyEngine;

namespace TravellerTime
{
    public class Airplane : VehicleScript
    {
        public MFlight.Demo.Plane planeControl;

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

            planeControl.enabled = false;
        }
        public override void TurnOn_Vehicle()
        {
            planeControl.enabled = true;
        }

        public override void LoadState()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            try
            {
                planeControl.thrust = JsonConvert.DeserializeObject<float>(Get_Variable("m_thrust"), settings);
            }
            catch
            {
                Debug.LogWarning("Missing variable! " + gameObject.name + " will have its variable reset to default state!");
            }
        }

        public override void SaveState()
        {
            string thrust_ = JsonConvert.SerializeObject(planeControl.thrust, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            Save_Variable("m_thrust", thrust_);
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
