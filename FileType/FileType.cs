/*
 * FileType.cs
 *
 * C# port of https://github.com/sindresorhus/file-type
 * by Ken T Ekeoha.
 * see License.md for license information.
 * 
 */

using System;
using System.Text;



public class FileType
{
    public class CheckOptions
    {
        public readonly int Offset;
        public readonly byte[] Mask;

        public CheckOptions(byte[] mask = null, int offset = 0)
        {
            this.Mask = mask;
            this.Offset = offset;
        }
        
    }
    public class FileTypeInfo
    {
        public readonly string Extension;
        public readonly string MimeType;

        public FileTypeInfo(string extension = "", string mimetype = "")
        {
            this.Extension = extension;
            this.MimeType = mimetype;
        }
    }

    protected static byte[] xpiZipFilename = Encoding.ASCII.GetBytes("META-INF/mozilla.rsa");
    protected static byte[] oxmlContentTypes = Encoding.ASCII.GetBytes("[Content_Types].xml");
    protected static byte[] oxmlRels = Encoding.ASCII.GetBytes("_rels/.rels");


    protected static int readUInt64LE(ref byte[] buffer, int offset = 0)
    {
        //let n = buf[offset];
        int n = buffer[offset];
        //let mul = 1;
        var mul = 1;
        // let i = 0;
        var i = 0;
        while (++i < 8)
        {
            mul *= 0x100;
            n += buffer[offset + i] * mul;
        }
        return n;
    }

    protected static bool checkString(ref byte[] buffer, String header, CheckOptions options = null)
    {
        return check(ref buffer, Encoding.ASCII.GetBytes(header), options);
    }

    protected static bool check(ref byte[] buffer, byte[] header, CheckOptions options = null)
    {
        if (options != null)
        {

            if (options.Offset >= buffer.Length)
            {
                return false;
            }

            for (var i = 0; i < header.Length; i++)
            {
                if (options.Mask != null)
                {
                    if (header[i] != (options.Mask[i] & buffer[i + options.Offset]))
                        return false;
                }
                else
                {
                    if (header[i] != (buffer[i + options.Offset]))
                        return false;
                }
            }
        }
        else
        {

            for (var i = 0; i < header.Length; i++)
            {

                if (header[i] != buffer[i])
                    return false;

            }
        }

        return true;
    }

