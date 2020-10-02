using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;
using DestinyEngine.Object;

namespace TestingTesting
{
    public class NPC_GenericRobotTest : DestinyScript, ICommand
    {
        [Header("Ignore above")]
        public ActorScript actorScript;

        public void CommandExecute(ActionCommand command)
        {
            if (command.commandID == "talk")
            {
                SpecialCheck();
            }
        }

        private void SpecialCheck()
        {
            if (DestinyInternalCommand.instance.Quest_GetCompleteIndex("vanilla", "Misc_PreciousRing", 10) != false)
            {
                actorScript.currentConversation = actorScript.allConversations[1];
            }
            if (Get_Variable("") == "")
            {

            }
        }

        public List<ActionCommand> CommandListRetrieveAll()
        {
            throw new System.NotImplementedException();
        }

        public override void LoadState()
        {
            
        }

        public override void SaveState()
        {
            actorScript.Save_Variable("PositionTEST", transform.position.ToString());
        }

        public override void ExecuteFunction(string functionName)
        {
            throw new System.NotImplementedException();
        }
    }
}
