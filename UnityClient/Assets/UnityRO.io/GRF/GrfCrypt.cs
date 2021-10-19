
using System;
namespace ROIO.GRF
{

    public static class GrfCrypt
    {
        /** Encryption processing.
        * Used to tell grfcrypt.c's functions to encrypt rather than decrypt. */
        public static byte GRFCRYPT_ENCRYPT = 0x00;
        /** Decryption processing.
         * Used to tell grfcrypt.c's functions to decrypt rather than encrypt. */
        public static byte GRFCRYPT_DECRYPT = 0x01;

        /*! Used to map bit number to bit */
        private static byte[] BitMap = new byte[] {
        0x80, 0x40, 0x20, 0x10, 0x8, 0x4, 0x2, 0x1
    };

        /* Some DES tables */
        private static byte[][] tables0x40 = {
	    /* Initial Permutation (IP) */
	    new byte[] {
            0x3A, 0x32, 0x2A, 0x22, 0x1A, 0x12, 0x0A, 0x02,
            0x3C, 0x34, 0x2C, 0x24, 0x1C, 0x14, 0x0C, 0x04,
            0x3E, 0x36, 0x2E, 0x26, 0x1E, 0x16, 0x0E, 0x06,
            0x40, 0x38, 0x30, 0x28, 0x20, 0x18, 0x10, 0x08,
            0x39, 0x31, 0x29, 0x21, 0x19, 0x11, 0x09, 0x01,
            0x3B, 0x33, 0x2B, 0x23, 0x1B, 0x13, 0x0B, 0x03,
            0x3D, 0x35, 0x2D, 0x25, 0x1D, 0x15, 0x0D, 0x05,
            0x3F, 0x37, 0x2F, 0x27, 0x1F, 0x17, 0x0F, 0x07
        },
	    /* Inverse Initial Permutation (IP^-1) */
	    new byte[] {
            0x28, 0x08, 0x30, 0x10, 0x38, 0x18, 0x40, 0x20,
            0x27, 0x07, 0x2F, 0x0F, 0x37, 0x17, 0x3F, 0x1F,
            0x26, 0x06, 0x2E, 0x0E, 0x36, 0x16, 0x3E, 0x1E,
            0x25, 0x05, 0x2D, 0x0D, 0x35, 0x15, 0x3D, 0x1D,
            0x24, 0x04, 0x2C, 0x0C, 0x34, 0x14, 0x3C, 0x1C,
            0x23, 0x03, 0x2B, 0x0B, 0x33, 0x13, 0x3B, 0x1B,
            0x22, 0x02, 0x2A, 0x0A, 0x32, 0x12, 0x3A, 0x1A,
            0x21, 0x01, 0x29, 0x09, 0x31, 0x11, 0x39, 0x19
        },
	    /* 8 Selection functions (S) */
	    new byte[] {
            0x0E, 0x00, 0x04, 0x0F, 0x0D, 0x07, 0x01, 0x04,
            0x02, 0x0E, 0x0F, 0x02, 0x0B, 0x0D, 0x08, 0x01,
            0x03, 0x0A, 0x0A, 0x06, 0x06, 0x0C, 0x0C, 0x0B,
            0x05, 0x09, 0x09, 0x05, 0x00, 0x03, 0x07, 0x08,
            0x04, 0x0F, 0x01, 0x0C, 0x0E, 0x08, 0x08, 0x02,
            0x0D, 0x04, 0x06, 0x09, 0x02, 0x01, 0x0B, 0x07,
            0x0F, 0x05, 0x0C, 0x0B, 0x09, 0x03, 0x07, 0x0E,
            0x03, 0x0A, 0x0A, 0x00, 0x05, 0x06, 0x00, 0x0D
        }, new byte[] {
            0x0F, 0x03, 0x01, 0x0D, 0x08, 0x04, 0x0E, 0x07,
            0x06, 0x0F, 0x0B, 0x02, 0x03, 0x08, 0x04, 0x0E,
            0x09, 0x0C, 0x07, 0x00, 0x02, 0x01, 0x0D, 0x0A,
            0x0C, 0x06, 0x00, 0x09, 0x05, 0x0B, 0x0A, 0x05,
            0x00, 0x0D, 0x0E, 0x08, 0x07, 0x0A, 0x0B, 0x01,
            0x0A, 0x03, 0x04, 0x0F, 0x0D, 0x04, 0x01, 0x02,
            0x05, 0x0B, 0x08, 0x06, 0x0C, 0x07, 0x06, 0x0C,
            0x09, 0x00, 0x03, 0x05, 0x02, 0x0E, 0x0F, 0x09
        }, new byte[] {
            0x0A, 0x0D, 0x00, 0x07, 0x09, 0x00, 0x0E, 0x09,
            0x06, 0x03, 0x03, 0x04, 0x0F, 0x06, 0x05, 0x0A,
            0x01, 0x02, 0x0D, 0x08, 0x0C, 0x05, 0x07, 0x0E,
            0x0B, 0x0C, 0x04, 0x0B, 0x02, 0x0F, 0x08, 0x01,
            0x0D, 0x01, 0x06, 0x0A, 0x04, 0x0D, 0x09, 0x00,
            0x08, 0x06, 0x0F, 0x09, 0x03, 0x08, 0x00, 0x07,
            0x0B, 0x04, 0x01, 0x0F, 0x02, 0x0E, 0x0C, 0x03,
            0x05, 0x0B, 0x0A, 0x05, 0x0E, 0x02, 0x07, 0x0C
        }, new byte[] {
            0x07, 0x0D, 0x0D, 0x08, 0x0E, 0x0B, 0x03, 0x05,
            0x00, 0x06, 0x06, 0x0F, 0x09, 0x00, 0x0A, 0x03,
            0x01, 0x04, 0x02, 0x07, 0x08, 0x02, 0x05, 0x0C,
            0x0B, 0x01, 0x0C, 0x0A, 0x04, 0x0E, 0x0F, 0x09,
            0x0A, 0x03, 0x06, 0x0F, 0x09, 0x00, 0x00, 0x06,
            0x0C, 0x0A, 0x0B, 0x01, 0x07, 0x0D, 0x0D, 0x08,
            0x0F, 0x09, 0x01, 0x04, 0x03, 0x05, 0x0E, 0x0B,
            0x05, 0x0C, 0x02, 0x07, 0x08, 0x02, 0x04, 0x0E
        }, new byte[] {
            0x02, 0x0E, 0x0C, 0x0B, 0x04, 0x02, 0x01, 0x0C,
            0x07, 0x04, 0x0A, 0x07, 0x0B, 0x0D, 0x06, 0x01,
            0x08, 0x05, 0x05, 0x00, 0x03, 0x0F, 0x0F, 0x0A,
            0x0D, 0x03, 0x00, 0x09, 0x0E, 0x08, 0x09, 0x06,
            0x04, 0x0B, 0x02, 0x08, 0x01, 0x0C, 0x0B, 0x07,
            0x0A, 0x01, 0x0D, 0x0E, 0x07, 0x02, 0x08, 0x0D,
            0x0F, 0x06, 0x09, 0x0F, 0x0C, 0x00, 0x05, 0x09,
            0x06, 0x0A, 0x03, 0x04, 0x00, 0x05, 0x0E, 0x03
        }, new byte[] {
            0x0C, 0x0A, 0x01, 0x0F, 0x0A, 0x04, 0x0F, 0x02,
            0x09, 0x07, 0x02, 0x0C, 0x06, 0x09, 0x08, 0x05,
            0x00, 0x06, 0x0D, 0x01, 0x03, 0x0D, 0x04, 0x0E,
            0x0E, 0x00, 0x07, 0x0B, 0x05, 0x03, 0x0B, 0x08,
            0x09, 0x04, 0x0E, 0x03, 0x0F, 0x02, 0x05, 0x0C,
            0x02, 0x09, 0x08, 0x05, 0x0C, 0x0F, 0x03, 0x0A,
            0x07, 0x0B, 0x00, 0x0E, 0x04, 0x01, 0x0A, 0x07,
            0x01, 0x06, 0x0D, 0x00, 0x0B, 0x08, 0x06, 0x0D
        }, new byte[] {
            0x04, 0x0D, 0x0B, 0x00, 0x02, 0x0B, 0x0E, 0x07,
            0x0F, 0x04, 0x00, 0x09, 0x08, 0x01, 0x0D, 0x0A,
            0x03, 0x0E, 0x0C, 0x03, 0x09, 0x05, 0x07, 0x0C,
            0x05, 0x02, 0x0A, 0x0F, 0x06, 0x08, 0x01, 0x06,
            0x01, 0x06, 0x04, 0x0B, 0x0B, 0x0D, 0x0D, 0x08,
            0x0C, 0x01, 0x03, 0x04, 0x07, 0x0A, 0x0E, 0x07,
            0x0A, 0x09, 0x0F, 0x05, 0x06, 0x00, 0x08, 0x0F,
            0x00, 0x0E, 0x05, 0x02, 0x09, 0x03, 0x02, 0x0C
        }, new byte[] {
            0x0D, 0x01, 0x02, 0x0F, 0x08, 0x0D, 0x04, 0x08,
            0x06, 0x0A, 0x0F, 0x03, 0x0B, 0x07, 0x01, 0x04,
            0x0A, 0x0C, 0x09, 0x05, 0x03, 0x06, 0x0E, 0x0B,
            0x05, 0x00, 0x00, 0x0E, 0x0C, 0x09, 0x07, 0x02,
            0x07, 0x02, 0x0B, 0x01, 0x04, 0x0E, 0x01, 0x07,
            0x09, 0x04, 0x0C, 0x0A, 0x0E, 0x08, 0x02, 0x0D,
            0x00, 0x0F, 0x06, 0x0C, 0x0A, 0x09, 0x0D, 0x00,
            0x0F, 0x03, 0x03, 0x05, 0x05, 0x06, 0x08, 0x0B
        }
    };
        private static byte[][] tables0x30 = {
	    /* Permuted Choice 2 (PC-2) */
	    new byte[] {
            0x0E, 0x11, 0x0B, 0x18, 0x01, 0x05, 0x03, 0x1C,
            0x0F, 0x06, 0x15, 0x0A, 0x17, 0x13, 0x0C, 0x04,
            0x1A, 0x08, 0x10, 0x07, 0x1B, 0x14, 0x0D, 0x02,
            0x29, 0x34, 0x1F, 0x25, 0x2F, 0x37, 0x1E, 0x28,
            0x33, 0x2D, 0x21, 0x30, 0x2C, 0x31, 0x27, 0x38,
            0x22, 0x35, 0x2E, 0x2A, 0x32, 0x24, 0x1D, 0x20
        },
	    /* Bit-selection table (E) */
	    new byte[] {
            0x20, 0x01, 0x02, 0x03, 0x04, 0x05,
            0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D,
            0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11,
            0x10, 0x11, 0x12, 0x13, 0x14, 0x15,
            0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
            0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D,
            0x1C, 0x1D, 0x1E, 0x1F, 0x20, 0x01
        }
    };
        private static byte[][] tables0x20 = {
	    /* P */
	    new byte[] {
            0x10, 0x07, 0x14, 0x15,
            0x1D, 0x0C, 0x1C, 0x11,
            0x01, 0x0F, 0x17, 0x1A,
            0x05, 0x12, 0x1F, 0x0A,
            0x02, 0x08, 0x18, 0x0E,
            0x20, 0x1B, 0x03, 0x09,
            0x13, 0x0D, 0x1E, 0x06,
            0x16, 0x0B, 0x04, 0x19
        }
    };

