using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DestinyEngine;
using DestinyEngine.Object;
using Newtonsoft.Json;

namespace TestingTesting
{
    public class TowingEquipment : DestinyScript, ICommand, IColliderReceiver
    {

        public List<ActionCommand_Parent> actionCommands = new List<ActionCommand_Parent>();
        public MonoBehaviour rotateScript;
        public LineRenderer lineRenderer;
        public Transform pointTowingStart;
        public Transform pointTowingEnd;
        public Slider UI_Slider;
        public Text UI_TextValue;
        public GameObject UI_Canvas;

        public float targetThrust = 5000;

        private float currentThrust = 7000;

        private DestinyEngine.VehicleScript targetVehicle;
        private WorldObjectScript worldObjectScript;
        private ScriptInstaller installer;

        //Data internal
        private bool isRopeUnrolled = false;
        private bool isRopeAttached = false;
        private bool isTowing = false;

        private ActionCommand ac_ropeUnroll = new ActionCommand();
        private ActionCommand ac_ropeCancel = new ActionCommand();
        private ActionCommand ac_ropeAttach = new ActionCommand();
        private ActionCommand ac_ropeDeattach = new ActionCommand();
        private ActionCommand ac_activeTow = new ActionCommand();
        private ActionCommand ac_deactiveTow = new ActionCommand();
        private ActionCommand ac_retrieve = new ActionCommand();
        private ActionCommand ac_settings = new ActionCommand();

        private void Awake()
        {
            lineRenderer.useWorldSpace = true;
            worldObjectScript = GetComponent<WorldObjectScript>();
            //assemblyInstallScript = GetComponent<AssemblyInstallScript>();
            Initialize_ActionCommand();

        }

        private void Start()
        {
            DestinyEventHandler.onActionCommandOpen += UpdateTowingEquipment;
            DestinyMainEngine.OnTick += TickEquipment;
            LoadState();
            worldObjectScript.OnSaveState += SaveState;

        }

        private void Initialize_ActionCommand()
        {

            ActionCommand_Parent parentCommand = new ActionCommand_Parent();
            parentCommand.commandID = "TowingEquipment1";
            parentCommand.commandName = "Towing Equipment";
            parentCommand.commandRange = 999;

            ac_ropeUnroll.commandID = "Rope_Unroll";
            ac_ropeCancel.commandID = "Rope_Cancel";
            ac_ropeAttach.commandID = "Rope_Attach";
            ac_ropeDeattach.commandID = "Rope_Deattach";
            ac_activeTow.commandID = "Tow_On";
            ac_deactiveTow.commandID = "Tow_Off";
            ac_retrieve.commandID = "Retrieve";
            ac_settings.commandID = "Setting";

            ac_ropeUnroll.commandName = "Unroll the rope";
            ac_ropeCancel.commandName = "Cancel rope";
            ac_ropeAttach.commandName = "Attach rope";
            ac_ropeDeattach.commandName = "Deattach rope";
            ac_activeTow.commandName = "Activate tow";
            ac_deactiveTow.commandName = "Deactivate tow";
            ac_retrieve.commandName = "Retrieve equipment";
            ac_settings.commandName = "[SETTINGS]";

            ac_ropeUnroll.commandRange = 6;
            ac_ropeCancel.commandRange = 999;
            ac_ropeAttach.commandRange = 999;
            ac_ropeDeattach.commandRange = 6;
            ac_activeTow.commandRange = 6;
            ac_deactiveTow.commandRange = 6;
            ac_retrieve.commandRange = 6;
            ac_settings.commandRange = 6;


            parentCommand.ActionCommand_childs.Add(ac_retrieve);
            parentCommand.ActionCommand_childs.Add(ac_ropeUnroll);
            parentCommand.ActionCommand_childs.Add(ac_ropeCancel);
            parentCommand.ActionCommand_childs.Add(ac_ropeAttach);
            parentCommand.ActionCommand_childs.Add(ac_ropeDeattach);
            parentCommand.ActionCommand_childs.Add(ac_activeTow);
            parentCommand.ActionCommand_childs.Add(ac_deactiveTow);
            parentCommand.ActionCommand_childs.Add(ac_settings);

            actionCommands.Add(parentCommand);
        }

