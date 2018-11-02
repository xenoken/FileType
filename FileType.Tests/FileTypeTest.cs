/*
 * FileTypeTest.cs
 *
 * Originally written by Ken T Ekeoha.
 * see License.md for license information.
 * 
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileTypeTests
{
    [TestClass]
    public class FileTypeTest
    {


        private FileType.FileTypeInfo test_internal(string filepath)
        {

            if (!System.IO.File.Exists(filepath))
            {
                Assert.Fail($"file at <{filepath}> not found.");
            }

            // to avoid reading an entire file, try reading a fixed-length chunk from the start.
            // if any problem arise during the reading, try reading the whole thing.
            //var file_data = new byte[FileHeaderLength];
            //try
            //{
            //    using (var file = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(filepath))
            //    {
            //        var stream = file.CreateViewStream(0, FileHeaderLength);
            //        stream.Read(file_data, 0, FileHeaderLength);
            //        stream.Close();
            //    };
            //}
            //catch {
            // use simple byte array
            byte[] file_data = System.IO.File.ReadAllBytes(filepath);


            // }

            var result = FileType.Get(ref file_data);

            if (result == null)
            {
                Assert.Fail("Unknown Type.");
            }

            return result;



        }

        [TestMethod]
        public void Get_JPG()
        {
            // ARRANGE
            var ext = "jpg";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/" + ext);

        }

        [TestMethod]
        public void Get_PNG()
        {
            // ARRANGE
            var ext = "png";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/" + ext);
        }

        [TestMethod]
        public void Get_WEBP()
        {
            // ARRANGE
            var ext = "webp";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/" + ext);
        }

        [TestMethod]
        public void Get_FLIF()
        {
            // ARRANGE
            var ext = "flif";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/" + ext);
        }

        [TestMethod]
        public void Get_CR2()
        {
            // ARRANGE
            var ext = "cr2";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/x-canon-cr2");
        }

        [TestMethod]
        public void Get_TIFF()
        {
            // BIG ENDIAN

            // ARRANGE
            var ext = "tif";
            var filepath = "..\\..\\fixture\\fixture-big-endian." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/tiff", "TIFF - Big Endian");



            // LITTLE ENDIAN

            // ARRANGE
            ext = "tif";
            filepath = "..\\..\\fixture\\fixture-little-endian." + ext;
            // ACT
            result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/tiff", "TIFF - Little Endian");
        }

        [TestMethod]
        public void Get_BMP()
        {
            // ARRANGE
            var ext = "bmp";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/" + ext);
        }

        [TestMethod]
        public void Get_JXR()
        {
            // ARRANGE
            var ext = "jxr";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/vnd.ms-photo");
        }

        [TestMethod]
        public void Get_PSD()
        {
            // ARRANGE
            var ext = "psd";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/vnd.adobe.photoshop");
        }

        [TestMethod]
        public void Get_EPUB()
        {
            // ARRANGE
            var ext = "epub";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/epub+zip");
        }

        [TestMethod]
        public void Get_XPI()
        {
            // ARRANGE
            var ext = "xpi";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-xpinstall");
        }

        [TestMethod]
        public void Get_ODT()
        {
            // ARRANGE
            var ext = "odt";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/vnd.oasis.opendocument.text");
        }

        [TestMethod]
        public void Get_ODS()
        {
            // ARRANGE
            var ext = "ods";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/vnd.oasis.opendocument.spreadsheet");
        }

        [TestMethod]
        public void Get_ODP()
        {
            // ARRANGE
            var ext = "odp";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/vnd.oasis.opendocument.presentation");
        }

        [TestMethod]
        public void Get_DOCX()
        {
            // ARRANGE
            var ext = "docx";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

        [TestMethod]
        public void Get_PPTX()
        {
            // ARRANGE
            var ext = "pptx";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/vnd.openxmlformats-officedocument.presentationml.presentation");
        }

        [TestMethod]
        public void Get_XLSX()
        {
            // ARRANGE
            var ext = "xlsx";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }


        [TestMethod]
        public void Get_ZIP()
        {
            // ARRANGE
            var ext = "zip";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/" + ext);
        }

        [TestMethod]
        public void Get_TAR()
        {
            // ARRANGE
            var ext = "tar";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-" + ext);
        }

        [TestMethod]
        public void Get_RAR()
        {
            // ARRANGE
            var ext = "rar";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-" + ext + "-compressed");
        }

        [TestMethod]
        public void Get_GZ()
        {
            // ARRANGE
            var ext = "gz";
            var filepath = "..\\..\\fixture\\fixture.tar." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/gzip");
        }

        [TestMethod]
        public void Get_BZ2()
        {
            // ARRANGE
            var ext = "bz2";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-bzip2");
        }

        [TestMethod]
        public void Get_7Z()
        {
            // ARRANGE
            var ext = "7z";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-" + ext + "-compressed");
        }

        [TestMethod]
        public void Get_DMG()
        {
            // ARRANGE
            var ext = "dmg";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-apple-diskimage");
        }

        [TestMethod]
        public void Get_MP4()
        {
            // ARRANGE
            var ext = "mp4";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/" + ext);


            // ARRANGE
            ext = "mp4";
            filepath = "..\\..\\fixture\\fixture-aac-adts." + ext;
            // ACT
            result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/mpeg");
        }

        [TestMethod]
        public void Get_MIDI()
        {
            // ARRANGE
            var ext = "mid";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/midi");
        }

        [TestMethod]
        public void Get_MKV()
        {
            // ARRANGE
            var ext = "mkv";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/x-matroska");
        }

        [TestMethod]
        public void Get_WEBM()
        {
            // ARRANGE
            var ext = "webm";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/" + ext);
        }

        [TestMethod]
        public void Get_MOV()
        {
            // ARRANGE
            var ext = "mov";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/quicktime");


            // ARRANGE
            ext = "mov";
            filepath = "..\\..\\fixture\\fixture-mjpeg." + ext;
            // ACT
            result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/quicktime", "MOV - MJPEG");


            // ARRANGE
            ext = "mov";
            filepath = "..\\..\\fixture\\fixture-moov." + ext;
            // ACT
            result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/quicktime", "MOV - Moov");
        }

        [TestMethod]
        public void Get_AVI()
        {
            // ARRANGE
            var ext = "avi";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/vnd." + ext);
        }

        [TestMethod]
        public void Get_WAV()
        {
            // ARRANGE
            var ext = "wav";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/vnd.wave");
        }


        [TestMethod]
        public void Get_QCP()
        {
            // ARRANGE
            var ext = "qcp";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/qcpelp");
        }

        [TestMethod]
        public void Get_WMA()
        {
            // ARRANGE
            var ext = "wma";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/x-ms-wma");
        }

        [TestMethod]
        public void Get_WMV()
        {
            // ARRANGE
            var ext = "wmv";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/x-ms-asf");
        }

        // TODO: Test: ASF
        //[TestMethod]
        //public void Get_ASF()
        //{

        //    // ARRANGE
        //    var ext = "asf";
        //    var filepath = "..\\..\\fixture\\fixture." + ext;
        //    // ACT
        //    var result = test_internal(filepath);
        //    // ASSERT
        //    Assert.IsTrue(result.Extension == ext && result.MimeType == "application/vnd.ms-asf");
        //}

        [TestMethod]
        public void Get_MPG()
        {
            // ARRANGE
            var ext = "mpg";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/mpeg");


            // ARRANGE
            ext = "mpg";
            filepath = "..\\..\\fixture\\fixture2." + ext;
            // ACT
            result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/mpeg");
        }

        [TestMethod]
        public void Get_3GP()
        {
            // ARRANGE
            var ext = "3gp";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/3gpp");
        }

        [TestMethod]
        public void Get_MP3()
        {
            // ARRANGE
            var ext = "mp3";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/mpeg");
        }


        [TestMethod]
        public void Get_MP2()
        {
            // ARRANGE
            var ext = "mp2";
            var filepath = "..\\..\\fixture\\fixture-mpa." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/mpeg");


            // ARRANGE
            ext = "mp2";
            filepath = "..\\..\\fixture\\fixture-mpa." + ext;
            // ACT
            result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/mpeg");


            // ARRANGE
            ext = "mp2";
            filepath = "..\\..\\fixture\\fixture-faac-adts." + ext;
            // ACT
            result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/mpeg");
        }




        [TestMethod]
        public void Get_M4A()
        {
            // ARRANGE
            var ext = "m4a";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/mp4");
        }


        [TestMethod]
        public void Get_OPUS()
        {
            // ARRANGE
            var ext = "opus";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/opus");
        }


        [TestMethod]
        public void Get_OGV()
        {
            // ARRANGE
            var ext = "ogv";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/ogg");
        }

        // TODO: Test: OGM
        //[TestMethod]
        //public void Get_OGM()
        //{
        //    // ARRANGE
        //    var ext = "ogm";
        //    var filepath = "..\\..\\fixture\\fixture." + ext;
        //    // ACT
        //    var result = test_internal(filepath);
        //    // ASSERT
        //    Assert.IsTrue(result.Extension == ext && result.MimeType == "video/ogg");
        //}

        [TestMethod]
        public void Get_OGA()
        {
            // ARRANGE
            var ext = "oga";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/ogg");
        }


        [TestMethod]
        public void Get_SPX()
        {
            // ARRANGE
            var ext = "spx";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/ogg");
        }

        [TestMethod]
        public void Get_OGG()
        {
            // ARRANGE
            var ext = "ogg";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/ogg");
        }

        // TODO: Test: OGX
        //[TestMethod]
        //public void Get_OGX()
        //{
        //    // ARRANGE
        //    var ext = "ogx";
        //    var filepath = "..\\..\\fixture\\fixture." + ext;
        //    // ACT
        //    var result = test_internal(filepath);
        //    // ASSERT
        //    Assert.IsTrue(result.Extension == ext && result.MimeType == "application/ogg");
        //}


        [TestMethod]
        public void Get_FLAC()
        {
            // ARRANGE
            var ext = "flac";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/x-" + ext);
        }


        [TestMethod]
        public void Get_APE()
        {
            // ARRANGE
            var ext = "ape";
            var filepath = "..\\..\\fixture\\fixture-monkeysaudio." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/" + ext);
        }


        [TestMethod]
        public void Get_WAVPACK()
        {
            // ARRANGE
            var ext = "wv";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/wavpack");
        }


        [TestMethod]
        public void Get_AMR()
        {
            // ARRANGE
            var ext = "amr";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/" + ext);
        }


        [TestMethod]
        public void Get_PDF()
        {
            // ARRANGE
            var ext = "pdf";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/" + ext);
        }


        [TestMethod]
        public void Get_EXE()
        {
            // ARRANGE
            var ext = "exe";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-msdownload");
        }


        [TestMethod]
        public void Get_SWF()
        {
            // ARRANGE
            var ext = "swf";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-shockwave-flash");
        }


        [TestMethod]
        public void Get_RTF()
        {
            // ARRANGE
            var ext = "rtf";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/" + ext);
        }


        [TestMethod]
        public void Get_WASM()
        {
            // ARRANGE
            var ext = "wasm";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/" + ext);
        }


        [TestMethod]
        public void Get_WOFF()
        {
            // ARRANGE
            var ext = "woff";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "font/" + ext);


            // ARRANGE
            ext = "woff";
            filepath = "..\\..\\fixture\\fixture-otto." + ext;
            // ACT
            result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "font/" + ext);
        }


        [TestMethod]
        public void Get_WOFF2()
        {
            // ARRANGE
            var ext = "woff2";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "font/" + ext);


            // ARRANGE
            ext = "woff2";
            filepath = "..\\..\\fixture\\fixture-otto." + ext;
            // ACT
            result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "font/" + ext);
        }


        [TestMethod]
        public void Get_EOT()
        {
            // ARRANGE
            var ext = "eot";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/vnd.ms-fontobject");
        }


        [TestMethod]
        public void Get_OTF()
        {
            // ARRANGE
            var ext = "otf";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "font/" + ext);
        }


        [TestMethod]
        public void Get_ICO()
        {
            // ARRANGE
            var ext = "ico";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/x-icon");
        }

        [TestMethod]
        public void Get_CUR()
        {
            // ARRANGE
            var ext = "cur";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/x-icon");
        }


        [TestMethod]
        public void Get_FLV()
        {
            // ARRANGE
            var ext = "flv";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "video/x-" + ext);
        }


        [TestMethod]
        public void Get_PS()
        {
            // ARRANGE
            var ext = "ps";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/postscript");
        }


        [TestMethod]
        public void Get_XZ()
        {
            // ARRANGE
            var ext = "xz";
            var filepath = "..\\..\\fixture\\fixture.tar." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-xz");
        }

        [TestMethod]
        public void Get_SQLITE()
        {
            // ARRANGE
            var ext = "sqlite";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-sqlite3");
        }


        [TestMethod]
        public void Get_NES()
        {
            // ARRANGE
            var ext = "nes";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-nintendo-nes-rom");
        }


        [TestMethod]
        public void Get_CRX()
        {
            // ARRANGE
            var ext = "crx";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-google-chrome-extension");
        }


        [TestMethod]
        public void Get_CAB()
        {
            // ARRANGE
            var ext = "cab";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/vnd.ms-cab-compressed");
        }


        [TestMethod]
        public void Get_DEB()
        {
            // ARRANGE
            var ext = "deb";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-" + ext);
        }


        [TestMethod]
        public void Get_UNIX_ARCHIVE()
        {
            // ARRANGE
            var ext = "ar";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-unix-archive");
        }

        [TestMethod]
        public void Get_RPM()
        {
            // ARRANGE
            var ext = "rpm";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-rpm");
        }

        [TestMethod]
        public void Get_Z()
        {
            // ARRANGE
            var ext = "Z";
            var filepath = "..\\..\\fixture\\fixture.tar." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-compress");
        }

        // TODO: Test: LZ
        //[TestMethod]
        //public void Get_LZ()
        //{
        //    // ARRANGE
        //    var ext = "lz";
        //    var filepath = "..\\..\\fixture\\fixture.lz" + ext;
        //    // ACT
        //    var result = test_internal(filepath);
        //    // ASSERT
        //    Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-lzip");
        //}


        [TestMethod]
        public void Get_MSI()
        {
            // ARRANGE
            var ext = "msi";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-" + ext);
        }


        [TestMethod]
        public void Get_MXF()
        {
            // ARRANGE
            var ext = "mxf";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/" + ext);
        }


        [TestMethod]
        public void Get_MTS()
        {
            // ARRANGE
            var ext = "mts";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/mp2t");
        }


        [TestMethod]
        public void Get_BLEND()
        {
            // ARRANGE
            var ext = "blend";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-blender");
        }


        [TestMethod]
        public void Get_BPG()
        {
            // ARRANGE
            var ext = "bpg";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/" + ext);
        }


        [TestMethod]
        public void Get_JP2()
        {
            // ARRANGE
            var ext = "jp2";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/" + ext);
        }


        [TestMethod]
        public void Get_JPX()
        {
            // ARRANGE
            var ext = "jpx";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/" + ext);
        }


        [TestMethod]
        public void Get_JPM()
        {
            // ARRANGE
            var ext = "jpm";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/" + ext);
        }


        [TestMethod]
        public void Get_MJ2()
        {
            // ARRANGE
            var ext = "mj2";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/" + ext);
        }

        [TestMethod]
        public void Get_AIFF()
        {
            // ARRANGE
            var ext = "aif";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "audio/aiff");
        }

        [TestMethod]
        public void Get_XML()
        {
            // ARRANGE
            var ext = "xml";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/" + ext);
        }


        [TestMethod]
        public void Get_MOBI()
        {
            // ARRANGE
            var ext = "mobi";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "application/x-mobipocket-ebook");
        }



        [TestMethod]
        public void Get_HEIC()
        {
            // ARRANGE
            var ext = "heic";
            var filepath = "..\\..\\fixture\\fixture-heic." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/heic", "HEIC");



            // ARRANGE
            ext = "heic";
            filepath = "..\\..\\fixture\\fixture-mif1." + ext;
            // ACT
            result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/heif", "HEIC - Heif");



            // ARRANGE
            ext = "heic";
            filepath = "..\\..\\fixture\\fixture-msf1." + ext;
            // ACT
            result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/heif-sequence", "HEIC - Heif Sequence");
        }




        [TestMethod]
        public void Get_KTX()
        {
            // ARRANGE
            var ext = "ktx";
            var filepath = "..\\..\\fixture\\fixture." + ext;
            // ACT
            var result = test_internal(filepath);
            // ASSERT
            Assert.IsTrue(result.Extension == ext && result.MimeType == "image/" + ext);
        }

    }
}
