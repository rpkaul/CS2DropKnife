﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API;

namespace CS2DropKnife;

public class CS2DropKnife : BasePlugin
{
    public override string ModuleName => "CS2 Drop Knife";

    public override string ModuleVersion => "0.0.1";

	public override void Load(bool hotReload)
    {
        base.Load(hotReload);

	    Console.WriteLine("[CS2DropKnife] Registering listeners.");
        RegisterListener<Listeners.OnMapStart>(OnMapStartHandler);
    }

	public void OnMapStartHandler(string map)
    {
		Server.ExecuteCommand("mp_drop_knife_enable 1");
    }

    [ConsoleCommand("css_drop", "Drop 5 copies of player's knife on the ground.")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnPrefireCommand(CCSPlayerController player, CommandInfo commandInfo)
    {
        // Player might not be alive.
        if (player.PlayerPawn?.Value == null || player.PlayerPawn?.Value.WeaponServices == null || player.PlayerPawn?.Value.ItemServices == null)
			return;

		var weapons = player.PlayerPawn.Value.WeaponServices?.MyWeapons;

        // Player might have no weapon.
		if (weapons == null) return;

        // Find the knife.
		foreach (var weapon in weapons)
		{
            if (weapon != null && weapon.IsValid && weapon.Value != null && weapon.Value.IsValid)
			{
				if (weapon.Value.DesignerName.Contains("knife") || weapon.Value.DesignerName.Contains("bayonet"))
				{
					Console.WriteLine("[CS2DropKnife] knife index = " + weapon.Index + ", entityindex = " + weapon.Value.Index + ", designer name = " + weapon.Value.DesignerName);
					for (int i = 0; i < 5; i++)
						player.GiveNamedItem(weapon.Value.DesignerName);
					return;
				}
			}
		}

		player.PrintToChat("[CS2DropKnife] Can't find a knife on you. Get one and try again please.");
    }
}
