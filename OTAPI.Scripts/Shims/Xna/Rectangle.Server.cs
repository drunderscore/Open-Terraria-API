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

namespace Microsoft.Xna.Framework
{
	public struct Rectangle
	{
		private static Rectangle _empty = default(Rectangle);

		public static Rectangle Empty {
			get
            { return Rectangle._empty; }
		}

		public int Left {
			get
            { return this.X; }
		}

		public int Right {
			get
            { return this.X + this.Width; }
		}

		public int Top {
			get
            { return this.Y; }
		}

		public int Bottom {
			get
            { return this.Y + this.Height; }
		}

		public Point Location {
			get {
				return new Point (this.X, this.Y);
			}
			set {
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		public Point Center {
			get {
				return new Point (this.X + this.Width / 2, this.Y + this.Height / 2);
			}
		}

		public static Rectangle[] Array;

		public int X;
		public int Y;
		public int Width;
		public int Height;

		public Rectangle (int x, int y, int width, int height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		public bool Intersects(Rectangle value)
		{
			if (value.X < X + Width && X < value.X + value.Width && value.Y < Y + Height)
			{
				return Y < value.Y + value.Height;
			}
			return false;
		}

		public void Intersects(ref Rectangle value, out bool result)
		{
			result = (value.X < X + Width && X < value.X + value.Width && value.Y < Y + Height && Y < value.Y + value.Height);
		}

		public static Rectangle Intersect(Rectangle value1, Rectangle value2)
		{
			int num = value1.X + value1.Width;
			int num2 = value2.X + value2.Width;
			int num3 = value1.Y + value1.Height;
			int num4 = value2.Y + value2.Height;
			int num5 = (value1.X > value2.X) ? value1.X : value2.X;
			int num6 = (value1.Y > value2.Y) ? value1.Y : value2.Y;
			int num7 = (num < num2) ? num : num2;
			int num8 = (num3 < num4) ? num3 : num4;
			Rectangle result = default(Rectangle);
			if (num7 > num5 && num8 > num6)
			{
				result.X = num5;
				result.Y = num6;
				result.Width = num7 - num5;
				result.Height = num8 - num6;
			}
			else
			{
				result.X = 0;
				result.Y = 0;
				result.Width = 0;
				result.Height = 0;
			}
			return result;
		}

		public static void Intersect(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
		{
			int num = value1.X + value1.Width;
			int num2 = value2.X + value2.Width;
			int num3 = value1.Y + value1.Height;
			int num4 = value2.Y + value2.Height;
			int num5 = (value1.X > value2.X) ? value1.X : value2.X;
			int num6 = (value1.Y > value2.Y) ? value1.Y : value2.Y;
			int num7 = (num < num2) ? num : num2;
			int num8 = (num3 < num4) ? num3 : num4;
			if (num7 > num5 && num8 > num6)
			{
				result.X = num5;
				result.Y = num6;
				result.Width = num7 - num5;
				result.Height = num8 - num6;
			}
			else
			{
				result.X = 0;
				result.Y = 0;
				result.Width = 0;
				result.Height = 0;
			}
		}


		public bool Contains (Point value)
		{
			return this.X <= value.X && value.X < this.X + this.Width && this.Y <= value.Y && value.Y < this.Y + this.Height;
		}

		public bool Contains (int x, int y)
		{
			return this.X <= x && x < this.X + this.Width && this.Y <= y && y < this.Y + this.Height;
		}

		public void Contains (ref Point value, out bool result)
		{
			result = (this.X <= value.X && value.X < this.X + this.Width && this.Y <= value.Y && value.Y < this.Y + this.Height);
		}

		public bool Contains (Rectangle value)
		{
			return this.X <= value.X && value.X + value.Width <= this.X + this.Width && this.Y <= value.Y && value.Y + value.Height <= this.Y + this.Height;
		}

		public void Contains (ref Rectangle value, out bool result)
		{
			result = (this.X <= value.X && value.X + value.Width <= this.X + this.Width && this.Y <= value.Y && value.Y + value.Height <= this.Y + this.Height);
		}

		public void Offset (Point value)
		{
			this.X += value.X;
			this.Y += value.Y;
		}

		public void Offset (int x, int y)
		{
			this.X += x;
			this.Y += y;
		}

		public void Inflate (int width, int height)
		{
			this.X -= width;
			this.Y -= height;
			this.Width += width * 2;
			this.Height += height * 2;
		}

		public static Rectangle Union (Rectangle r1, Rectangle r2)
		{
			int num = r1.X + r1.Width;
			int num2 = r2.X + r2.Width;
			int num3 = r1.Y + r1.Height;
			int num4 = r2.Y + r2.Height;
			int num5 = (r1.X < r2.X) ? r1.X : r2.X;
			int num6 = (r1.Y < r2.Y) ? r1.Y : r2.Y;
			int num7 = (num > num2) ? num : num2;
			int num8 = (num3 > num4) ? num3 : num4;
			Rectangle result;
			result.X = num5;
			result.Y = num6;
			result.Width = num7 - num5;
			result.Height = num8 - num6;
			return result;
		}

		public bool Equals (Rectangle other)
		{
			return this.X == other.X && this.Y == other.Y && this.Width == other.Width && this.Height == other.Height;
		}

		public override bool Equals (object obj)
		{
			bool result = false;
			if (obj is Rectangle rec)
			{
				result = this.Equals (rec);
			}
			return result;
		}

		public override int GetHashCode ()
		{
			return this.X.GetHashCode () + this.Y.GetHashCode () + this.Width.GetHashCode () + this.Height.GetHashCode ();
		}

		public static bool operator == (Rectangle r1, Rectangle r2)
		{
			return r1.X == r2.X && r1.Y == r2.Y && r1.Width == r2.Width && r1.Height == r2.Height;
		}

		public static bool operator != (Rectangle r1, Rectangle r2)
		{
			return r1.X != r2.X || r1.Y != r2.Y || r1.Width != r2.Width || r1.Height != r2.Height;
		}
	}
}