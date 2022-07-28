namespace Engine_PC;

internal class Addict2WorldObjectData
{
	public uint Posx;

	public uint Posy;

	public uint Posz;

	public uint Sclx;

	public uint Scly;

	public uint Sclz;

	public uint Quat;

	public uint EmitterData;

	public uint SRCBlend;

	public uint DSTBlend;

	public uint Textured;

	public uint TexSlot;

	public uint Tail;

	public uint Head;

	public uint ObjectHead;

	public uint TailRes1;

	public uint TailRes2;

	public uint ReadBit(uint nData, int nOffset, int nSize)
	{
		return (uint)((nData >> nOffset) & ((1 << nSize) - 1));
	}

	public Addict2WorldObjectData(uint i)
	{
		int num = 0;
		Posx = ReadBit(i, num, 1);
		num++;
		Posy = ReadBit(i, num, 1);
		num++;
		Posz = ReadBit(i, num, 1);
		num++;
		Sclx = ReadBit(i, num, 1);
		num++;
		Scly = ReadBit(i, num, 1);
		num++;
		Sclz = ReadBit(i, num, 1);
		num++;
		Quat = ReadBit(i, num, 1);
		num++;
		EmitterData = ReadBit(i, num, 1);
		num++;
		SRCBlend = ReadBit(i, num, 4);
		num += 4;
		DSTBlend = ReadBit(i, num, 3);
		num += 3;
		Textured = ReadBit(i, num, 1);
		num++;
		TexSlot = ReadBit(i, num, 3);
		num += 3;
		Tail = ReadBit(i, num, 1);
		num++;
		Head = ReadBit(i, num, 1);
		num++;
		ObjectHead = ReadBit(i, num, 1);
		num++;
		TailRes1 = ReadBit(i, num, 4);
		num += 4;
		TailRes2 = ReadBit(i, num, 4);
		num += 4;
	}
}
