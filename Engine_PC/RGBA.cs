namespace Engine_PC;

public class RGBA
{
	public byte R;

	public byte G;

	public byte B;

	public byte A;

	public uint AsDword
	{
		get
		{
			uint num = 0u;
			num |= R;
			num |= (uint)(G << 8);
			num |= (uint)(B << 16);
			return num | (uint)(A << 24);
		}
	}

	public RGBA(byte R, byte G, byte B, byte A)
	{
		this.R = R;
		this.G = G;
		this.B = B;
		this.A = A;
	}

	public RGBA(uint D)
	{
		R = (byte)(D & 0xFFu);
		G = (byte)((D >> 8) & 0xFFu);
		B = (byte)((D >> 16) & 0xFFu);
		A = (byte)((D >> 24) & 0xFFu);
	}
}
