using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine;

namespace TestingTesting
{
    //FOR TEST ONLY!
    public class Door : Interactables
    {
        [Header("Objects")]
        public GameObject cylinder;

        [Header("Storage Variables")]
        public bool m_IsDoorOpened = false;

        void Update_State()
        {
            if (m_IsDoorOpened)
            {
                cylinder.gameObject.SetActive(true);
            }
            else
            {
                cylinder.gameObject.SetActive(false);
            }
        }

        public override void SaveState()
        {
            Save_Variable("m_IsDoorOpened", m_IsDoorOpened.ToString());
        }

        public override void LoadState()
        {
            string IsDoorOpened = Get_Variable("m_IsDoorOpened");

            if (bool.TryParse(IsDoorOpened, out m_IsDoorOpened))
            {
                bool b = bool.Parse(IsDoorOpened);
                m_IsDoorOpened = b;
            }

            Update_State();
        }

        public override void CommandExecute(ActionCommand command)
        {
            if (command.commandID == "command.opendoor")
            {
                OpenDoor();
            }
        }

        #region Actions
        void OpenDoor()
        {
            m_IsDoorOpened = !m_IsDoorOpened;
            Update_State();
        }
        #endregion
    }


}