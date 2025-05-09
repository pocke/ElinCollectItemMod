using System.Collections.Generic;
using System.Configuration;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace CollectItem;

internal static class ModInfo
{
    internal const string Guid = "me.pocke.collect-item";
    internal const string Name = "Collect Item Mod";
    internal const string Version = "1.0.1";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class CollectItem : BaseUnityPlugin
{
    private void Awake()
    {
        Logger.LogInfo("CollectItem Mod is loaded!");
        Settings.keyCode = Config.Bind("Settings", "KeyCode", KeyCode.C, new ConfigDescription("Key to collect items", null, null));
        Settings.keyCodeMod = Config.Bind("Settings", "KeyCodeMod", KeyCode.LeftAlt, new ConfigDescription("Modifier key to collect item. If None is specified, it does not require a modifier key.", null, null));
    }

    private void Update()
    {
        if (!EClass.core.IsGameStarted)
        {
            return;
        }

        if (!EClass._zone.IsPCFaction)
        {
            return;
        }

        if (!EClass._zone.CanEnterBuildModeAnywhere)
        {
            return;
        }
        

        if ((Settings.KeyCodeMod == KeyCode.None || Input.GetKey(Settings.KeyCodeMod)) && Input.GetKeyDown(Settings.KeyCode))
        {
            Logger.LogInfo("CollectItem: Key pressed. Starting collection.");

            Point.map.ForeachPoint(point =>
            {
                DoCollectItem(point);
            });
        }
    }

    // This method is based on Perform method in Elin/AM_Deconstruct.cs.
    private void DoCollectItem(Point point)
    {
        List<Card> list = point.ListCards();
		list.Reverse();
		foreach (Card item in list.Copy())
		{
			if ((!EClass.debug.ignoreBuildRule && (!item.isThing || !item.trait.CanPutAway)) || item.IsInstalled || item.IsPCParty)
			{
				continue;
			}

            item.PlaySound(item.material.GetSoundDead(item.sourceCard));
            if (item.isThing)
            {
                EClass._map.PutAway(item.Thing);
            }
            else
            {
                item.Destroy();
            }
		}
    }
}
