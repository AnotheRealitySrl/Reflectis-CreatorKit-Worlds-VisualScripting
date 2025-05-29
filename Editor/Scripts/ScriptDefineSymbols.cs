using Reflectis.SDK.Core.Editor;
using UnityEditor;

namespace Reflectis.CreatorKit.Worlds.VisualScripting.Editor
{
    [InitializeOnLoad]
    public class ScriptDefineSymbols
    {
        public const string VISUAL_SCRIPTING_SCRIPT_DEFINE_SYMBOL = "REFLECTIS_CREATOR_KIT_WORLDS_VISUAL_SCRIPTING";
        static ScriptDefineSymbols()
        {
            ScriptDefineSymbolsUtilities.AddScriptingDefineSymbolToAllBuildTargetGroups(VISUAL_SCRIPTING_SCRIPT_DEFINE_SYMBOL);
        }
    }
}