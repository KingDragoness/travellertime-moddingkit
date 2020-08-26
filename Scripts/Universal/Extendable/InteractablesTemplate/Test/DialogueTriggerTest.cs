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

        [Tooltip("Only trigger if no other conversation is already active.")]
        public bool exclusive = false;

        [Tooltip("Only trigger if at least one entry's Conditions are currently true.")]
        public bool skipIfNoValidEntries = true;

        [Tooltip("Start at this entry ID.")]
        [HideInEditorMode] public int startConversationEntryID = -1;

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
            if (DestinyInternalCommand.instance.Quest_GetStageIndex("vanilla", "Misc_PreciousRing") == 10)
            {
                currentConversation = allConversations[1];
            }
        }

        private void Talk()
        {

            DestinyInternalCommand.instance.Quit_ActionCommand();
            DestinyInternalCommand.instance.Game_LockGame(true, false);

            DialogueManager.databaseManager.Clear();
            DialogueManager.databaseManager.Add(DialogueDatabase);

            if (string.IsNullOrEmpty(currentConversation)) return;
            if (exclusive && DialogueManager.isConversationActive)
            {
                if (DialogueDebug.logInfo) Debug.Log("Dialogue System: Conversation triggered on " + name + " but skipping because another conversation is active.", this);
            }
            else
            {
                var actorTransform = Tools.Select(DestinyMainEngine.main.ExamplePlayer.transform, transform);
                var conversantTransform = this.transform;
                if (conversantTransform == null)
                {
                    var conversationAsset = DialogueManager.MasterDatabase.GetConversation(currentConversation);
                    var conversationConversantActor = DialogueManager.MasterDatabase.GetActor(conversationAsset.ConversantID);
                    var registeredTransform = (conversationConversantActor != null) ? PixelCrushers.DialogueSystem.CharacterInfo.GetRegisteredActorTransform(conversationConversantActor.Name) : null;
                    conversantTransform = (registeredTransform != null) ? registeredTransform : this.transform;
                }
                if (skipIfNoValidEntries && !DialogueManager.ConversationHasValidEntry(currentConversation, actorTransform, conversantTransform))
                {
                    if (DialogueDebug.logInfo) Debug.Log("Dialogue System: Conversation triggered on " + name + " but skipping because no entries are currently valid.", this);
                }
                else
                {
                    DialogueManager.StartConversation(currentConversation, actorTransform, conversantTransform, startConversationEntryID);
                }
            }
        }

        public override void LoadState()
        {

        }

        public override void SaveState()
        {

        }

    }
}