using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;
using EVP;

namespace TravellerTime
{
    public class EVP_Car : Vehicle
    {
        public VehicleController vehicle;

        VehicleStandardInput m_commonInput = null;
        VehicleTelemetry m_commonTelemetry = null;
        public bool overrideVehicleComponents = true;

        public override void LoadState()
        {
            VehicleStandardInput vehicleInput = vehicle.GetComponent<VehicleStandardInput>();
            vehicleInput.enabled = false;
        }

        public override void SaveState()
        {

        }

        public override void TurnOff_Vehicle()
        {
            if (vehicle == null) return;

            SetupVehicleComponents(vehicle, false);
            vehicle.throttleInput = 0.0f;
            vehicle.brakeInput = 1.0f;
        }

        public override void TurnOn_Vehicle()
        {
            if (vehicle == null) return;

            SetupVehicleComponents(vehicle, true);

        }

        void SetupVehicleComponents(VehicleController vehicle, bool enabled)
        {
            VehicleTelemetry vehicleTelemetry = vehicle.GetComponent<VehicleTelemetry>();
            VehicleStandardInput vehicleInput = vehicle.GetComponent<VehicleStandardInput>();
            VehicleDamage vehicleDamage = vehicle.GetComponent<VehicleDamage>();

            if (vehicleInput != null)
            {
                if (m_commonInput != null)
                {
                    if (overrideVehicleComponents)
                    {
                        vehicleInput.enabled = false;
                        m_commonInput.enabled = true;
                        m_commonInput.target = enabled ? vehicle : null;
                    }
                    else
                    {
                        vehicleInput.enabled = enabled;
                        m_commonInput.enabled = false;
                    }
                }
                else
                {
                    vehicleInput.enabled = enabled;
                }
            }
            else
            {
                if (m_commonInput != null)
                {
                    m_commonInput.enabled = true;
                    m_commonInput.target = enabled ? vehicle : null;
                }
            }

            if (vehicleTelemetry != null)
            {
                if (m_commonTelemetry != null)
                {
                    if (overrideVehicleComponents)
                    {
                        vehicleTelemetry.enabled = false;
                        m_commonTelemetry.enabled = true;
                        m_commonTelemetry.target = enabled ? vehicle : null;
                    }
                    else
                    {
                        vehicleTelemetry.enabled = enabled;
                        m_commonTelemetry.enabled = false;
                    }
                }
                else
                {
                    vehicleTelemetry.enabled = enabled;
                }
            }
            else
            {
                if (m_commonTelemetry != null)
                {
                    m_commonTelemetry.enabled = true;
                    m_commonTelemetry.target = enabled ? vehicle : null;
                }
            }

            if (vehicleDamage != null)
            {
                vehicleDamage.enableRepairKey = enabled;
            }
        }
    }
}
