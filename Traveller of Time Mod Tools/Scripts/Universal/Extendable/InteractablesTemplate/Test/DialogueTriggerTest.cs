using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;

namespace DestinyEngine.Interact
{
    public class DialogueTriggerTest : Interactables
    {
        public DialogueDatabase DialogueDatabase;

        [ConversationPopup(true)]
        public string[] allConversations;

        [ConversationPopup(true)]
        public string currentConversation = string.Empty;


        public override void CommandExecute(ActionCommand command)
        {
            if (command.commandID == "Talk")
            {
                SpecialCheck();
                Talk();
            }
        }

        private void SpecialCheck()
        {
            if (DestinyInternalCommand.instance.Quest_GetCompleteIndex("vanilla", "Misc_PreciousRing", 10) != false)
            {
                currentConversation = allConversations[1];
            }
            if (Get_Variable("") == "")
            {

            }
        }

        private void Talk()
        {
            DestinyInternalCommand.instance.Camera_NPC_On(true);
            DestinyInternalCommand.instance.Interact_OpenDialogue(DialogueDatabase, currentConversation);
            DestinyInternalCommand.instance.Camera_NPC_ChangeTarget(this.transform, this.transform);
            DestinyInternalCommand.instance.Camera_NPC_ChangeDist(new Vector3(0, 0, 6));
            DestinyInternalCommand.instance.GameUI_HideMainUI();
        }

        public override void LoadState()
        {




            if (Get_Variable("PlayerDelayQuestStart") != null)
            {

            }
        }

        public override void SaveState()
        {

        }

    }
}