        private static byte[] DES_IP = tables0x40[0];     /** Initial Permutation (IP) */
        private static byte[] DES_IP_INV = tables0x40[1]; /** Final Permutatioin (IP^-1) */
        private static byte[] DES_E = tables0x30[1];      /** Bit-selection (E) */
        private static byte[] DES_S(int num)
        {
            return tables0x40[2 + num];
        }
        private static byte[] DES_P = tables0x20[0];      /** (P) */

        public static byte[] GRFProcess(byte[] src, uint len, uint flags, uint digitsGen, byte[] ks)
        {
            uint i;
            byte digits;

            if ((flags & GrfTypes.GRFFILE_FLAG_MIXCRYPT) != 0)
            {
                byte[] dst = new byte[src.Length];

                /* Determine the number of digits */
                for (i = digitsGen, digits = 0; i > 0; i /= 0xA, digits++) ;
                if (digits < 1)
                {
                    digits = 1;
                }

                /* Decrypt/encrypt the data */
                GRF_MixedProcess(dst, src, len, digits, ks, GRFCRYPT_DECRYPT);

                return dst;
            }
            else if ((flags & GrfTypes.GRFFILE_FLAG_0x14_DES) != 0)
            {
                byte[] dst = new byte[src.Length];

                /* Copy all the blocks past 0x14 */
                i = len / 8;
                if (i > 0x14)
                {
                    i = 0x14;
                    Array.ConstrainedCopy(src, 0x14 * 8, dst, 0x14 * 8, (int)(len - 0x14 * 8));
                }

                /* Decrypt/encrypt the data */
                DES_Process(dst, src, i * 8, ks, GRFCRYPT_DECRYPT);

                return dst;
            }
            else
            {
                return src;
            }
        }

