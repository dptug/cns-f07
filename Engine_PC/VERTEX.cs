using Microsoft.Xna.Framework;

namespace Engine_PC;

public struct VERTEX
{
	public Vector3 Position;

	public Vector3 MapTransformedPosition;

	public Vector3 Normal;

	public Vector3 CurrentNormal;

	public Vector2 TextureCoordinate;

	public Vector2 CurrentTextureCoordinate;

	public int[] EdgeList;

	public int EdgeNum;

	public int EdgeCapacity;

	public float[] Weight;

	public bool Selected;

	public int SelectCount;

	public int PolyCount;
}
