using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_PC;

public class WORLDOBJECT
{
	public int ID;

	public int ParentID;

	public Addict2Scene DataScene;

	public Addict2World DataWorld;

	public Addict2WorldType Primitive;

	public Matrix ModelView;

	public Matrix TransformBuffer;

	public Matrix Inverted;

	public Matrix IT;

	public RST Position;

	public EMITTERDATA EmitterData;

	public Blend SRCBlend;

	public Blend DSTBlend;

	public bool Textured;

	public bool ZMask;

	public Addict2Material Material;

	public DEFAULTOBJECTSPLINES[] Animations;

	public int AnimNum;

	public int AnimCapacity;

	public RST PosData;

	public ANIMPOS APosData;

	public RGBA Color;

	public WORLDOBJECT Parent;

	public byte AEpsilon;

	public int TargetID;
}