        /** Function to process data with GRFFILE_FLAG_MIXCRYPT set
         *
         * @warning Pointers aren't checked to be valid and of at least len length
         *
         * @param dst Pointer to where destination (processed) should be stored
         * @param src Pointer to source (unprocessed) data
         * @param len Length of the data to process
         * @param cycle uint32_t describing how often the data should be run through
         *		the DES functions
         * @param ks Pointer to the 0x80 byte key schedule
         * @param dir Direction of processing (GRFCRYPT_DECRYPT or GRFCRYPT_ENCRYPT)
         * @return Duplicate pointer to the data stored in dst
         */
        public static byte[] GRF_MixedProcess(byte[] dst, byte[] src, uint len, byte cycle, byte[] ks, byte dir)
        {
            uint i;
            byte j, tmp;
            byte[] orig;

            orig = dst;

            /* Modify the cycle */
            if (cycle < 3)
            {
                cycle = 1;
            }
            else if (cycle < 5)
            {
                cycle++;
            }
            else if (cycle < 7)
            {
                cycle += 9;
            }
            else
            {
                cycle += 0xF;
            }

            int offset = 0;

            for (i = j = 0; i < (len / 8); i++, offset += 8)
            {
                /* Check if its one of the first 0x14, or if its evenly
                 * divisible by cycle
                 */
                if (i < 0x14 || (i % cycle) == 0)
                {
                    byte[] offsetedDst = new byte[dst.Length - offset];
                    byte[] offsetedSrc = new byte[src.Length - offset];
                    Array.ConstrainedCopy(dst, offset, offsetedDst, 0, offsetedDst.Length);
                    Array.ConstrainedCopy(src, offset, offsetedSrc, 0, offsetedSrc.Length);
                    DES_ProcessBlock(1, offsetedDst, offsetedSrc, ks, dir);
                    Array.ConstrainedCopy(offsetedDst, 0, dst, offset, offsetedDst.Length);
                    Array.ConstrainedCopy(offsetedSrc, 0, src, offset, offsetedSrc.Length);
                }
                else
                {
                    /* Check if its time to modify byte order */
                    if (j == 7)
                    {
                        /* Swap around some bytes */
                        if (dir == GRFCRYPT_DECRYPT)
                        {
                            // 3450162
                            Array.ConstrainedCopy(src, offset + 3, dst, offset, 2);
                            // 01_____
                            dst[offset + 2] = src[offset + 6];
                            // 012____
                            Array.ConstrainedCopy(src, offset, dst, offset + 3, 3);
                            // 012345_
                            dst[offset + 6] = src[offset + 5];
                            // 0123456
                        }
                        else
                        {
                            // 0123456
                            Array.ConstrainedCopy(src, offset, dst, offset + 3, 2);
                            // ___01__
                            dst[offset + 6] = src[offset + 2];
                            // ___01_2
                            Array.ConstrainedCopy(src, offset + 3, dst, offset, 3);
                            // 34501_2
                            dst[offset + 5] = src[offset + 6];
                            // 3450162
                        }

                        /* Modify byte 7 */
                        if ((tmp = src[offset + 7]) <= 0x77)
                        {
                            if (tmp == 0x77)     /* 0x77 */
                                dst[offset + 7] = 0x48;
                            else if (tmp == 0)       /* 0x00 */
                                dst[offset + 7] = 0x2B;
                            else if ((--tmp) == 0)   /* 0x01 */
                                dst[offset + 7] = 0x68;
                            else if ((tmp -= 0x2A) == 0) /* 0x2B */
                                dst[offset + 7] = 0x00;
                            else if ((tmp -= 0x1D) == 0) /* 0x48 */
                                dst[offset + 7] = 0x77;
                            else if ((tmp -= 0x18) == 0) /* 0x60 */
                                dst[offset + 7] = 0xFF;
                            else if ((tmp -= 0x08) == 0) /* 0x68 */
                                dst[offset + 7] = 0x01;
                            else if ((tmp -= 0x04) == 0) /* 0x6C */
                                dst[offset + 7] = 0x80;
                            else
                                dst[offset + 7] = src[offset + 7];
                        }
                        else
                        {
                            if ((tmp -= 0x80) == 0)  /* 0x80 */
                                dst[offset + 7] = 0x6C;
                            else if ((tmp -= 0x39) == 0) /* 0xB9 */
                                dst[offset + 7] = 0xC0;
                            else if ((tmp -= 0x07) == 0) /* 0xC0 */
                                dst[offset + 7] = 0xB9;
                            else if ((tmp -= 0x2B) == 0) /* 0xEB */
                                dst[offset + 7] = 0xFE;
                            else if ((tmp -= 0x13) == 0) /* 0xFE */
                                dst[offset + 7] = 0xEB;
                            else if ((--tmp) == 0)   /* 0xFF */
                                dst[offset + 7] = 0x60;
                            else
                                dst[offset + 7] = src[offset + 7];
                        }

                        j = 0;

                    }
                    else
                    {
                        Array.ConstrainedCopy(src, offset, dst, offset, 8);
                    }

                    j++;
                }
            }

            return orig;
        }

