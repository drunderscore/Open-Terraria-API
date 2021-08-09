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
#pragma warning disable CS0436 // Type conflicts with imported type

using ModFramework;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod;
using MonoMod.Cil;
using System;
using System.Linq;

/// <summary>
/// @doc Creates Hooks.Chest.QuickStack.
/// </summary>
[MonoModIgnore]
partial class ChestHooks
{
    static MethodDefinition PutItemInNearbyChest { get; set; }
    static ParameterDefinition PlayerID { get; set; }

    [Modification(ModType.PreMerge, "Hooking chest stacking")]
    static void HookChestQuickStack(ModFwModder modder)
    {
        var csr = modder.GetILCursor(() => Terraria.Chest.PutItemInNearbyChest(null, default));
        PutItemInNearbyChest = csr.Method;

        modder.OnRewritingMethodBody += Modder_OnRewritingMethodBody;

        // add playerID for the hook to consume, then we need to rewrite all the uses of it to add it in.
        PutItemInNearbyChest.Parameters.Add(PlayerID = new ParameterDefinition(
            "playerID",
             ParameterAttributes.None,
            modder.Module.TypeSystem.Int32
        ));

        // inject the hook
        {
            var beginInstruction = csr.Method.Body.Instructions.Single(x => x.OpCode == OpCodes.Bge_Un);
            var endInstruction = beginInstruction.Next(x => x.OpCode == OpCodes.Ldc_I4_0);

            csr.Goto(beginInstruction, MoveType.After);

            csr.EmitAll(
                new { OpCodes.Ldarg, Operand = csr.Method.Parameters.Skip(2).SingleOrDefault() },
                new { OpCodes.Ldarg, Operand = csr.Method.Parameters.First() },
                new { OpCodes.Ldloc_0 },
                new { OpCodes.Call, Operand = modder.GetMethodDefinition(() => OTAPI.Hooks.Chest.InvokeQuickStack(0, null, 0)) },
                new { OpCodes.Brtrue, endInstruction },
                new { OpCodes.Br, Operand = (Instruction)beginInstruction.Operand }
            );
        }
    }

    private static void Modder_OnRewritingMethodBody(MonoModder modder, MethodBody body, Instruction instr, int instri)
    {
        if (instr.Operand is MethodReference methodReference)
        {
            if (methodReference.DeclaringType.Name == PutItemInNearbyChest.DeclaringType.Name
                && methodReference.Name == PutItemInNearbyChest.Name)
            {
                if (methodReference.Parameters.Any(x => x.Name == PlayerID.Name))
                {
                    return;
                }
                methodReference.Parameters.Add(PlayerID);

                switch (body.Method.Name)
                {
                    case "ServerPlaceItem":
                        // add the player id from the first argument
                        body.GetILProcessor().InsertBefore(instr, new
                        {
                            OpCodes.Ldarg_0
                        });

                        break;
                    case "QuickStackAllChests":
                        body.GetILProcessor().InsertBefore(instr,
                            new { OpCodes.Ldarg_0, },
#if !tModLoaderServer_V1_3 
                            new { OpCodes.Ldfld, Operand = (FieldReference)modder.GetFieldDefinition(() => (new Terraria.Player()).whoAmI) }
#else
                            new { OpCodes.Ldfld, Operand = (FieldReference)modder.GetFieldDefinition(() => (new Terraria.Player(true)).whoAmI) }
#endif
                        );
                        break;
                    default:
                        throw new NotImplementedException($"{body.Method.Name} is not a supported caller for this modification");
                }
            }
        }
    }
}

namespace OTAPI
{
    public static partial class Hooks
    {
        public static partial class Chest
        {
            public class QuickStackEventArgs : EventArgs
            {
                public HookResult? Result { get; set; }

                public int PlayerId { get; set; }
                public Terraria.Item Item { get; set; }
                public int ChestIndex { get; set; }
            }
            public static event EventHandler<QuickStackEventArgs> QuickStack;

            public static bool InvokeQuickStack(int playerId, Terraria.Item item, int chestIndex)
            {
                var args = new QuickStackEventArgs()
                {
                    PlayerId = playerId,
                    Item = item,
                    ChestIndex = chestIndex,
                };
                QuickStack?.Invoke(null, args);
                return args.Result != HookResult.Cancel;
            }
        }
    }
}