        private void TickEquipment(object sender, OnTickEventArgs e)
        {
            UpdateTowingEquipment();
        }

        private void Update()
        {
            lineRenderer.SetPosition(0, pointTowingStart.position);
            lineRenderer.SetPosition(1, pointTowingEnd.position);

            if (isRopeUnrolled && !isRopeAttached)
            {
                Vector3 targetLine = DestinyMainEngine.main.cinemachineBrain.transform.position;
                targetLine += DestinyMainEngine.main.cinemachineBrain.transform.forward * 1;

                RefreshFindCar();

                if (targetVehicle != null)
                {
                    targetLine = targetVehicle.transform.position;
                }

                pointTowingEnd.transform.position = targetLine;
            }

            if (isRopeAttached && isRopeAttached)
            {
                if (targetVehicle != null)
                {
                    Vector3 targetLine = targetVehicle.transform.position;

                    pointTowingEnd.transform.position = targetLine;
                }
            }

            if (isRopeAttached && isRopeUnrolled && isTowing)
            {
                if (targetVehicle != null)
                {
                    Vector3 targetLine = targetVehicle.transform.position;
                    Vector3 dir = (this.transform.position - targetVehicle.transform.position).normalized;
                    Debug.DrawLine(transform.position, transform.position + dir * 10, Color.red, 0.1f);

                    targetVehicle.vehicleRigidbody.AddForce(dir * targetThrust * 100f * Time.deltaTime);

                    try
                    {

                    }
                    catch
                    {

                    }

                    pointTowingEnd.transform.position = targetLine;

                }
            }
        }

        private void UpdateTowingEquipment()
        {

            if (isRopeUnrolled)
            {
                lineRenderer.enabled = true;
                ac_ropeUnroll.disabled = true;
                ac_ropeCancel.disabled = false;

                if (isRopeAttached)
                {

                    isRopeAttached = true;

                    ac_ropeCancel.disabled = true;
                    ac_ropeAttach.disabled = true;
                    ac_ropeDeattach.disabled = false;
                    ac_activeTow.disabled = false;
                    ac_deactiveTow.disabled = false;

                }
                else
                {
                    isRopeAttached = false;

                    ac_ropeCancel.disabled = false;

                    if (targetVehicle != null)
                    {
                        ac_ropeAttach.disabled = false;
                    }
                    else
                    {
                        ac_ropeAttach.disabled = true;
                    }

                    ac_ropeDeattach.disabled = false;
                    ac_activeTow.disabled = true;
                    ac_deactiveTow.disabled = true;
                }
            }
            else
            {
                lineRenderer.enabled = false;
                ac_ropeUnroll.disabled = false;
                ac_ropeCancel.disabled = true;
                ac_ropeAttach.disabled = true;
                ac_ropeDeattach.disabled = true;
                ac_activeTow.disabled = true;
                ac_deactiveTow.disabled = true;
                isRopeAttached = false;
                isTowing = false;
            }

            if (isTowing)
            {
                rotateScript.enabled = true;
            }
            else
            {
                rotateScript.enabled = false;
            }
        }

        public void CommandExecute(ActionCommand command)
        {
            if (command.commandID == "Retrieve")
            {
                Retrieve();
            }
            else if (command.commandID == "Rope_Unroll")
            {
                isRopeUnrolled = true;
            }
            else if (command.commandID == "Rope_Cancel")
            {
                isRopeUnrolled = false;
                targetVehicle = null;

            }
            else if (command.commandID == "Rope_Attach")
            {
                isRopeAttached = true;
            }
            else if (command.commandID == "Rope_Deattach")
            {
                isRopeAttached = false;
                targetVehicle = null;

            }
            else if (command.commandID == "Tow_On")
            {
                isTowing = true;
            }
            else if (command.commandID == "Tow_Off")
            {
                isTowing = false;
            }
            else if (command.commandID == "Setting")
            {
                DestinyInternalCommand.instance.Quit_ActionCommand();
                DestinyInternalCommand.instance.Game_LockGame(true, true);
                UI_Canvas.gameObject.SetActive(true);
            }

            UpdateTowingEquipment();
        }

