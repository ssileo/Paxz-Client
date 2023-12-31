﻿using Priority_Queue;

public class PNode : FastPriorityQueueNode
{
	public int X { get; private set; }
	public int Y { get; private set; }

	public static PNode Create(int x, int y)
	{
		return new PNode(x, y);
	}

	private PNode(int x, int y)
	{
		X = x;
		Y = y;
	}

	// Uncomment these and add a reference to UnityEngine if you want to covert to and from Vector2Ints.

	//public static implicit operator Vector2(PNode pn)
	//{
	//    return new Vector2(pn.X, pn.Y);
	//}

	//public static explicit operator Vector2Int(PNode pn)
	//{
	//    return new Vector2Int(pn.X, pn.Y);
	//}

	public override bool Equals(object obj)
	{
		var other = (PNode)obj;
		return X == other.X && Y == other.Y;
	}

	public override int GetHashCode()
	{
		return X + Y * 7;
	}

	public override string ToString()
	{
		return "(" + X + ", " + Y + ")";
	}
}