        /** Private DES function to property process a block of data
         *
         * Calls DES_Permutation and DES_RawProcessBlock to
         *	appropriately encrypt or decrypt an 8-byte block of data
         *
         * @warning Memory is not checked to be valid or of proper length
         *
         * @param rounds Number of times to process the block (GRF always uses 1)
         * @param dst Location in memory to store the processed block of data
         * @param src Location in memory to retrieve the unprocessed block of data
         * @param ks 0x80-byte key schedule to process the data against
         * @param dir Direction the processing is going, one of GRFCRYPT_DECRYPT
         *		or GRFCRYPT_ENCRYPT
         */
        private static byte[] DES_ProcessBlock(byte rounds, byte[] dst, byte[] src, byte[] ks, byte dir)
        {
            uint i;
            byte[] tmp = new byte[4];

            /* Copy src to dst */
            Array.Copy(src, dst, 8);

            /* Run the initial permutation */
            DES_Permutation(dst, DES_IP);

            if (rounds > 0)
            {
                for (i = 0; i < rounds; i++)
                {
                    byte[] nks = new byte[ks.Length];
                    int offset = (int)(dir == GRFCRYPT_DECRYPT ? 0xF - i : i) * 8;
                    Array.ConstrainedCopy(ks, offset, nks, 0, ks.Length - offset);
                    DES_RawProcessBlock(dst, nks);

                    /* Swap L and R */
                    Array.ConstrainedCopy(dst, 0, tmp, 0, 4);
                    Array.ConstrainedCopy(dst, 4, dst, 0, 4);
                    Array.ConstrainedCopy(tmp, 0, dst, 4, 4);
                }
            }

            /* Swap L and R a final time */
            Array.ConstrainedCopy(dst, 0, tmp, 0, 4);
            Array.ConstrainedCopy(dst, 4, dst, 0, 4);
            Array.ConstrainedCopy(tmp, 0, dst, 4, 4);

            /* Run the final permutation */
            DES_Permutation(dst, DES_IP_INV);

            return dst;
        }

