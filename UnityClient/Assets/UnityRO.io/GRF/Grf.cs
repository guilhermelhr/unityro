using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace ROIO.GRF
{
    public class Grf
    {
        /* Headers */
        public static string GRF_HEADER = "Master of Magic";
        public static int GRF_HEADER_LEN = GRF_HEADER.Length;
        public static int GRF_HEADER_MID_LEN = GRF_HEADER_LEN + 0xF;    /* -1 + 0xF */
        public static int GRF_HEADER_FULL_LEN = GRF_HEADER_LEN + 0x1F;
        public static uint GRF_TYPE_GRF = 0x01; /** Value to distinguish a GRF file in Grf::type */

        string filename;
        protected uint len;
        protected uint type;
        protected uint version;
        protected uint nfiles;

        public Hashtable files;

        protected byte allowCrypt;
        protected bool allowWrite;

        public static Grf grf_callback_open(string fname, string mode, GrfOpenCallback callback)
        {
            byte[] buf = new byte[GRF_HEADER_FULL_LEN];
            uint i;//, zero_fcount = GrfSupport.ToLittleEndian32(7), create_ver = GrfSupport.ToLittleEndian32(0x0200);
            Grf grf;

            if (fname == null || mode == null)
            {
                throw new Exception("GE_BADARGS");
            }

            /* Allocate memory for grf */
            grf = new Grf();

            /* Copy the filename */
            grf.filename = fname;

            /* Open the file */
            var fStream = FileManager.Load(grf.filename) as Stream;

            if (fStream == null)
            {
                throw new Exception("GE_ERRNO");
            }

            using (var br = new System.IO.BinaryReader(fStream))
            {
                grf.allowWrite = !mode.Contains("+") && mode.Contains("w");

                //***skipped write***/

                /* Read the header */
                buf = br.ReadBytes(GRF_HEADER_FULL_LEN);

                /* Check the header */
                string strA = Encoding.ASCII.GetString(buf);
                int v = string.Compare(strA, 0, GRF_HEADER, 0, GRF_HEADER_LEN);
                if (v != 0)
                {
                    throw new Exception("GE_INVALID");
                }

                /* Continued header check...
                 *
                 * GRF files that allow encryption of the files inside the archive
                 * have a hex header following "Master of Magic" (not including
                 * the nul-terminator) that looks like:
                 * 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E
                 *
                 * GRF files that do not allow it have a hex header that looks like:
                 * 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
                 *
                 * GRF files that do not allow it are generally found after a
                 * "Ragnarok.exe /repak" command has been issued
                 */
                if (buf[GRF_HEADER_LEN + 1] == 1)
                {
                    grf.allowCrypt = 1;
                    /* 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E */
                    for (i = 0; i < 0xF; i++)
                    {
                        if (buf[GRF_HEADER_LEN + i] != (int)i)
                        {
                            throw new Exception("GE_CORRUPTED");
                        }
                    }
                }
                else if (buf[GRF_HEADER_LEN] == 0)
                {
                    grf.allowCrypt = 0;
                    /* 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 */
                    for (i = 0; i < 0xF; i++)
                    {
                        if (buf[GRF_HEADER_LEN + i] != 0)
                        {
                            new Exception("GE_CORRUPTED");
                        }
                    }
                }
                else
                {
                    throw new Exception("GE_CORRUPTED");

                }

                /* Okay, so we finally are sure that its a valid GRF/GPF file.
                 * now its time to read info from it
                 */

                /* Set the type of archive this is */
                grf.type = GRF_TYPE_GRF;

                /* Read the version */
                grf.version = GrfSupport.LittleEndian32(buf, GRF_HEADER_MID_LEN + 0xC);

                /* Read the number of files */
                grf.nfiles = GrfSupport.LittleEndian32(buf, GRF_HEADER_MID_LEN + 8);
                grf.nfiles -= GrfSupport.LittleEndian32(buf, GRF_HEADER_MID_LEN + 4) + 7;

                /* Create the array of files */
                grf.files = new Hashtable(StringComparer.OrdinalIgnoreCase);

                /* Grab the filesize */
                grf.len = (uint)fStream.Length;

                /* Seek to the offset of the file tables */
                br.BaseStream.Seek(GrfSupport.LittleEndian32(buf, GRF_HEADER_MID_LEN) + GRF_HEADER_FULL_LEN, SeekOrigin.Begin);

                /* Run a different function to read the file information based on
                 * the major version number
                 */
                switch (grf.version & 0xFF00)
                {
                    case 0x0200:
                        i = GRF_readVer2_info(br, grf, callback);
                        break;
                    default:
                        throw new Exception("UNSUP_GRF_VERSION");
                }

                if (i > 0)
                {
                    return null;
                }
            }

            return grf;
        }

        /*! \brief Private function to read GRF0x2xx headers
         *
         * Reads the information about files within the archive...
         * for archive versions 0x02xx
         *
         * \todo Find GRF versions other than just 0x200 (do any exist?)
         *
         * \param grf Pointer to the Grf struct to read to
         * \param error Pointer to a GrfErrorType struct/enum for error reporting
         * \param callback Function to call for each read file. It should return 0 if
         *		everything is fine, 1 if everything is fine (but further
         *		reading should stop), or -1 if there has been an error
         * \return 0 if everything went fine, 1 if something went wrong
         */
        static uint GRF_readVer2_info(System.IO.BinaryReader br, Grf grf, GrfOpenCallback callback)
        {
            uint i, offset, len, len2;
            byte[] buf, zbuf;

            /* Check grf */
            if (grf.version != 0x200)
            {
                throw new Exception("GE_NSUP");
            }

            /* Read the original and compressed sizes */
            buf = br.ReadBytes(8);

            /* Allocate memory and read the compressed file table */
            len = GrfSupport.LittleEndian32(buf, 0);
            zbuf = br.ReadBytes((int)len);

            if (0 == (len2 = GrfSupport.LittleEndian32(buf, 4)))
            {
                return 0;
            }

            /* Allocate memory and uncompress the compressed file table */
            Array.Resize(ref buf, (int)len2);

            var stream = new InflaterInputStream(new MemoryStream(zbuf));

            stream.Read(buf, 0, buf.Length);

            stream.Close();

            /* Read information about each file */
            for (i = offset = 0; i < grf.nfiles; i++)
            {
                /* Grab the filename length */
                len = GrfSupport.getCStringLength(buf, offset) + 1;

                /* Make sure its not too large */
                if (len >= GrfTypes.GRF_NAMELEN)
                {
                    throw new Exception("GE_CORRUPTED");
                }

                /* Grab filename */
                GrfFile file = new GrfFile();
                file.name = GrfSupport.getCString(buf, offset);

                offset += len;

                /* Grab the rest of the information */
                file.compressed_len = GrfSupport.LittleEndian32(buf, (int)offset);
                file.compressed_len_aligned = GrfSupport.LittleEndian32(buf, (int)offset + 4);
                file.real_len = GrfSupport.LittleEndian32(buf, (int)offset + 8);
                file.flags = buf[offset + 0xC];
                file.pos = GrfSupport.LittleEndian32(buf, (int)(offset + 0xD)) + (uint)GRF_HEADER_FULL_LEN;
                file.hash = GrfSupport.GRF_NameHash(file.name);

                file.name = NormalizePath(file.name);

                grf.files.Add(file.name, file);

                /* Advance to the next file */
                offset += 0x11;

                /* Run the callback, if we have one */
                if (callback != null)
                {
                    callback.doCallback(file);

                    if (!callback.hasReturned())
                    {
                        throw new Exception("NO_RETURN");
                    }

                    if (callback.Response < 0)
                    {
                        /* Callback function had an error, so we
                         * have an error
                         */
                        return 1;
                    }
                    else if (callback.Response > 0)
                    {
                        /* Callback function found the file it needed,
                         * just exit now
                         */
                        return 0;
                    }
                }
            }

            /* Calling functions will set success...
            GRF_SETERR(error,GE_SUCCESS,GRF_readVer2_info);
            */

            return 0;
        }

        public static string NormalizePath(string path)
        {
            return path.Replace('\\', '/');
        }

        private byte[] getFileZBlock(GrfFile file)
        {
            /* Get new stream */
            var stream = FileManager.Load(filename) as Stream;

            if (stream == null)
            {
                throw new Exception("GE_ERRNO");
            }

            byte[] data;
            using (var br = new System.IO.BinaryReader(stream))
            {
                br.BaseStream.Seek(file.pos, SeekOrigin.Begin);
                data = br.ReadBytes((int)file.compressed_len_aligned);
            }

            byte[] keyschedule = new byte[0x80];

            return GrfCrypt.GRFProcess(data, file.compressed_len_aligned, file.flags, file.compressed_len, keyschedule);
        }

        public byte[] GetData(GrfFile file)
        {
            //not a file
            if (file.IsDir())
            {
                return null;
            }

            //empty file
            if (file.real_len == 0)
            {
                return null;
            }

            //check if data was already extracted
            if (file.data != null)
            {
                return file.data;
            }

            /* Retrieve the unencrypted block */
            byte[] zbuf = getFileZBlock(file);
            if (zbuf == null)
            {
                return null;
            }

            using (var stream = new InflaterInputStream(new MemoryStream(zbuf)))
            {
                file.data = new byte[file.real_len];

                stream.Read(file.data, 0, file.data.Length);
            }

            return file.data;
        }

        public GrfFile GetDescriptor(string name)
        {
            return files[NormalizePath(name)] as GrfFile;
        }
    }
}

