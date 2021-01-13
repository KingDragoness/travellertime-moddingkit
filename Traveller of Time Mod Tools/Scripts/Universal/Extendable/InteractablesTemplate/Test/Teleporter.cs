using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;

namespace TestingTesting
{
    public class Teleporter : InteractableScript
    {

        [Header("Objects")]
        public Transform target;

        public override void CommandExecute(ActionCommand command)
        {
            if (command.commandID == "command.teleport")
            {
                TeleportPlayer();
            }

            if (command.commandID == "command.read")
            {
                DestinyInternalCommand.instance.Interact_ReadBookSign("Teleporter", "The teleport system to almost instaneously moved from one space to another. ");
            }
        }

        public override void CommandExecute(string functionName)
        {
            if (functionName == "command.teleport")
            {
                TeleportPlayer();
            }
        }

        public override void LoadState()
        {

        }

        public override void SaveState()
        {

        }

        #region Actions
        void TeleportPlayer()
        {
            DestinyInternalCommand.instance.TeleportPlayer(target);
        }
        #endregion
    }
}