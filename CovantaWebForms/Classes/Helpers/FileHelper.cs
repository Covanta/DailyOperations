
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI.WebControls;

namespace CovantaWebForms.Classes.Helpers
{
    public static class FileHelper
    {
        public static bool IsImageFile(FileInfo fileInfo)
        {
            List<string> imageExtensions = new List<string> { ".JPG", ".JPE", ".PNG" };
            if (imageExtensions.Contains(Path.GetExtension(fileInfo.Extension).ToUpperInvariant()))
            {
                return true;
            }

            return false;
        }

        public static bool IsWordDocumet(FileInfo fileInfo)
        {
            List<string> wordExtensions = new List<string> { ".DOC", ".DOCX" };
            if (wordExtensions.Contains(Path.GetExtension(fileInfo.Extension).ToUpperInvariant()))
            {
                return true;
            }

            return false;
        }

        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
                                                                System.UInt32 pBC,
                                                                [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
                                                                [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
                                                                System.UInt32 cbSize, [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
                                                                System.UInt32 dwMimeFlags,
                                                                out System.UInt32 ppwzMimeOut,
                                                                System.UInt32 dwReserverd
                                                            );

        public static bool IsFileMimeTypeValid(FileUpload uploadedFile)
        {
            bool isValid = false;

            HttpPostedFile file = uploadedFile.PostedFile;
            byte[] document = new byte[file.ContentLength];
            file.InputStream.Read(document, 0, file.ContentLength);
            System.UInt32 mimetype;
            FindMimeFromData(0, null, document, 256, null, 0, out mimetype, 0);
            System.IntPtr mimeTypePtr = new IntPtr(mimetype);
            string mime = Marshal.PtrToStringUni(mimeTypePtr);
            Marshal.FreeCoTaskMem(mimeTypePtr);

            if (mime == "application/pdf" || mime== "image/jpeg" || mime == "image/pjpeg" || mime == "image/png")
            {
                isValid = true;
            }

            return isValid;
        }
    }
}