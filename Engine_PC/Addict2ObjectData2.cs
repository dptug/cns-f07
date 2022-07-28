namespace Engine_PC;

internal class Addict2ObjectData2
{
	public bool BackFace;

	public bool BackFront;

	public bool Orientation;

	public bool Position;

	public Addict2ObjectData2(byte i)
	{
		BackFace = (i & 1) > 0;
		BackFront = (i & 2) > 0;
		Orientation = (i & 4) > 0;
		Position = (i & 8) > 0;
	}
}
