using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestinyEngine
{
    [System.Serializable]
    public class Entity_Data : Entity
    {
        [SerializeField] private string entityID_;
        [SerializeField] private DictionaryObject variables_ = new DictionaryObject();

        public string entityID
        {
            get { return entityID_; }
            set { entityID_ = value; }
        }

        public DictionaryObject variables
        {
            get { return variables_; }
            set { variables_ = value; }
        }
    }

    public interface Entity
    {
        string entityID { get; set; }
        DictionaryObject variables { get; set; }

    }


    [System.Serializable]
    public abstract class Interactables : MonoBehaviour, Entity
    {
        public bool constantSave = false;

        [SerializeField] private string entityID_;
        [SerializeField] private DictionaryObject variables_ = new DictionaryObject();

        public string entityID
        {
            get { return entityID_;  }
            set { entityID_ = value; }
        }

        public DictionaryObject variables
        {
            get { return variables_;  }
            set { variables_ = value; }
        }

        public void Save_Variable(string key, string value)
        {
            variables_[key] = value;
        }

        public string Get_Variable(string key)
        {
            string object_ = null;

            if (!variables_.ContainsKey(key))
            {
                return object_;
            }
            if (variables_[key] != null)
            {
                object_ = variables_[key];
            }

            return object_;
        }

        [ContextMenu("Generate_PersistentID")]
        public void Generate_PersistentID()
        {
            entityID = this.gameObject.name;
        }

        public abstract void SaveState();
        public abstract void LoadState();

        public void OverrideVariables(DictionaryObject variables_)
        {
            variables = variables_;
        }
    }

}
