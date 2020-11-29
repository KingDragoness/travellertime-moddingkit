using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;

public class Debug_Interactable : MonoBehaviour
{
    public InteractableScript interactables;
    public string commandID = "";

    [ContextMenu("CommandExecute")]
    public void CommandExecute()
    {
        ActionCommand command = new ActionCommand();
        command.commandID = commandID;

        interactables.CommandExecute(command);
    }
}
