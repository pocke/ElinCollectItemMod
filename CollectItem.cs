using BepInEx;
using HarmonyLib;

namespace MyMod;

internal static class ModInfo
{
    internal const string Guid = "me.pocke.collect-item";
    internal const string Name = "Collect Item Mod";
    internal const string Version = "1.0";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class CollectItem : BaseUnityPlugin
{
    private void Awake()
    {
        Logger.LogInfo("CollectItem Mod is loaded!");
    }
}