        /** Private DES function to permutate a block.
         *
         * @warning block and table aren't checked for length or validity
         *
         * @param block A 64-bit block of data to be encrypted/decrypted
         * @param table One of DES_IP or DES_IP_INV, to select initial or final
         *		permutation of the block
         * @return A duplicate pointer to the block parameter
         */
        private static byte[] DES_Permutation(byte[] block, byte[] table)
        {
            byte[] tmpblock = new byte[8];
            byte tmp;
            uint i;

            for (i = 0; i < 0x40; i++)
            {
                tmp = (byte)(table[i] - 1);
                if ((block[tmp >> 3] & BitMap[tmp & 7]) != 0)
                    tmpblock[i >> 3] |= BitMap[i & 7];
            }

            Array.Copy(tmpblock, block, 8);

            return block;
        }

        /** Private function to process a block after its been permutated
         *
         * @warning block and ks aren't checked for length or validity
         *
         * @param block 8-byte block in memory to be DES crypted
         * @param ks 8-byte keyschedule to use (one of 16 in an array)
         * @return A duplicate pointer of block
         */
        private static byte[] DES_RawProcessBlock(byte[] block, byte[] ks)
        {
            int i, tmp;
            byte[][] tmpblock = new byte[2][];
            tmpblock[0] = new byte[8];
            tmpblock[1] = new byte[8];

            /* Use E to expand R from block into tmpblock */
            for (i = 0; i < 0x30; i++)
            {
                tmp = DES_E[i] + 0x1F;
                if ((block[tmp >> 3] & BitMap[tmp & 7]) != 0)
                    tmpblock[0][i / 6] |= BitMap[i % 6];
            }

            /* bitwise XOR the keyschedule against the expanded block */
            for (i = 0; i < 8; i++)
                tmpblock[0][i] ^= ks[i];

            /* Run the S functions */
            for (i = 0; i < 8; i++)
            {
                if (i % 2 != 0)
                    tmpblock[1][i >> 1] += DES_S(i)[tmpblock[0][i] >> 2];
                else
                    tmpblock[1][i >> 1] = (byte)(DES_S(i)[tmpblock[0][i] >> 2] << 4);
            }

            tmpblock[0] = new byte[8];
            /* Run the P function against the output of the S functions */
            for (i = 0; i < 0x20; i++)
            {
                tmp = DES_P[i] - 1;
                if ((tmpblock[1][tmp >> 3] & BitMap[tmp & 7]) != 0)
                    tmpblock[0][i >> 3] |= BitMap[i & 7];
            }

            /* XOR the 32 bit converted R against the old L */
            block[0] ^= tmpblock[0][0];
            block[1] ^= tmpblock[0][1];
            block[2] ^= tmpblock[0][2];
            block[3] ^= tmpblock[0][3];

            return block;
        }

