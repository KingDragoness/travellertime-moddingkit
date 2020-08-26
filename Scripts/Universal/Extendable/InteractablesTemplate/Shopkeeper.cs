using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using DestinyEngine;
using DestinyEngine.Object;

namespace DestinyEngine.Interact
{
    public class Shopkeeper : ChestContainer
    {
        public int shopkeeperMoney = 2000;
        [Range(1f, 10f)]
        public float buyMultiplierPrice = 1.2f;
        [Range(0.2f, 0.99f)]
        public float sellMultiplierPrice = 0.8f;

        public override void CommandExecute(ActionCommand command)
        {
            if (command.commandID == "command.Open")
            {
                DestinyInternalCommand.instance.Interact_OpenShopkeeper(this, buyMultiplierPrice, sellMultiplierPrice);
            }
        }

        public override void LoadState()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            try
            {
                itemContainer = JsonConvert.DeserializeObject<ItemContainer>(Get_Variable("m_all_InventoryItem"), settings);
                shopkeeperMoney = JsonConvert.DeserializeObject<int>(Get_Variable("m_merchantCash"), settings);
            }
            catch
            {
                Debug.LogWarning("Missing variable! " + gameObject.name + " will have its variable reset to default state!");
            }

        }

        public override void SaveState()
        {
            string dataToSave_ = JsonConvert.SerializeObject(itemContainer, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            string shopkeeperMoney_ = JsonConvert.SerializeObject(shopkeeperMoney, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            Save_Variable("m_all_InventoryItem", dataToSave_);
            Save_Variable("m_merchantCash", shopkeeperMoney_);
            Save_Variable("INTERACTABLE_SPECIAL_FLAG", "ChestContainer");

        }
    }
}