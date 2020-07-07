using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using DestinyEngine;
using DestinyEngine.Interact;

namespace TravellerTime.Default
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

        public override void LoadState()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            itemContainer = JsonConvert.DeserializeObject<ItemContainer>(Get_Variable("m_all_InventoryItem"), settings);
        }

        public override void SaveState()
        {
            string dataToSave = JsonConvert.SerializeObject(itemContainer, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            Save_Variable("m_all_InventoryItem", dataToSave);

        }
    }
}