        private void Retrieve()
        {
            DestroyObject();

            DestinyInternalCommand.instance.Inventory_AddItem("vanilla", "Construct_TowEquipment", "Weapon", 1);
            DestinyMainEngine.main.SpawnablesManager_.Remove_Spawnable(worldObjectScript);
            DestinyInternalCommand.instance.Quit_ActionCommand();

        }

        private void OnDisable()
        {
            DestroyObject();
        }

        private void DestroyObject()
        {
            DestinyEventHandler.onActionCommandOpen -= UpdateTowingEquipment;
            DestinyMainEngine.OnTick -= TickEquipment;
            worldObjectScript.OnLoadState -= LoadState;
            worldObjectScript.OnSaveState -= SaveState;
        }

        public List<ActionCommand_Parent> CommandListRetrieveAll()
        {
            return actionCommands;
        }

        public void OnChildCollisionDetection(Collider collider)
        {

        }

        private void RefreshFindCar()
        {
            targetVehicle = null;

            DestinyEngine.VehicleScript vehicle = DestinyMainEngine.main.ActiveVehicle;

            if (vehicle != null)
            {
                targetVehicle = vehicle;
            }
        }

        private void RefreshSlider()
        {
            targetThrust = UI_Slider.value;
            UI_TextValue.text = Mathf.Round(targetThrust).ToString();
        }

        public override void SaveState()
        {

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All

            };

            string DATA_isRopeUnrolled = JsonConvert.SerializeObject(isRopeUnrolled, Formatting.Indented, jsonSerializerSettings);
            string DATA_isRopeAttached = JsonConvert.SerializeObject(isRopeAttached, Formatting.Indented, jsonSerializerSettings);
            string DATA_isTowing = JsonConvert.SerializeObject(isTowing, Formatting.Indented, jsonSerializerSettings);
            string DATA_currentThrust = JsonConvert.SerializeObject(targetThrust, Formatting.Indented, jsonSerializerSettings);

            string DATA_vehicleRefID = string.Empty;

            if (targetVehicle != null)
            {
                DATA_vehicleRefID = targetVehicle.RefID;
            }

            worldObjectScript.Save_Variable("isRopeUnrolled", DATA_isRopeUnrolled);
            worldObjectScript.Save_Variable("isRopeAttached", DATA_isRopeAttached);
            worldObjectScript.Save_Variable("isTowing", DATA_isTowing);
            worldObjectScript.Save_Variable("vehicleRefID", DATA_vehicleRefID);
            worldObjectScript.Save_Variable("currentThrust", DATA_currentThrust);

        }

        public override void LoadState()
        {
            ObjectReference_Data Data = worldObjectScript.Get_ObjectRefData();
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            string vehicleRefID = Data.Get_Variable("vehicleRefID");

            if (Data.Get_Variable("isRopeUnrolled") == "true")
            {
                isRopeUnrolled = true;
            }
            else
            {
                isRopeUnrolled = false;
            }

            if (Data.Get_Variable("isRopeAttached") == "true")
            {
                isRopeAttached = true;
            }
            else
            {
                isRopeAttached = false;
            }

            if (Data.Get_Variable("isTowing") == "true")
            {
                isTowing = true;
            }
            else
            {
                isTowing = false;
            }

            targetVehicle = DestinyMainEngine.main.VehicleManager_.Spawned_Vehicle.Find(x => x.RefID == vehicleRefID);

            try
            {
                targetThrust = JsonConvert.DeserializeObject<float>(Data.Get_Variable("currentThrust"), settings);
            }
            catch
            {
                Debug.LogWarning($"{gameObject.name} - TowingEquipment's currentThrust variable is empty or invalid, resetting to default values.");
            }

            UpdateTowingEquipment();
        }

        public void CommandExecute(string functionName)
        {
            if (functionName == "Refresh")
            {
                RefreshSlider();
            }
        }

        public void OnChildCollisionExit(Collider collider)
        {
            throw new System.NotImplementedException();
        }

        public bool ConditionalCheck(string conditionName)
        {
            throw new System.NotImplementedException();
        }
    }
}