using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using DestinyEngine;
using DestinyEngine.Object;

namespace DestinyEngine.Interact
{
    public class ChestContainer : Interactables
    {
        public ItemContainer itemContainer;

        public override void CommandExecute(ActionCommand command)
        {
            if (command.commandID == "command.Open")
            {
                DestinyInternalCommand.instance.Interact_OpenItemContainer(itemContainer);
            }
        }

        public override void ExecuteFunction(string functionName)
        {
            throw new System.NotImplementedException();
        }

        public override void LoadState()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            try
            {
                itemContainer = JsonConvert.DeserializeObject<ItemContainer>(Get_Variable("m_all_InventoryItem"), settings);
            }
            catch
            {

            }
        }

        public override void SaveState()
        {
            string dataToSave = JsonConvert.SerializeObject(itemContainer, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            Save_Variable("m_all_InventoryItem", dataToSave); //Must have "m_all_InventoryItem" in it's name if you want the chest appear correctly in map marker
            Save_Variable("INTERACTABLE_SPECIAL_FLAG", "ChestContainer"); //Must use this line if you want the chest appear correctly in map marker

        }
    }
}