        /** DES function to process a set amount of data.
         *
         * @param dst Destination of processed data
         * @param src Source of unprocessed data
         * @param len Length of data to be processed
         * @param ks Pointer to the 0x80 byte key schedule
         * @param dir Direction of processing (GRFCRYPT_DECRYPT or GRFCRYPT_ENCRYPT)
         * @return A duplicate pointer to the data of dst
         */
        private static byte[] DES_Process(byte[] dst, byte[] src, uint len, byte[] ks, byte dir)
        {
            uint i;
            byte[] orig;

            orig = dst;

            int offset = 0;
            for (i = 0; i < len / 8; i++, offset += 8)
            {

                byte[] offsetedDst = new byte[dst.Length - offset];
                byte[] offsetedSrc = new byte[src.Length - offset];
                Array.ConstrainedCopy(dst, offset, offsetedDst, 0, offsetedDst.Length);
                Array.ConstrainedCopy(src, offset, offsetedSrc, 0, offsetedSrc.Length);
                DES_ProcessBlock(1, offsetedDst, offsetedSrc, ks, dir);
                Array.ConstrainedCopy(offsetedDst, 0, dst, offset, offsetedDst.Length);
                Array.ConstrainedCopy(offsetedSrc, 0, src, offset, offsetedSrc.Length);
            }

            return orig;
        }
    }
}
