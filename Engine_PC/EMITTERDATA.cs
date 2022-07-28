using Microsoft.Xna.Framework;

namespace Engine_PC;

public class EMITTERDATA
{
	public int DefaultAge;

	public int AgeChaos;

	public float ParticlesPerFrame;

	public float ParticleNumBuffer;

	public float d;

	public Vector3 n;

	public PARTICLE[] Particles;

	public int MaxParticles;

	public int LastFrameChecked;

	public bool Head;

	public bool Tail;

	public bool ObjectHead;

	public byte TailLength;

	public byte TailLength2;

	public Addict2Scene ObjectHeadScene;

	public int ObjectHeadSceneID;

	public uint HeadMaterial;

	public float[] Color1;

	public float[] Color2;

	public float Param1;

	public float Param2;

	public float Size;

	public float CamFade;

	public float Rotspeed;

	public float RotChaos;

	public float Speed;

	public float SpeedChaos;

	public float DirChaos;

	public Vector3 Dir;

	public bool RandRotate;

	public bool FixedUp;
}
