using Microsoft.Xna.Framework;

namespace Engine_PC;

public struct PARTICLE
{
	public Vector3 Position;

	public Vector3 LastPos;

	public Vector3 DisplayPos;

	public Vector3 DisplayPos2;

	public Vector3 Speed;

	public int TailCnt;

	public int TailCnt2;

	public int Age;

	public int StartAge;

	public bool Aging;

	public bool Active;

	public byte[] Color1;

	public byte[] Color2;

	public Vector3[] Tail;

	public float Rotation;

	public float RotChaos;

	public float Size;
}
