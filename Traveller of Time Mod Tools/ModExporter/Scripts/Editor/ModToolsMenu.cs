//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Destiny_Modding_Suite.Generated
{
    using UMod.Shared;
    
    
    public class ModToolsMenu
    {
        
        [UnityEditor.MenuItem("Scripting/Export script mod...", priority=0)]
        internal static void Menu_Export_script_mod___()
        {
            UMod.BuildEngine.ModToolsUtil.ShowToolsWindow(typeof(UMod.Exporter.ExporterWindow));
        }
        
        [UnityEditor.MenuItem("Scripting/New script mod...", priority=1)]
        internal static void Menu_New_script_mod___()
        {
            UMod.BuildEngine.ModToolsUtil.ShowToolsWindow(typeof(UMod.Exporter.CreateModWindow));
        }
        
        [UnityEditor.MenuItem("Scripting/Export Settings", priority=22)]
        internal static void Menu_Export_Settings()
        {
            UMod.BuildEngine.ModToolsUtil.ShowToolsWindow(typeof(UMod.Exporter.SettingsWindow));
        }
        
        [UnityEditor.MenuItem("Scripting/Build script mod %#b", priority=43)]
        internal static void Menu_Build_script_mod___b()
        {
            UMod.ModTools.Export.ExportSettings settings = UMod.ModTools.Export.ExportSettings.Active.Load();
            if ((settings == null))
            {
                throw new UMod.ModLoadException("The export settings are missing from this mod tools package");
            }
            UMod.BuildEngine.ModToolsUtil.StartBuild(settings);
        }
        
        [UnityEditor.MenuItem("Scripting/Help", priority=64)]
        internal static void Menu_Help()
        {
            UMod.BuildEngine.ModToolsUtil.ShowToolsWindow(typeof(UMod.Exporter.HelpWindow));
        }
        
        [UnityEditor.MenuItem("Scripting/About", priority=65)]
        internal static void Menu_About()
        {
            UMod.BuildEngine.ModToolsUtil.ShowToolsWindow(typeof(UMod.Exporter.AboutWindow));
        }
    }
}
