public abstract class TileProvider
{
	public int Width { get; private set; }
	public int Height { get; private set; }

	public TileProvider(int width, int height)
	{
		Width = width;
		Height = height;
	}

	public virtual void ResetSize(int width, int height)
    {
		Width = width;
		Height = height;
    }

	public virtual bool TileInBounds(int x, int y)
	{
		return x >= 0 && x < Width && y >= 0 && y < Height;
	}

	public abstract bool IsTileWalkable(int x, int y);

	public abstract bool IsBlockInstaKillOn(int x, int y);
	public bool IsBlockInstakill(World.BlockType blockType)
    {
		return ConfigData.IsBlockInstakill(blockType);
	}

	public bool IsBlockCloud(World.BlockType blockType)
	{
		return blockType == World.BlockType.CloudPlatform || blockType == World.BlockType.TrapdoorMetalPlatform;
	}

	public bool IsBlockCloudOn(int x, int y) { return this.IsBlockCloud(krak.KrakMonoBehaviour.world.GetBlockType(x, y)); }
}