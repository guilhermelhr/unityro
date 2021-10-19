
namespace ROIO.GRF
{
    public class GrfTypes
    {
        /** Maximum length of filenames.
         * @note GRAVITY uses 0x100 as a length for filenames.
         */
        public static uint GRF_NAMELEN = 0x100;

        /** GrfFile::compressed_len_aligned value used for directory entries */
        public static uint GRFFILE_DIR_SZFILE = 0x0714;
        /** GrfFile::compressed_len value used for directory entries */
        public static uint GRFFILE_DIR_SZSMALL = 0x0449;
        /** GrfFile::real_len value used for directory entries */
        public static uint GRFFILE_DIR_SZORIG = 0x055C;
        /** GrfFile::pos value used for directory entries */
        public static uint GRFFILE_DIR_OFFSET = 0x058A;

        /* Known flags for GRF/GPF files */
        public static uint GRFFILE_FLAG_FILE = 0x01; /** File entry
                         * GrfFile::type flag to specify that
                         * entry is a file when set(and
                         * directory when not set)
						 */

        public static uint GRFFILE_FLAG_MIXCRYPT = 0x02;/** Encrypted
						 * GrfFile::type flag to specify that the file
                         * uses mixed crypto, explained in grfcrypt.h.
                         */
        public static uint GRFFILE_FLAG_0x14_DES = 0x04;/** Encrypted
						 * GrfFile::type flag to specify that only the
                         * first 0x14 blocks are encrypted.

                         * Explained in grfcrypt.h
                         */
    }
}
