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
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
/// <summary>
/// @doc Patches the entity class to allow instance mods.
/// </summary>
namespace Terraria
{
    public abstract class patch_Entity : Terraria.Entity
    {
        public object EntityMod { get; set; }

        public Dictionary<string, object> EntityData { get; }

        [MonoMod.MonoModConstructor]
        patch_Entity()
        {
            EntityData = new Dictionary<string, object>();
        }
    }
}