namespace ROIO.GRF
{
    public class GrfFile
    {
        /** Size in file.
         * If using any form of DES encryption, this must be a multiple of 8 (which is the block size).
         */
        public uint compressed_len_aligned;
        public uint compressed_len;        /**  Compressed size */
        public uint real_len;          /**  Original (decompressed) file size */
        public uint pos;               /** location in GRF archive */

        /* Directories have specific sizes and offsets, even though
         * no data is stored inside the GRF file
         */

        /** GrfFile::compressed_len_aligned value used for directory entries */
        public static uint GRFFILE_DIR_SZFILE = 0x0714;
        /** GrfFile::compressed_len value used for directory entries */
        public static uint GRFFILE_DIR_SZSMALL = 0x0449;
        /** GrfFile::real_len value used for directory entries */
        public static uint GRFFILE_DIR_SZORIG = 0x055C;
        /** GrfFile::pos value used for directory entries */
        public static uint GRFFILE_DIR_OFFSET = 0x058A;

        /** Flags of file.
         * Such as whether its a file or not, and what encryption methods it uses */
        public byte flags;

        /* Known flags for GRF/GPF files */
        public static uint GRFFILE_FLAG_FILE = 0x01;    /** File entry
						 * GrfFile::type flag to specify that
                         * entry is a file when set(and
                         * directory when not set)
						 */
        public static uint GRFFILE_FLAG_MIXCRYPT = 0x02;    /** Encrypted
						 * GrfFile::type flag to specify that the file
                         * uses mixed crypto, explained in grfcrypt.h.

                         */
        public static uint GRFFILE_FLAG_0x14_DES = 0x04;    /** Encrypted
						 * GrfFile::type flag to specify that only the
                         * first 0x14 blocks are encrypted.

                         * Explained in grfcrypt.h
                         */

        public uint hash;          /**  Filename hash; used internally by grf_find() */
        public string name;     /**  Filename */
        public string Path
        {
            get { return Grf.NormalizePath(name); }
        }

        /* This is calculated when the file is crypted, and only used
         * for GRFFILE_FLAG_MIXCRYPT, to determine how often to use
         * DES encryption.
         * Commented out because it doesn't appear in GRAVITY's struct
         */
        /* uint32_t cycle; */

        /* Extra data (which is not found in GRAVITY's struct) */
        public byte[] data = null; /**  Uncompressed file data */

        public bool IsDir()
        {
            return ((flags & GrfTypes.GRFFILE_FLAG_FILE) == 0 || (
            (compressed_len_aligned == GrfTypes.GRFFILE_DIR_SZFILE) &&
            (compressed_len == GrfTypes.GRFFILE_DIR_SZSMALL) &&
            (real_len == GrfTypes.GRFFILE_DIR_SZORIG) &&
            (pos == GrfTypes.GRFFILE_DIR_OFFSET)));
        }

    }
}