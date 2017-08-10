namespace Globitrust.Net.P2PMessaging
{
	/// <summary>
	/// A static class representing a collections of bit operations.
	/// </summary>
	static class BitConverter
	{
		#region Get Bytes

		/// <summary>
		/// Returns the specified integer as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes representing the specified number.</returns>
		public static byte[] GetBytes(int value)
		{
			var toReturn = System.BitConverter.GetBytes(value);
			CheckByteArray(toReturn);
			return toReturn;
		}

		/// <summary>
		/// Returns the specified unsigned integer as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes representing the specified number.</returns>
		public static byte[] GetBytes(uint value)
		{
			var toReturn = System.BitConverter.GetBytes(value);
			CheckByteArray(toReturn);
			return toReturn;
		}

		#endregion

		#region Check Bytes Array

		/// <summary>
		/// Reverses an array of bytes if necessary.
		/// </summary>
		/// <param name="toCheck">The array of bytes to reverse.</param>
		static void CheckByteArray(byte[] toCheck)
		{
			if (System.BitConverter.IsLittleEndian)
				System.Array.Reverse(toCheck);
		}

		#endregion

		#region To UInt32

		/// <summary>
		/// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A UInt32 representing the specified value.</returns>
		public static System.UInt32 ToUInt32(byte[] value, int startIndex)
		{
			var toConvert = new byte[4];
			System.Buffer.BlockCopy(value, startIndex, toConvert, 0, 4);

			if (System.BitConverter.IsLittleEndian)
			{
				System.Array.Reverse(toConvert);
			}

			return System.BitConverter.ToUInt32(toConvert, 0);
		}

		#endregion

	}
}
