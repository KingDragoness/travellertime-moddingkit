using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestinyEngine.Object;
using UMod;

namespace DestinyEngine
{
    /// <summary>
    /// 
    /// => Insert the island name or if it is Overworld then: insert "Destiny.Overworld" or empty string.
    /// 
    /// </summary>
    /// 

    public interface IPositionContainer
    {
        Vector3 Rotation { get; set; }
        Vector3 SubCoord { get; set; }     //Use this in LOCAL
        Vector3 GCECoord { get; set; }     //Use this in GLOBAL
        string CurrentIsland { get; set; }
    }


    [System.Serializable]
    public class IslandData
    {
        public string NameDisplay = "";
        public string RegionName = "";
        public string ModPackName = "vanilla";
        public Vector3 GCECoord = new Vector3(0, 0, 0);

        [Tooltip("IslandSize Y axis will not affect the scale.")]
        public Vector3 IslandSize = new Vector3(1, 1, 1);

    }

    [System.Serializable]
    public class Overworld_MapData
    {
        public List<IslandData> GlobalIslands = new List<IslandData>();

        public Overworld_MapData(List<IslandData> globalIslands)
        {
            GlobalIslands = globalIslands;
        }
    }


    [System.Serializable]
    public class LocalRegion_SaveData
    {
        public string RegionName = "";
        public List<Entity_Data>        all_Interactables_Entities = new List<Entity_Data>();
        public List<Spawnables_Data>    all_SpawnedDatas = new List<Spawnables_Data>();
    }

    [System.Serializable]
    public class GlobalRegion_SaveData
    {
        public string RegionName = "";
    }

    [System.Serializable]
    public class Vehicle_SaveData : IPositionContainer
    {
        [SerializeField] public DictionaryObject variables_ = new DictionaryObject();
        public Vector3  Rotation_;
        public Vector3  SubCoord_;
        public Vector3  GCECoord_;
        public string   CurrentIsland_;

        public Vector3 Rotation         { get { return Rotation_; } set { Rotation_ = value; } }
        public Vector3 SubCoord         { get { return SubCoord_; } set { SubCoord_ = value; } }
        public Vector3 GCECoord         { get { return GCECoord_; } set { GCECoord_ = value; } }
        public string CurrentIsland     { get { return CurrentIsland_; } set { CurrentIsland_ = value; } }

        #region FormID
        public string VehicleBaseID = "$VEHICLE";   //Prefab object

        public string refID = "";

        public string RefID          { get { return refID; } set { refID = value; } }

        //private string stupidIslandID = "";
        //public string StupidIslandID { get => stupidIslandID; set => stupidIslandID = value; }

        #endregion
        public DictionaryObject variables
        {
            get { return variables_; }
            set { variables_ = value; }
        }

    }

    public enum Game_PlayerMode
    {
        FirstPerson,
        VehicleMode,
    }

    public enum Game_Gamemode
    {
        Overworld,
        Island
    }

    public class AssetBundleHost
    {
        public string name = "modName";

        public AssetBundle assetBundle;
    }

    [System.Serializable]
    public class ModHosting
    {
        public string name = "modName";

        public ModHost modHost;
    }


    [System.Serializable]
    public class EngineData
    {
        public Game_Gamemode Gamemode = Game_Gamemode.Island;
        public Game_PlayerMode Playermode = Game_PlayerMode.FirstPerson;
        public string CurrentIsland = ""; //Insert the island name or if it is Overworld then: insert "Destiny.Overworld" or empty string.
    }
}