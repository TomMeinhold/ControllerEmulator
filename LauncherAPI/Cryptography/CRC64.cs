using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace LauncherAPI.Cryptography
{
    public class CRC64 : HashAlgorithm
    {
        public const ulong DefaultSeed = 0x0;

        public const ulong Iso3309Polynomial = 0xD800000000000000;

        internal static ulong[] Table;

        private readonly ulong[] table;

        private readonly ulong seed;

        private ulong hash;

        public CRC64()
            : this(Iso3309Polynomial)
        {
        }

        public CRC64(ulong polynomial)
            : this(polynomial, DefaultSeed)
        {
        }

        public CRC64(ulong polynomial, ulong seed)
        {
            if (!BitConverter.IsLittleEndian)
#pragma warning disable CA1303 //
                throw new PlatformNotSupportedException("Not supported on Big Endian processors");
#pragma warning restore CA1303 //

            this.table = InitializeTable(polynomial);
            this.seed = this.hash = seed;
        }

        public static new CRC64 Create() => new CRC64();

        public override void Initialize()
        {
            this.hash = this.seed;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            this.hash = CalculateHash(this.hash, this.table, array, ibStart, cbSize);
        }

        protected override byte[] HashFinal()
        {
            var hashBuffer = UInt64ToBigEndianBytes(this.hash);
            this.HashValue = hashBuffer;
            return hashBuffer;
        }

        public override int HashSize { get { return 64; } }

        protected static ulong CalculateHash(ulong seed, ulong[] table, IList<byte> buffer, int start, int size)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            var hash = seed;
            for (var i = start; i < start + size; i++)
                unchecked
                {
                    if (table != null)
                    {
                        hash = (hash >> 8) ^ table[(buffer[i] ^ hash) & 0xff];
                    }
                }
            return hash;
        }

        private static byte[] UInt64ToBigEndianBytes(ulong value)
        {
            var result = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }

        private static ulong[] InitializeTable(ulong polynomial)
        {
            if (polynomial == Iso3309Polynomial && Table != null)
                return Table;

            var createTable = CreateTable(polynomial);

            if (polynomial == Iso3309Polynomial)
                Table = createTable;

            return createTable;
        }

        protected static ulong[] CreateTable(ulong polynomial)
        {
            var createTable = new ulong[256];
            for (var i = 0; i < 256; ++i)
            {
                var entry = (ulong)i;
                for (var j = 0; j < 8; ++j)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry >>= 1;
                createTable[i] = entry;
            }
            return createTable;
        }
    }
}