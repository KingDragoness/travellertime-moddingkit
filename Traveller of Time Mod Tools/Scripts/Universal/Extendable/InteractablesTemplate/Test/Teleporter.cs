﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;

namespace TestingTesting
{
    public class Teleporter : Interactables
    {

        [Header("Objects")]
        public Transform target;

        public override void CommandExecute(ActionCommand command)
        {
            if (command.commandID == "command.teleport")
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