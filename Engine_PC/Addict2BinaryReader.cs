using System.IO;
using Microsoft.Xna.Framework;

namespace Engine_PC;

public class Addict2BinaryReader : BinaryReader
{
	public Addict2BinaryReader(Stream s)
		: base(s)
	{
	}

	public unsafe float ReadCompactFloat(int nBytes)
	{
		uint num = 0u;
		for (int i = 0; i < nBytes; i++)
		{
			byte b = ReadByte();
			num |= (uint)(b << i * 8);
		}
		num <<= (4 - nBytes) * 8;
		float* ptr = (float*)(&num);
		return *ptr;
	}

	public Matrix ReadMatrix(int nBytes, bool bOrient, bool bPos)
	{
		Matrix matrix = default(Matrix);
		matrix = Matrix.Identity;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if ((bOrient && i < 3) || (bPos && i == 3))
				{
					if (j == 0 && i == 0)
					{
						matrix.M11 = ReadCompactFloat(nBytes);
					}
					if (j == 1 && i == 0)
					{
						matrix.M12 = ReadCompactFloat(nBytes);
					}
					if (j == 2 && i == 0)
					{
						matrix.M13 = ReadCompactFloat(nBytes);
					}
					if (j == 3 && i == 0)
					{
						matrix.M14 = ReadCompactFloat(nBytes);
					}
					if (j == 0 && i == 1)
					{
						matrix.M21 = ReadCompactFloat(nBytes);
					}
					if (j == 1 && i == 1)
					{
						matrix.M22 = ReadCompactFloat(nBytes);
					}
					if (j == 2 && i == 1)
					{
						matrix.M23 = ReadCompactFloat(nBytes);
					}
					if (j == 3 && i == 1)
					{
						matrix.M24 = ReadCompactFloat(nBytes);
					}
					if (j == 0 && i == 2)
					{
						matrix.M31 = ReadCompactFloat(nBytes);
					}
					if (j == 1 && i == 2)
					{
						matrix.M32 = ReadCompactFloat(nBytes);
					}
					if (j == 2 && i == 2)
					{
						matrix.M33 = ReadCompactFloat(nBytes);
					}
					if (j == 3 && i == 2)
					{
						matrix.M34 = ReadCompactFloat(nBytes);
					}
					if (j == 0 && i == 3)
					{
						matrix.M41 = ReadCompactFloat(nBytes);
					}
					if (j == 1 && i == 3)
					{
						matrix.M42 = ReadCompactFloat(nBytes);
					}
					if (j == 2 && i == 3)
					{
						matrix.M43 = ReadCompactFloat(nBytes);
					}
					if (j == 3 && i == 3)
					{
						matrix.M44 = ReadCompactFloat(nBytes);
					}
				}
			}
		}
		return matrix;
	}
}
