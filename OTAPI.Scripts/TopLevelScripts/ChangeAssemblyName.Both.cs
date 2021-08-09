﻿/*
Copyright (C) 2020 DeathCradle

This file is part of Open Terraria API v3 (OTAPI)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <http://www.gnu.org/licenses/>.
*/
#pragma warning disable CS8321 // Local function is declared but never used

using ModFramework;
using MonoMod;

/// <summary>
/// @doc A mod to change the Terraria or TerrariaServer name to OTAPI so cross platform plugins only need to reference one assembly name
/// </summary>
[Modification(ModType.PostPatch, "Changing assembly name to OTAPI", ModPriority.Last)]
void ChangeAssemblyName(MonoModder modder)
{
    foreach (var asmref in modder.Module.AssemblyReferences)
    {
        if (asmref.Name == "Terraria" || asmref.Name == "TerrariaServer")
        {
            var from = asmref.Name;
            modder.Log($"[OTAPI] RelinkModule: {from} -> {modder.Module.Name}");
            modder.RelinkModuleMap[from] = modder.Module;
            modder.RelinkModuleMap["OTAPI"] = modder.Module;

            asmref.Name = "OTAPI";
        }
    }
    modder.Module.Name = modder.Module.Assembly.Name.Name = "OTAPI";

    (modder.AssemblyResolver as Mono.Cecil.DefaultAssemblyResolver).ResolveFailure += (s, e) =>
    {
        if (e.Name == "OTAPI") return modder.Module.Assembly;
        return null;
    };
}