﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;

namespace DestinyEngine.Interact
{
    public class DialogueTriggerTest : InteractableScript
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
            if (DestinyInternalCommand.instance.Quest_GetFlag("vanilla", "Misc_PreciousRing", "Real Ring") != false)
            {
                currentConversation = allConversations[1];
            }
            if (Get_Variable("") == "")
            {

            }
        }

        private void Talk()
        {
            OpenConversationDialogue_Token token = new OpenConversationDialogue_Token();
            token.dialogueDatabase = DialogueDatabase;
            token.currentConversation = currentConversation;

            DestinyInternalCommand.instance.Camera_NPC_On(true);
            DestinyInternalCommand.instance.Interact_OpenDialogue(token);
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

        public override void CommandExecute(string functionName)
        {
            throw new System.NotImplementedException();
        }
    }
}