    protected static int findNextZipHeaderIndex(ref byte[] arr, int startAt = 0)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (i >= startAt && arr[i] == 0x50 && arr[i + 1] == 0x4B && arr[i + 2] == 0x3 && arr[i + 3] == 0x4)
            {
                return i;
            }
        }
        return -1;
    }

    public static FileTypeInfo Get(ref byte[] buffer)
    {

        if (buffer == null || buffer.Length == 0)
            return null;

        // JPEG
        if (check(ref buffer, new byte[] { 0xFF, 0xD8, 0xFF }))
            return new FileTypeInfo("jpg", "image/jpg");

        // PNG
        if (check(ref buffer, new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }))
            return new FileTypeInfo("png", "image/png");

        // WEBP
        if (check(ref buffer, new byte[] { 0x57, 0x45, 0x42, 0x50 }, new CheckOptions(offset: 8)))
            return new FileTypeInfo("webp", "image/webp");

        // FLIF
        if (check(ref buffer, new byte[] { 0x46, 0x4C, 0x49, 0x46 }))
            return new FileTypeInfo("flif", "image/flif");

        // CR2
        if (
            (check(ref buffer, new byte[] { 0x49, 0x49, 0x2A, 0x0 }) ||
            check(ref buffer, new byte[] { 0x4D, 0x4D, 0x0, 0x2A })) &&
            check(ref buffer, new byte[] { 0x43, 0x52 }, new CheckOptions(offset: 8))
            )
            return new FileTypeInfo("cr2", "image/x-canon-cr2");

        // TIF
        if (
            check(ref buffer, new byte[] { 0x49, 0x49, 0x2A, 0x0 }) ||
            check(ref buffer, new byte[] { 0x4D, 0x4D, 0x0, 0x2A })
            )
            return new FileTypeInfo("tif", "image/tiff");

        // BMP
        if (check(ref buffer, new byte[] { 0x42, 0x4D }))
            return new FileTypeInfo("bmp", "image/bmp");

        // JXR
        if (check(ref buffer, new byte[] { 0x49, 0x49, 0xBC }))
            return new FileTypeInfo("jxr", "image/vnd.ms-photo");

        // PSD
        if (check(ref buffer, new byte[] { 0x38, 0x42, 0x50, 0x53 }))
            return new FileTypeInfo("psd", "image/vnd.adobe.photoshop");

        // Zip-based file formats
        // Need to be before the `zip` check
        if (check(ref buffer, new byte[] { 0x50, 0x4B, 0x3, 0x4 }))
        {
            // EPUB

            if (check(ref buffer, new byte[] { 0x6D, 0x69, 0x6D, 0x65, 0x74, 0x79, 0x70,
                0x65, 0x61, 0x70, 0x70, 0x6C, 0x69, 0x63, 0x61, 0x74, 0x69, 0x6F,
                0x6E, 0x2F, 0x65, 0x70, 0x75, 0x62, 0x2B, 0x7A, 0x69, 0x70 }, new CheckOptions(offset: 30)))
            {
                return new FileTypeInfo("epub", "application/epub+zip");
            }



            // Assumes signed `.xpi` from addons.mozilla.org
            if (check(ref buffer, xpiZipFilename, new CheckOptions(offset: 30)))
                return new FileTypeInfo("xpi", "application/x-xpinstall");



            if (checkString(ref buffer, "mimetypeapplication/vnd.oasis.opendocument.text", new CheckOptions(offset: 30)))
                return new FileTypeInfo("odt", "application/vnd.oasis.opendocument.text");


            if (checkString(ref buffer, "mimetypeapplication/vnd.oasis.opendocument.spreadsheet", new CheckOptions(offset: 30)))
                return new FileTypeInfo("ods", "application/vnd.oasis.opendocument.spreadsheet");


            if (checkString(ref buffer, "mimetypeapplication/vnd.oasis.opendocument.presentation", new CheckOptions(offset: 30)))
                return new FileTypeInfo("odp", "application/vnd.oasis.opendocument.presentation");



            // The docx, xlsx and pptx file types extend the Office Open XML file format:
            // https://en.wikipedia.org/wiki/Office_Open_XML_file_formats
            // We look for:
            // - one entry named '[Content_Types].xml' or '_rels/.rels',
            // - one entry indicating specific type of file.
            // MS Office, OpenOffice and LibreOffice may put the parts in different order, so the check should not rely on it.

            var zipHeaderIndex = 0; // The first zip header was already found at index 0
            bool oxmlFound = false;
            FileTypeInfo type = null;

            do
            {
                var _offset = zipHeaderIndex + 30;

                if (!oxmlFound)
                {
                    oxmlFound =

                        check(ref buffer, oxmlContentTypes, new CheckOptions(offset: _offset)) ||
                        check(ref buffer, oxmlRels, new CheckOptions(offset: _offset));
                }

                if (type == null)
                {
                    // DOCX
                    if (checkString(ref buffer, "word/", new CheckOptions(offset: _offset)))
                    {
                        type = new FileTypeInfo("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

                    }
                    // PPT
                    else if (checkString(ref buffer, "ppt/", new CheckOptions(offset: _offset)))
                    {
                        type = new FileTypeInfo("pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
                    }
                    // XLSX
                    else if (checkString(ref buffer, "xl/", new CheckOptions(offset: _offset)))
                    {
                        type = new FileTypeInfo("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                    }
                }
                if (oxmlFound && type != null)
                {
                    return type;
                }


                zipHeaderIndex = findNextZipHeaderIndex(ref buffer, _offset);
            } while (zipHeaderIndex >= 0);

            // No more zip parts available in the buffer, but maybe we are almost certain about the type?
            if (type != null)
            {
                return type;
            }
        }


        // ZIP
        if (
            check(ref buffer, new byte[] { 0x50, 0x4B }) &&
            (buffer[2] == 0x3 || buffer[2] == 0x5 || buffer[2] == 0x7) &&
            (buffer[3] == 0x4 || buffer[3] == 0x6 || buffer[3] == 0x8)
                    )
            return new FileTypeInfo("zip", "application/zip");


        // TAR
        if (check(ref buffer, new byte[] { 0x75, 0x73, 0x74, 0x61, 0x72 }, new CheckOptions(offset: 257)))
            return new FileTypeInfo("tar", "application/x-tar");


        // RAR
        if (
            check(ref buffer, new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x7 }) &&
            (buffer[6] == 0x0 || buffer[6] == 0x1))
            return new FileTypeInfo("rar", "application/x-rar-compressed");


        // GZ
        if (check(ref buffer, new byte[] { 0x1F, 0x8B, 0x8 }))
        {
            return new FileTypeInfo("gz", "application/gzip");
        }

        // BZ2
        if (check(ref buffer, new byte[] { 0x42, 0x5A, 0x68 }))
        {
            return new FileTypeInfo("bz2", "application/x-bzip2");
        }

        // 7Z
        if (check(ref buffer, new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C }))
        {
            return new FileTypeInfo("7z", "application/x-7z-compressed");
        }

        // DMG
        if (check(ref buffer, new byte[] { 0x78, 0x01 }))
        {
            return new FileTypeInfo("dmg", "application/x-apple-diskimage");
        }

        // MP4
        if (check(ref buffer, new byte[] { 0x33, 0x67, 0x70, 0x35 }) || // 3gp5
            (
                check(ref buffer, new byte[] { 0x0, 0x0, 0x0 }) &&
                check(ref buffer, new byte[] { 0x66, 0x74, 0x79, 0x70 }, new CheckOptions(offset: 4)) &&
(
check(ref buffer, new byte[] { 0x6D, 0x70, 0x34, 0x31 }, new CheckOptions(offset: 8)) || // MP41
check(ref buffer, new byte[] { 0x6D, 0x70, 0x34, 0x32 }, new CheckOptions(offset: 8)) || // MP42
check(ref buffer, new byte[] { 0x69, 0x73, 0x6F, 0x6D }, new CheckOptions(offset: 8)) || // ISOM
check(ref buffer, new byte[] { 0x69, 0x73, 0x6F, 0x32 }, new CheckOptions(offset: 8)) || // ISO2
check(ref buffer, new byte[] { 0x6D, 0x6D, 0x70, 0x34 }, new CheckOptions(offset: 8)) || // MMP4
check(ref buffer, new byte[] { 0x4D, 0x34, 0x56 }, new CheckOptions(offset: 8)) || // M4V
check(ref buffer, new byte[] { 0x64, 0x61, 0x73, 0x68 }, new CheckOptions(offset: 8)) // DASH
                    )
                    ))
            return new FileTypeInfo("mp4", "video/mp4");



        // MIDI
        if (check(ref buffer, new byte[] { 0x4D, 0x54, 0x68, 0x64 }))
        {
            return new FileTypeInfo("mid", "audio/midi");
        }

        // MKV
        // https://github.com/threatstack/libmagic/blob/master/magic/Magdir/matroska
        if (check(ref buffer, new byte[] { 0x1A, 0x45, 0xDF, 0xA3 }))
        {
            var sliced = new byte[4096];
            Array.Copy(buffer, 4, sliced, 0, sliced.Length); //buffer. (4, 4 + 4096);


            //private const idPos = sliced.findIndex((el, i, arr) => arr[i] === 0x42 && arr[i + 1] === 0x82);
            int idPos = -1;
            for (int i = 0; i < sliced.Length; i++)
            {
                if (sliced[i] == 0x42 && sliced[i + 1] == 0x82)
                {
                    idPos = i;
                }
            }


            if (idPos != -1)
            {
                var docTypePos = idPos + 3;

                //var findDocType = type => [...type].every((c, i) => sliced[docTypePos + i] === c.charCodeAt(0));
                var counter = 0;
                foreach (var _char in "matroska")
                {
                    if (sliced[docTypePos + counter] != (byte)_char)
                        break;

                    return new FileTypeInfo("mkv", "video/x-matroska");
                }

                counter = 0;
                foreach (var _char in "webm")
                {
                    if (sliced[docTypePos + counter] != (byte)_char)
                        break;

                    return new FileTypeInfo("webm", "video/webm");
                }
            }
        }

        // MOV
        if (check(ref buffer, new byte[] { 0x0, 0x0, 0x0, 0x14, 0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20 }) ||
            check(ref buffer, new byte[] { 0x66, 0x72, 0x65, 0x65 }, new CheckOptions(offset: 4)) || // Type: `free`
            check(ref buffer, new byte[] { 0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20 }, new CheckOptions(offset: 4)) ||
            check(ref buffer, new byte[] { 0x6D, 0x64, 0x61, 0x74 }, new CheckOptions(offset: 4)) || // MJPEG
            check(ref buffer, new byte[] { 0x6D, 0x6F, 0x6F, 0x76 }, new CheckOptions(offset: 4)) || // Type: `moov`
            check(ref buffer, new byte[] { 0x77, 0x69, 0x64, 0x65 }, new CheckOptions(offset: 4)))
            return new FileTypeInfo("mov", "video/quicktime");



        // RIFF file format which might be AVI, WAV, QCP, etc
        if (check(ref buffer, new byte[] { 0x52, 0x49, 0x46, 0x46 }))
        {
            if (check(ref buffer, new byte[] { 0x41, 0x56, 0x49 }, new CheckOptions(offset: 8)))
                return new FileTypeInfo("avi", "video/vnd.avi");


            if (check(ref buffer, new byte[] { 0x57, 0x41, 0x56, 0x45 }, new CheckOptions(offset: 8)))
            {

                return new FileTypeInfo("wav", "audio/vnd.wave");


            }
            // QLCM, QCP file
            if (check(ref buffer, new byte[] { 0x51, 0x4C, 0x43, 0x4D }, new CheckOptions(offset: 8)))
            {

                return new FileTypeInfo("qcp", "audio/qcpelp");

            }
        }


        // ASF_Header_Object first 80 bytes
        if (check(ref buffer, new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9 }))
        {
            // Search for header should be in first 1KB of file.

            var _offset = 30;
            do
            {
                var objectSize = readUInt64LE(ref buffer, _offset + 16);
                if (check(ref buffer, new byte[] { 0x91, 0x07, 0xDC, 0xB7, 0xB7, 0xA9, 0xCF, 0x11, 0x8E, 0xE6, 0x00, 0xC0, 0x0C, 0x20, 0x53, 0x65 },
                    new CheckOptions(offset: _offset)))
                {
                    // Sync on Stream-Properties-Object (B7DC0791-A9B7-11CF-8EE6-00C00C205365)
                    if (check(ref buffer, new byte[] { 0x40, 0x9E, 0x69, 0xF8, 0x4D, 0x5B, 0xCF, 0x11, 0xA8, 0xFD, 0x00, 0x80, 0x5F, 0x5C, 0x44, 0x2B }, new CheckOptions(offset: _offset + 24)))
                    {
                        // Found audio:
                        return new FileTypeInfo("wma", "audio/x-ms-wma");
                    }

                    if (check(ref buffer, new byte[] { 0xC0, 0xEF, 0x19, 0xBC, 0x4D, 0x5B, 0xCF, 0x11, 0xA8, 0xFD, 0x00, 0x80, 0x5F, 0x5C, 0x44, 0x2B }, new CheckOptions(offset: _offset + 24)))
                    {
                        // Found video:

                        return new FileTypeInfo("wmv", "video/x-ms-asf");
                    }

                    break;
                }
                _offset += objectSize;
            } while (_offset + 24 <= buffer.Length);

            // Default to ASF generic extension

            return new FileTypeInfo("asf", "application/vnd.ms-asf");
        }

        // MPG
        if (check(ref buffer, new byte[] { 0x0, 0x0, 0x1, 0xBA }) || check(ref buffer, new byte[] { 0x0, 0x0, 0x1, 0xB3 }))
            return new FileTypeInfo("mpg", "video/mpeg");

        // 3GP
        if (check(ref buffer, new byte[] { 0x66, 0x74, 0x79, 0x70, 0x33, 0x67 }, new CheckOptions(offset: 4)))
            return new FileTypeInfo("3gp", "video/3gpp");

        // MP*
        // Check for MPEG header at different starting offsets
        for (var start = 0; start < 2 && start < (buffer.Length - 16); start++)
        {
            if (
        check(ref buffer, new byte[] { 0x49, 0x44, 0x33 }, new CheckOptions(offset: start)) || // ID3 header
        check(ref buffer, new byte[] { 0xFF, 0xE2 }, new CheckOptions(offset: start, mask: new byte[] { 0xFF, 0xE2 })) // MPEG 1 or 2 Layer 3 header
            )
            {
                return new FileTypeInfo("mp3", "audio/mpeg");
            }

            if (check(ref buffer, new byte[] { 0xFF, 0xE4 }, new CheckOptions(offset: start, mask: new byte[] { 0xFF, 0xE4 }))) // MPEG 1 or 2 Layer 2 header
            {

                return new FileTypeInfo("mp2", "audio/mpeg");

            }

            if (
                check(ref buffer, new byte[] { 0xFF, 0xF8 }, new CheckOptions(offset: start, mask: new byte[] { 0xFF, 0xFC })) // MPEG 2 layer 0 using ADTS
                )
            {

                return new FileTypeInfo("mp2", "audio/mpeg");

            }

            if (check(ref buffer, new byte[] { 0xFF, 0xF0 }, new CheckOptions(offset: start, mask: new byte[] { 0xFF, 0xFC })) // MPEG 4 layer 0 using ADTS
                )
            {

                return new FileTypeInfo("mp4", "audio/mpeg");

            }
        }


        // M4A
        if (
            check(ref buffer, new byte[] { 0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x41 }, new CheckOptions(offset: 4)) ||
            check(ref buffer, new byte[] { 0x4D, 0x34, 0x41, 0x20 }
                ))
        { // MPEG-4 layer 3 (audio)

            return new FileTypeInfo("m4a", "audio/mp4"); // RFC 4337
        }

        // OPUS
        if (check(ref buffer, new byte[] { 0x4F, 0x70, 0x75, 0x73, 0x48, 0x65, 0x61, 0x64 }, new CheckOptions(offset: 28)))
        {

            return new FileTypeInfo("opus", "audio/opus");
        }

        // OGG
        if (check(ref buffer, new byte[] { 0x4F, 0x67, 0x67, 0x53 }))
        {
            // This is a OGG container

            // If ' theora' in header.
            if (check(ref buffer, new byte[] { 0x80, 0x74, 0x68, 0x65, 0x6F, 0x72, 0x61 }, new CheckOptions(offset: 28)))
            {

                return new FileTypeInfo("ogv", "video/ogg");
            }
            // If '\x01video' in header.
            if (check(ref buffer, new byte[] { 0x01, 0x76, 0x69, 0x64, 0x65, 0x6F, 0x00 }, new CheckOptions(offset: 28)))
            {

                return new FileTypeInfo("ogm", "video/ogg");
            }
            // If ' FLAC' in header  https://xiph.org/flac/faq.html
            if (check(ref buffer, new byte[] { 0x7F, 0x46, 0x4C, 0x41, 0x43 }, new CheckOptions(offset: 28)))
            {
                return new FileTypeInfo("oga", "audio/ogg");
            }

            // 'Speex  ' in header https://en.wikipedia.org/wiki/Speex
            if (check(ref buffer, new byte[] { 0x53, 0x70, 0x65, 0x65, 0x78, 0x20, 0x20 }, new CheckOptions(offset: 28)))
            {

                return new FileTypeInfo("spx", "audio/ogg");
            }

            // If '\x01vorbis' in header
            if (check(ref buffer, new byte[] { 0x01, 0x76, 0x6F, 0x72, 0x62, 0x69, 0x73 }, new CheckOptions(offset: 28)))
            {

                return new FileTypeInfo("ogg", "audio/ogg");
            }

            // Default OGG container https://www.iana.org/assignments/media-types/application/ogg

            return new FileTypeInfo("ogx", "application/ogg");
        }

        // FLAC
        if (check(ref buffer, new byte[] { 0x66, 0x4C, 0x61, 0x43 }))
        {

            return new FileTypeInfo("flac", "audio/x-flac");
        }

        // APE
        if (check(ref buffer, new byte[] { 0x4D, 0x41, 0x43, 0x20 }))
        { // 'MAC '

            return new FileTypeInfo("ape", "audio/ape");
        }

        //WV
        if (check(ref buffer, new byte[] { 0x77, 0x76, 0x70, 0x6B }))
        { // 'wvpk'


            return new FileTypeInfo("wv", "audio/wavpack");

        }

        // AMR

        if (check(ref buffer, new byte[] { 0x23, 0x21, 0x41, 0x4D, 0x52, 0x0A }))
        {

            return new FileTypeInfo("amr", "audio/amr");
        }

        // PDF
        if (check(ref buffer, new byte[] { 0x25, 0x50, 0x44, 0x46 }))
        {

            return new FileTypeInfo("pdf", "application/pdf");
        }

        // EXE
        if (check(ref buffer, new byte[] { 0x4D, 0x5A }))
        {

            return new FileTypeInfo("exe", "application/x-msdownload");
        }

        // SWF
        if (
            (buffer[0] == 0x43 || buffer[0] == 0x46) &&

     check(ref buffer, new byte[] { 0x57, 0x53 }, new CheckOptions(offset: 1)))
        {
            return new FileTypeInfo("swf", "application/x-shockwave-flash");
        }

        // RTF
        if (check(ref buffer, new byte[] { 0x7B, 0x5C, 0x72, 0x74, 0x66 }))
        {
            return new FileTypeInfo("rtf", "application/rtf");
        }

        // WASM
        if (check(ref buffer, new byte[] { 0x00, 0x61, 0x73, 0x6D }))
        {

            return new FileTypeInfo("wasm", "application/wasm");
        }

        // WOFF
        if (
            check(ref buffer, new byte[] { 0x77, 0x4F, 0x46, 0x46 }) &&
            (
                check(ref buffer, new byte[] { 0x00, 0x01, 0x00, 0x00 }, new CheckOptions(offset: 4)) ||

     check(ref buffer, new byte[] { 0x4F, 0x54, 0x54, 0x4F }, new CheckOptions(offset: 4))))
        {
            return new FileTypeInfo("woff", "font/woff");

        }

        // WOFF 2
        if (
            check(ref buffer, new byte[] { 0x77, 0x4F, 0x46, 0x32 }) &&
            (
                check(ref buffer, new byte[] { 0x00, 0x01, 0x00, 0x00 }, new CheckOptions(offset: 4)) ||


         check(ref buffer, new byte[] { 0x4F, 0x54, 0x54, 0x4F }, new CheckOptions(offset: 4))
    ))
        {

            return new FileTypeInfo("woff2", "font/woff2");
        }

        // EOT
        if (
            check(ref buffer, new byte[] { 0x4C, 0x50 }, new CheckOptions(offset: 34)) &&
            (
                check(ref buffer, new byte[] { 0x00, 0x00, 0x01 }, new CheckOptions(offset: 8)) ||
                check(ref buffer, new byte[] { 0x01, 0x00, 0x02 }, new CheckOptions(offset: 8)) ||
                check(ref buffer, new byte[] { 0x02, 0x00, 0x02 }, new CheckOptions(offset: 8))
        )
    )
        {

            return new FileTypeInfo("eot", "application/vnd.ms-fontobject");

        }
        // TTF
        if (check(ref buffer, new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00 }))
        {

            return new FileTypeInfo("ttf", "font/ttf");

        }
        // OTF
        if (check(ref buffer, new byte[] { 0x4F, 0x54, 0x54, 0x4F, 0x00 }))
        {

            return new FileTypeInfo("otf", "font/otf");

        }
        // ICO
        if (check(ref buffer, new byte[] { 0x00, 0x00, 0x01, 0x00 }))
        {

            return new FileTypeInfo("ico", "image/x-icon");

        }

        if (check(ref buffer, new byte[] { 0x00, 0x00, 0x02, 0x00 }))
        {
            return new FileTypeInfo("cur", "image/x-icon");
        }

        // FLV
        if (check(ref buffer, new byte[] { 0x46, 0x4C, 0x56, 0x01 }))
        {

            return new FileTypeInfo("flv", "video/x-flv");

        }

        // PS
        if (check(ref buffer, new byte[] { 0x25, 0x21 }))
        {
            return new FileTypeInfo("ps", "application/postscript");

        }

        // XZ
        if (check(ref buffer, new byte[] { 0xFD, 0x37, 0x7A, 0x58, 0x5A, 0x00 }))
        {

            return new FileTypeInfo("xz", "application/x-xz");

        }

        // SQLITE
        if (check(ref buffer, new byte[] { 0x53, 0x51, 0x4C, 0x69 }))
        {

            return new FileTypeInfo("sqlite", "application/x-sqlite3");

        }
        // NES
        if (check(ref buffer, new byte[] { 0x4E, 0x45, 0x53, 0x1A }))
        {

            return new FileTypeInfo("nes", "application/x-nintendo-nes-rom");

        }
        // CRX
        if (check(ref buffer, new byte[] { 0x43, 0x72, 0x32, 0x34 }))
        {

            return new FileTypeInfo("crx", "application/x-google-chrome-extension");

        }

        // CAB
        if (
            check(ref buffer, new byte[] { 0x4D, 0x53, 0x43, 0x46 }) ||


     check(ref buffer, new byte[] { 0x49, 0x53, 0x63, 0x28 }))
        {
            return new FileTypeInfo("cab", "application/vnd.ms-cab-compressed");

        }

        // DEB
        // Needs to be before `ar` check
        if (check(ref buffer, new byte[] { 0x21, 0x3C, 0x61, 0x72, 0x63, 0x68, 0x3E, 0x0A, 0x64, 0x65, 0x62, 0x69, 0x61, 0x6E, 0x2D, 0x62, 0x69, 0x6E, 0x61, 0x72, 0x79 }))
        {

            return new FileTypeInfo("deb", "application/x-deb");

        }

        // AR
        if (check(ref buffer, new byte[] { 0x21, 0x3C, 0x61, 0x72, 0x63, 0x68, 0x3E }))
        {

            return new FileTypeInfo("ar", "application/x-unix-archive");

        }

        // RPM
        if (check(ref buffer, new byte[] { 0xED, 0xAB, 0xEE, 0xDB }))
        {

            return new FileTypeInfo("rpm", "application/x-rpm");

        }

        // Z
        if (
            check(ref buffer, new byte[] { 0x1F, 0xA0 }) ||


     check(ref buffer, new byte[] { 0x1F, 0x9D })
                                )
        {

            return new FileTypeInfo("Z", "application/x-compress");
        }

        // LZ
        if (check(ref buffer, new byte[] { 0x4C, 0x5A, 0x49, 0x50 }))
        {

            return new FileTypeInfo("lz", "application/x-lzip");

        }

        // MSI
        if (check(ref buffer, new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 }))
        {

            return new FileTypeInfo("msi", "application/x-msi");

        }

        // MXF
        if (check(ref buffer, new byte[] { 0x06, 0x0E, 0x2B, 0x34, 0x02, 0x05, 0x01, 0x01, 0x0D, 0x01, 0x02, 0x01, 0x01, 0x02 }))
        {

            return new FileTypeInfo("mxf", "application/mxf");

        }
        // MTS
        if (check(ref buffer, new byte[] { 0x47 }, new CheckOptions(offset: 4)) &&
            (
            check(ref buffer, new byte[] { 0x47 }, new CheckOptions(offset: 192)) ||
             check(ref buffer, new byte[] { 0x47 }, new CheckOptions(offset: 196))
             ))
        {

            return new FileTypeInfo("mts", "application/mp2t");

        }

        // BLEND
        if (check(ref buffer, new byte[] { 0x42, 0x4C, 0x45, 0x4E, 0x44, 0x45, 0x52 }))
        {

            return new FileTypeInfo("blend", "application/x-blender");

        }

        // BPG
        if (check(ref buffer, new byte[] { 0x42, 0x50, 0x47, 0xFB }))
        {

            return new FileTypeInfo("bpg", "image/bpg");

        }


        if (check(ref buffer, new byte[] { 0x00, 0x00, 0x00, 0x0C, 0x6A, 0x50, 0x20, 0x20, 0x0D, 0x0A, 0x87, 0x0A }))
        {
            // JPEG-2000 family
            // JP2
            if (check(ref buffer, new byte[] { 0x6A, 0x70, 0x32, 0x20 }, new CheckOptions(offset: 20)))
            {

                return new FileTypeInfo("jp2", "image/jp2");
            }
            // JPX
            if (check(ref buffer, new byte[] { 0x6A, 0x70, 0x78, 0x20 }, new CheckOptions(offset: 20)))
            {

                return new FileTypeInfo("jpx", "image/jpx");
            }
            // JPM
            if (check(ref buffer, new byte[] { 0x6A, 0x70, 0x6D, 0x20 }, new CheckOptions(offset: 20)))
            {

                return new FileTypeInfo("jpm", "image/jpm");

            }
            // MJ2
            if (check(ref buffer, new byte[] { 0x6D, 0x6A, 0x70, 0x32 }, new CheckOptions(offset: 20)))
            {

                return new FileTypeInfo("mj2", "image/mj2");
            }
        }

        // AIFF
        if (check(ref buffer, new byte[] { 0x46, 0x4F, 0x52, 0x4D, 0x00 }))
        {

            return new FileTypeInfo("aif", "audio/aiff");

        }

        // XML
        // TODO: use regex instead?
        if (checkString(ref buffer, "<?xml "))
        {

            return new FileTypeInfo("xml", "application/xml");

        }
        // MOBI
        if (check(ref buffer, new byte[] { 0x42, 0x4F, 0x4F, 0x4B, 0x4D, 0x4F, 0x42, 0x49 }, new CheckOptions(offset: 60)))
        {

            return new FileTypeInfo("mobi", "application/x-mobipocket-ebook");
        }

        // HEIC
        // File Type Box (https://en.wikipedia.org/wiki/ISO_base_media_file_format)
        if (check(ref buffer, new byte[] { 0x66, 0x74, 0x79, 0x70 }, new CheckOptions(offset: 4)))
        {
            if (check(ref buffer, new byte[] { 0x6D, 0x69, 0x66, 0x31 }, new CheckOptions(offset: 8)))
            {

                return new FileTypeInfo("heic", "image/heif");

            }

            if (check(ref buffer, new byte[] { 0x6D, 0x73, 0x66, 0x31 }, new CheckOptions(offset: 8)))
            {

                return new FileTypeInfo("heic", "image/heif-sequence");

            }

            if (
            check(ref buffer, new byte[] { 0x68, 0x65, 0x69, 0x63 }, new CheckOptions(offset: 8)) ||
             check(ref buffer, new byte[] { 0x68, 0x65, 0x69, 0x78 }, new CheckOptions(offset: 8))
             )
            {

                return new FileTypeInfo("heic", "image/heic");

            }

            if (check(ref buffer, new byte[] { 0x68, 0x65, 0x76, 0x63 }, new CheckOptions(offset: 8)) ||
         check(ref buffer, new byte[] { 0x68, 0x65, 0x76, 0x78 }, new CheckOptions(offset: 8)))
            {

                return new FileTypeInfo("heic", "image/heic-sequence");

            }
        }

        // KTX
        if (check(ref buffer, new byte[] { 0xAB, 0x4B, 0x54, 0x58, 0x20, 0x31, 0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A }))
        {

            return new FileTypeInfo("ktx", "image/ktx");

        }

        return null;


    }

  

}

