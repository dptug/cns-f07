namespace Engine_PC;

internal class Addict2ObjectData
{
	public uint Wireframe;

	public uint NormalsInverted;

	public uint XSwap;

	public uint YSwap;

	public uint Swap;

	public uint Zmask;

	public uint AEpsilon;

	public uint SRCBlend;

	public uint DSTBlend;

	public uint Textured;

	public uint TexSlot;

	public uint Envmapped;

	public uint EnvSlot;

	public uint Shading;

	public uint XTile;

	public uint YTile;

	public uint Offx;

	public uint Offy;

	public uint Red;

	public uint Green;

	public uint Blue;

	public uint Alpha;

	public uint ReadBit(uint nData, int nOffset, int nSize)
	{
		return (uint)((nData >> nOffset) & ((1 << nSize) - 1));
	}

	public Addict2ObjectData(uint i)
	{
		int num = 0;
		Wireframe = ReadBit(i, num, 1);
		num++;
		NormalsInverted = ReadBit(i, num, 1);
		num++;
		XSwap = ReadBit(i, num, 1);
		num++;
		YSwap = ReadBit(i, num, 1);
		num++;
		Swap = ReadBit(i, num, 1);
		num++;
		Zmask = ReadBit(i, num, 1);
		num++;
		AEpsilon = ReadBit(i, num, 1);
		num++;
		SRCBlend = ReadBit(i, num, 4);
		num += 4;
		DSTBlend = ReadBit(i, num, 3);
		num += 3;
		Textured = ReadBit(i, num, 1);
		num++;
		TexSlot = ReadBit(i, num, 3);
		num += 3;
		Envmapped = ReadBit(i, num, 1);
		num++;
		EnvSlot = ReadBit(i, num, 3);
		num += 3;
		Shading = ReadBit(i, num, 2);
		num += 2;
		XTile = ReadBit(i, num, 1);
		num++;
		YTile = ReadBit(i, num, 1);
		num++;
		Offx = ReadBit(i, num, 1);
		num++;
		Offy = ReadBit(i, num, 1);
		num++;
		Red = ReadBit(i, num, 1);
		num++;
		Green = ReadBit(i, num, 1);
		num++;
		Blue = ReadBit(i, num, 1);
		num++;
		Alpha = ReadBit(i, num, 1);
		num++;
	}
}
