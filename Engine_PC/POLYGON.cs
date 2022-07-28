using Microsoft.Xna.Framework;

namespace Engine_PC;

public struct POLYGON
{
	public int[] v;

	public int[] e;

	public Vector2[] t;

	public Vector2[] ct;

	public uint Material;

	public Vector3 Normal;

	public Vector3 CurrentNormal;

	public Addict2Shading Shading;

	public Addict2Shading CurrentShading;

	public bool Selected;

	public bool Highlighted;
}
