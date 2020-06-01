using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using Bolt;

namespace DestinyEngine
{

    public class Bolt_Interactable : Interactables
    {
        [Header("Bolt")]
        public Variables        variablesStorage;
        public List<string>     VariableNames_ToSave = new List<string>();

        public override void CommandExecute(ActionCommand command)
        {
            CustomEvent.Trigger(this.gameObject, command.commandID);
        }

        public override void LoadState()
        {
            var variableDeclare = Variables.Object(gameObject).ToListPooled<VariableDeclaration>();

            for (int x = 0; x < variableDeclare.Count; x++)
            {
                if (VariableNames_ToSave.Find(l => l.ToString() == variableDeclare[x].name) != null)
                {
                    var value = Get_Variable(variableDeclare[x].name);
                    var s = Variables.Object(gameObject).Get(variableDeclare[x].name);

                    if (s.GetType() == typeof(string))
                    {
                        Variables.Object(gameObject).Set(variableDeclare[x].name, JsonUtility.FromJson<string>(value));
                    }
                }
            }
        }

        [ContextMenu("SaveState")]
        public override void SaveState()
        {
            var variableDeclare = Variables.Object(gameObject).ToListPooled<VariableDeclaration>();

            for(int x = 0; x < variableDeclare.Count; x++)
            {
                if (VariableNames_ToSave.Find(l => l.ToString() == variableDeclare[x].name) != null)
                {
                    print(variableDeclare[x].name);
                    Save_Variable(variableDeclare[x].name, JsonUtility.ToJson(variableDeclare[x].value));
                }
            }
        }
    }
}
