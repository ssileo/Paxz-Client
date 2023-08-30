using BasicTypes;
using Priority_Queue;
using PaxzClient;
using System.Collections.Generic;
using PaxzClient;

public class Pathfinding
{
	public const int MAX = 5000;

	public const float DIAGONAL_DST = 1.41421356237f;
	//public const float DIAGONAL_DST = 0;

	private FastPriorityQueue<PNode> open = new FastPriorityQueue<PNode>(MAX);
	private Dictionary<PNode, PNode> cameFrom = new Dictionary<PNode, PNode>();
	private Dictionary<PNode, float> costSoFar = new Dictionary<PNode, float>();
	private List<PNode> near = new List<PNode>();
	private bool left, right, below, above;

	public PathfindingResult Run(int startX, int startY, int endX, int endY, TileProvider provider,
		out List<PNode> path)
	{
		if (provider == null)
		{
			path = null;
			return PathfindingResult.Path_Not_Found;
		}

		// Validate start and end points.
		if (!provider.TileInBounds(startX, startY))
		{
			/*path = null;
            return PathfindingResult.ERROR_START_OUT_OF_BOUNDS;*/
		}

		if (!provider.TileInBounds(endX, endY))
		{
			path = null;
			return PathfindingResult.ERROR_END_OUT_OF_BOUNDS;
		}

		if (!provider.IsTileWalkable(startX, startY))
		{
			/*path = null;
            return PathfindingResult.Start_Not_Valid;*/
		}

		if (!provider.IsTileWalkable(endX, endY) && !provider.IsBlockCloudOn(endX, endY) && !provider.IsBlockInstaKillOn(endX, endY) && !ConfigData.IsBlockPlatform(krak.KrakMonoBehaviour.world.GetBlockType(endX, endY)))
		{
			path = null;
			return PathfindingResult.Invalid_Ending_Pos;
		}

		// Clear everything up.
		Clear();

		var start = PNode.Create(startX, startY);
		var end = PNode.Create(endX, endY);

		// Check the start/end relationship.
		if (start.Equals(end))
		{
			/*path = null;
            return PathfindingResult.Same_Block;*/
		}

		// Add the starting point to all relevant structures.
		open.Enqueue(start, 0f);
		cameFrom[start] = start;
		costSoFar[start] = 0f;

		int count;
		while ((count = open.Count) > 0)
		{
			// Detect if the current open amount exceeds the capacity.
			// This only happens in very large open areas. Corridors and hallways will never cause this, not matter how large the actual path length.
			if (count >= MAX - 8)
			{
				path = null;
				return PathfindingResult.ERROR_PATH_TOO_LONG;
			}

			var current = open.Dequeue();
			krak.KrakMonoBehaviour.lastpos = new Vector2i(current.X, current.Y);
			if (current.Equals(end))
			{
				// We found the end of the path!
				path = TracePath(end);
				return PathfindingResult.SUCCESSFUL;
			}

			// Get all neighbours (tiles that can be walked on to)
			var neighbours = GetNear(current, provider);
			foreach (var n in neighbours)
			{
				var newCost =
					costSoFar[current] +
					GetCost(current, n); // Note that this could change depending on speed changes per-tile.

				if (!costSoFar.ContainsKey(n) || newCost < costSoFar[n])
				{
					costSoFar[n] = newCost;
					var priority = newCost + Heuristic(current, n);
					open.Enqueue(n, priority);
					cameFrom[n] = current;
				}
			}
		}

		path = null;
		return PathfindingResult.Path_Not_Found;
	}

	private List<PNode> TracePath(PNode end)
	{
		var path = new List<PNode>();
		var child = end;

		var run = true;
		while (run)
		{
			var previous = cameFrom[child];
			path.Add(child);
			if (previous != null && child != previous)
				child = previous;
			else
				run = false;
		}

		path.Reverse();
		return path;
	}

	public void Clear()
	{
		costSoFar.Clear();
		cameFrom.Clear();
		near.Clear();
		open.Clear();
	}

	private float Abs(float x)
	{
		if (x < 0)
			return -x;
		return x;
	}

	private float Heuristic(PNode a, PNode b)
	{
		// Gives a rough distance.
		return Abs(a.X - b.X) + Abs(a.Y - b.Y);
	}

	private float GetCost(PNode a, PNode b)
	{
		// Only intended for neighbours.

		// Is directly horzontal
		if (Abs(a.X - b.X) == 1 && a.Y == b.Y) return 1;

		// Directly vertical.
		if (Abs(a.Y - b.Y) == 1 && a.X == b.X) return 1;

		// Assume that it is on one of the corners.
		return DIAGONAL_DST;
	}

	private List<PNode> GetNear(PNode node, TileProvider provider)
	{
		// Want to add nodes connected to the center node, if they are walkable.
		// This code stops the pathfinder from cutting corners, and going through walls that are diagonal from each other.

		near.Clear();

		// Left
		left = provider.IsTileWalkable(node.X - 1, node.Y);

		// Right
		right = provider.IsTileWalkable(node.X + 1, node.Y);

		// Above
		above = provider.IsTileWalkable(node.X, node.Y + 1);

		// Below
		below = provider.IsTileWalkable(node.X, node.Y - 1);

		/* ---- */

		bool aboveLeft = provider.IsBlockInstaKillOn(node.X - 1, node.Y + 1) || provider.IsTileWalkable(node.X - 1, node.Y + 1);
		bool aboveRight = provider.IsBlockInstaKillOn(node.X + 1, node.Y + 1) || provider.IsTileWalkable(node.X + 1, node.Y + 1);
		bool belowLeft = provider.IsBlockInstaKillOn(node.X - 1, node.Y - 1) || provider.IsTileWalkable(node.X - 1, node.Y - 1);
		bool belowRight = provider.IsBlockInstaKillOn(node.X + 1, node.Y - 1) || provider.IsTileWalkable(node.X + 1, node.Y - 1);

		if (above) near.Add(PNode.Create(node.X, node.Y + 1));
		if (below && (int)krak.KrakMonoBehaviour.world.GetBlockType(node.X, node.Y) != 110) near.Add(PNode.Create(node.X, node.Y - 1)); /* Entrance Check */

		if (left) near.Add(PNode.Create(node.X - 1, node.Y));
		if (right) near.Add(PNode.Create(node.X + 1, node.Y));

		if (aboveLeft && (above || left)) near.Add(PNode.Create(node.X - 1, node.Y + 1));
		if (aboveRight && (above || right)) near.Add(PNode.Create(node.X + 1, node.Y + 1));
		if (belowLeft && (below || left)) near.Add(PNode.Create(node.X - 1, node.Y - 1));
		if (belowRight && (below || right)) near.Add(PNode.Create(node.X + 1, node.Y - 1));

		return near;
	}
}