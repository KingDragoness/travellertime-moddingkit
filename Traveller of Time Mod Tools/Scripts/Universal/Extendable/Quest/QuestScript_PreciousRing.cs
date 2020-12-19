using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;
using PixelCrushers.DialogueSystem;

using QuestState = DestinyEngine.Object.QuestState;


namespace TestingTesting.Quests
{
    public class QuestScript_PreciousRing : QuestScript
    {


        public override void Initialize()
        {
            base.Initialize();
            DestinyInternalCommand.instance.Quest_ShowObjective("vanilla", "Misc_PreciousRing", "Fake Ring");
            DestinyInternalCommand.instance.Quest_ShowObjective("vanilla", "Misc_PreciousRing", "Real Ring");

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
            DestinyInternalCommand.instance.Quest_SetFlag("vanilla", "Misc_PreciousRing", "Real Ring");
            DestinyInternalCommand.instance.Quest_SetCurrentFlag("vanilla", "Misc_PreciousRing", "Real Ring");

            DestinyInternalCommand.instance.Quest_CompleteObjective("vanilla", "Misc_PreciousRing", "Start");
            DestinyInternalCommand.instance.Quest_SetCurrentState("vanilla", "Misc_PreciousRing", QuestState.Success);
            DestinyInternalCommand.instance.Inventory_RemoveItem("vanilla", "OneRing", "Key", 1);
        }

        private void CompleteQuest_GiveFakeRing()
        {
            print("Quest done, fake ring!");
            DestinyInternalCommand.instance.Quest_SetFlag("vanilla", "Misc_PreciousRing", "Fake Ring");
            DestinyInternalCommand.instance.Quest_SetCurrentFlag("vanilla", "Misc_PreciousRing", "Fake Ring");

            DestinyInternalCommand.instance.Quest_CompleteObjective("vanilla", "Misc_PreciousRing", "Start");
            DestinyInternalCommand.instance.Quest_SetCurrentState("vanilla", "Misc_PreciousRing", QuestState.Success);
            DestinyInternalCommand.instance.Inventory_RemoveItem("vanilla", "Fake OneRing", "Key", 1);

        }

        public override void CommandExecute(string functionName)
        {
            throw new System.NotImplementedException();
        }
    }
}