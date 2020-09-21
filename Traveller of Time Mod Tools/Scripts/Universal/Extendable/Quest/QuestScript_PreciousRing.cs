using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;
using PixelCrushers.DialogueSystem;

using QuestState = DestinyEngine.Object.QuestState;


namespace TestingTesting.Quests
{
    public class QuestScript_PreciousRing : QuestObject
    {


        public override void Initialize()
        {
            DestinyInternalCommand.instance.Quest_ShowObjective("vanilla", "Misc_PreciousRing", 1);

        }

        public override void LoadState()
        {

        }

        public override void SaveState()
        {

        }

        public override void CallCommand(string commandName)
        {
            if (commandName == "Complete:RealRing")
            {
                CompleteQuest_GiveRealRing();
            }
            else if (commandName == "Complete:FakeRing")
            {
                CompleteQuest_GiveFakeRing();
            }
        }

        private void CompleteQuest_GiveRealRing()
        {
            print("Quest done");
            DestinyInternalCommand.instance.Quest_SetIndexComplete("vanilla", "Misc_PreciousRing", 10);
            DestinyInternalCommand.instance.Quest_CompleteObjective("vanilla", "Misc_PreciousRing", 0);
            DestinyInternalCommand.instance.Quest_SetCurrentState("vanilla", "Misc_PreciousRing", QuestState.Success);
            DestinyInternalCommand.instance.Inventory_RemoveItem("vanilla", "OneRing", "Item_Key", 1);
        }

        private void CompleteQuest_GiveFakeRing()
        {
            print("Quest done, fake ring!");
            DestinyInternalCommand.instance.Quest_SetIndexComplete("vanilla", "Misc_PreciousRing", 11);
            DestinyInternalCommand.instance.Quest_CompleteObjective("vanilla", "Misc_PreciousRing", 1);
            DestinyInternalCommand.instance.Quest_SetCurrentState("vanilla", "Misc_PreciousRing", QuestState.Success);
            DestinyInternalCommand.instance.Inventory_RemoveItem("vanilla", "Fake OneRing", "Item_Key", 1);

        }
    }
}