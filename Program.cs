using System;
using System.Diagnostics;
using System.Text;

namespace TestConversions
{
    class Program
    {
        private static int TwoByteAddressLength = 6;

        private static string ConvertToReturnBytesValue(string memoryData, string displayUnits, string returnFormat)
        {
            // we expect two byte hex: this format '0x0000'
            if (memoryData.Length != TwoByteAddressLength)
            {
                return memoryData;
            }

            string result = memoryData;
            bool displayIsHEX = (displayUnits.ToLower() == "hex");

            // values are U1, U2, I2
            switch (returnFormat)
            {
                case "U1":
                    // "0x0012"  -> "0x12"
                    result = result.Remove(2, 2);
                    if (displayIsHEX) break;

                    uint u1Value = Convert.ToUInt16(result, 16);
                    result = u1Value.ToString();
                    break;

                case "U2":
                    if (displayIsHEX) break;

                    uint u2Value = Convert.ToUInt16(result, 16);
                    result = u2Value.ToString();
                    break;

                case "I2":
                    if (displayIsHEX) break;

                    int i2Value = Convert.ToInt16(result, 16);
                    result = i2Value.ToString();
                    break;

                default:
                    break;
            }

            return result;
        }

        private static string GetHexValueAsString(ushort[] words, ushort startWordIndex, ushort returnByteCount,
             bool startFirstByteOfWord = true, bool DataIsLittleEndian = false)
        {
            byte[] memoryDataBytes = new byte[returnByteCount];
            ushort currentWordIndex = startWordIndex;
            int byteStart = startFirstByteOfWord ? 0 : 1;

            int bytesAdded = 0;
            for (int byteIndex = byteStart; bytesAdded < returnByteCount; byteIndex++)
            {
                ushort currentWord = words[currentWordIndex];
                byte byteValue;

                if (byteIndex % 2 == 0)
                {
                    byteValue = (byte)(currentWord >> 8);

                }
                else
                {
                    byteValue = (byte)currentWord;
                    currentWordIndex++;
                }
                memoryDataBytes[bytesAdded] = byteValue;
                bytesAdded++;
            }

            StringBuilder sb = new StringBuilder("0x");

            int count = memoryDataBytes.Length;
            if (DataIsLittleEndian)
            {
                // Reverse the bytes for output
                for (int i = count - 1; i >= 0; i--)
                {
                    _ = sb.Append(memoryDataBytes[i].ToString("X2").ToUpper());
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    _ = sb.Append(memoryDataBytes[i].ToString("X2").ToUpper());
                }
            }
            return sb.ToString();
        }

        static void Main(string[] args)
        {
            ushort[] words = new ushort[12];
            words[0] = 1234;
            words[1] = 1234;
            words[2] = 1234;
            words[3] = 0;
            words[4] = 0x4F52; //- 0x4F52(big-endian)  20306

            const ushort ReadDataMemoryDataWordStartPosition = 4;
            const ushort MemoryDataByteCount = 2;
            bool DataIsLittleEndian = false;

            string memoryData = GetHexValueAsString(
                words,
                ReadDataMemoryDataWordStartPosition,
                MemoryDataByteCount,
                true,
                DataIsLittleEndian
            );
            Debug.Assert(memoryData == "0x4F52", string.Format("Little-endian Hex string data does not equal '0x4F52' {0}", memoryData));

            string value = ConvertToReturnBytesValue(memoryData, "hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "I2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "I2");
            Console.WriteLine(value);


            words[4] = 0x524F;   // little-endian  20306
            DataIsLittleEndian = true;
            memoryData = GetHexValueAsString(
                words,
                ReadDataMemoryDataWordStartPosition,
                MemoryDataByteCount,
                true,
                DataIsLittleEndian
            );
            Debug.Assert(memoryData == "0x4F52", string.Format("Little-endian Hex string data does not equal '0x4F52' {0}", memoryData));

            value = ConvertToReturnBytesValue(memoryData, "hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "I2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "I2");
            Console.WriteLine(value);


            words[4] = 0x0019; //  0x0x0019(big-endian) = 25
            DataIsLittleEndian = false;

            memoryData = GetHexValueAsString(
                words,
                ReadDataMemoryDataWordStartPosition,
                MemoryDataByteCount,
                true,
                DataIsLittleEndian
            );
            Debug.Assert(memoryData == "0x0019", string.Format("Little-endian Hex string data does not equal '0x0019' {0}", memoryData));

            value = ConvertToReturnBytesValue(memoryData, "hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "I2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "I2");
            Console.WriteLine(value);


            words[4] = 0x1900;          // little-endian = 25
            DataIsLittleEndian = true;
            memoryData = GetHexValueAsString(
                words,
                ReadDataMemoryDataWordStartPosition,
                MemoryDataByteCount,
                true,
                DataIsLittleEndian
            );
            Debug.Assert(memoryData == "0x0019", string.Format("Little-endian Hex string data does not equal '0x0019' {0}", memoryData));

            value = ConvertToReturnBytesValue(memoryData, "hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "I2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "I2");
            Console.WriteLine(value);

            //////////////////////////////

            words[4] = 0xFDE8;               // big-endian = 65000
            DataIsLittleEndian = false;

            memoryData = GetHexValueAsString(
                words,
                ReadDataMemoryDataWordStartPosition,
                MemoryDataByteCount,
                true,
                DataIsLittleEndian
            );
            Debug.Assert(memoryData == "0xFDE8", string.Format("Little-endian Hex string data does not equal '0xFDE8' {0}", memoryData));

            value = ConvertToReturnBytesValue(memoryData, "hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "I2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "I2");
            Console.WriteLine(value);

            words[4] = 0xE8FD;               // little-endian = 65000
            DataIsLittleEndian = true;
            memoryData = GetHexValueAsString(
                words,
                ReadDataMemoryDataWordStartPosition,
                MemoryDataByteCount,
                true,
                DataIsLittleEndian
            );
            Debug.Assert(memoryData == "0xFDE8", string.Format("Little-endian Hex string data does not equal '0xFDE8' {0}", memoryData));

            value = ConvertToReturnBytesValue(memoryData, "hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "I2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "I2");
            Console.WriteLine(value);


            //////////////////////////

            words[4] = 0x3039;             // big-endian 12345
            DataIsLittleEndian = false;

            memoryData = GetHexValueAsString(
                words,
                ReadDataMemoryDataWordStartPosition,
                MemoryDataByteCount,
                true,
                DataIsLittleEndian
            );
            Debug.Assert(memoryData == "0x3039", string.Format("Little-endian Hex string data does not equal '0x3039' {0}", memoryData));

            value = ConvertToReturnBytesValue(memoryData, "hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "I2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "I2");
            Console.WriteLine(value);


            DataIsLittleEndian = true;
            words[4] = 0x3930;             // little-endian 12345
            memoryData = GetHexValueAsString(
                words,
                ReadDataMemoryDataWordStartPosition,
                MemoryDataByteCount,
                true,
                DataIsLittleEndian
            );
            Debug.Assert(memoryData == "0x3039", string.Format("Little-endian Hex string data does not equal '0x3039' {0}", memoryData));

            value = ConvertToReturnBytesValue(memoryData, "hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "hex", "I2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U1");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "U2");
            Console.WriteLine(value);

            value = ConvertToReturnBytesValue(memoryData, "not hex", "I2");
            Console.WriteLine(value);

        }
    }
}
