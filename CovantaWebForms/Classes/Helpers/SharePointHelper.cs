using Microsoft.SharePoint.Client;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace CovantaWebForms.Classes.Helpers
{
    public static class SharePointHelper
    {
        /// <summary>
        /// Process PDF and upload to SharePoint.
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="company"></param>
        /// <param name="supplierName"></param>
        /// <param name="fileAttachments"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static string UploadFileToSharePoint(string supplierId, string company, string supplierName, FileUpload fileAttachments, Category category, string supplierDataFilePath)
        {
            FileInfo fileInfo = new FileInfo(fileAttachments.FileName);
            string filePath = string.Empty;
            company = Regex.Replace(company, "[^a-zA-Z0-9 ]+", "", RegexOptions.Compiled).Trim();

            filePath = string.Format("{0}{1}-{2} (NDA)_Original.pdf", Path.GetTempPath(), supplierId, company); // ConfigurationManager.AppSettings["AttachmentsUploadDirectory"]
            string finalNdaFile = string.Format("{0}{1}-{2} (NDA).pdf", Path.GetTempPath(), supplierId, company);

            LogHelper.LogInfo(Path.GetTempPath());
            try
            {
                if (FileHelper.IsImageFile(fileInfo))
                {
                    PDFHelper.ConvertImageToPdf(fileAttachments, filePath);
                }
                else if (FileHelper.IsWordDocumet(fileInfo))
                {
                    PDFHelper.ConvertWordToPdf(fileAttachments, filePath);
                }
                else
                {
                    fileAttachments.SaveAs(filePath); // Save uploaded PDF to a local directory.
                }

                //Remove external links in uploaded PDF file.
                RemovePdfAnnotations(filePath);
                PDFHelper.RemovePdfAnnotations(filePath);

                List<string> fileNames = new List<string>();
                fileNames.Add(supplierDataFilePath);
                fileNames.Add(filePath);
                PDFHelper.MergePDFs(fileNames, finalNdaFile);

                //Create a folder in SharePoint and upload file to that folder. 
                string sharePointUrl = ConfigurationManager.AppSettings["SharePointUrl"];
                using (ClientContext context = new ClientContext(sharePointUrl))
                {
                    //Provide SharePoint Credentials.
                    string passWd = ConfigurationManager.AppSettings["SharePointPassword"];
                    SecureString securePassWd = new SecureString();
                    foreach (var c in passWd.ToCharArray())
                    {
                        securePassWd.AppendChar(c);
                    }
                    context.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings["SharePointUserName"], securePassWd);

                    //New folder name format <SupplierID-SupplierName>.
                    string newFolderName = string.Format("{0}-{1}", supplierId, company);

                    //Location in SharePoint where new folder must be created.
                    string newFolderLocation = string.Format("{0}{1}/", sharePointUrl, ConfigurationManager.AppSettings["SharePointFolderName"]);
                    LogHelper.LogInfo(newFolderLocation);
                    //Create folder in SharePoint.
                    bool folderCreated = CreateFolder(context, newFolderLocation, ConfigurationManager.AppSettings["SharePointFolderName"], category.SharePointFolderName, newFolderName);

                    //Upload file to SharePoint only if folder is created/Exists.
                    if (folderCreated)
                    {
                        //SharePoint folder location where file must be uploaded.
                        string uploadFolderUrl = string.Format("{0}{1}/{2}/", newFolderLocation, category.SharePointFolderName, newFolderName);

                        //Upload file to SharePoint.
                        //UploadFile(context, uploadFolderUrl, finalNdaFile);

                        //Upload file to SharePoint using binary direct
                        UploadFileUsingSaveBinaryDirect(context, category.SharePointFolderName, newFolderName, finalNdaFile);

                        //Delete the file form local directory.
                        System.IO.File.Delete(finalNdaFile);
                        System.IO.File.Delete(filePath);
                        System.IO.File.Delete(supplierDataFilePath);

                        //Return SharePoint folder location where the file was uploaded.
                        return uploadFolderUrl;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);

                // Delete the file on exception
                System.IO.File.Delete(finalNdaFile);
                System.IO.File.Delete(filePath);
                System.IO.File.Delete(supplierDataFilePath);

                throw new Exception(ex.Message);
            }

            //Return empty string if there was an exception.
            return string.Empty;
        }

        /// <summary>
        /// Remove Annotations in PDF.
        /// </summary>
        /// <param name="filePath"></param>
        public static void RemovePdfAnnotations(string filePath)
        {
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(filePath);
            for (int i = 0; i < loadedDocument.Pages.Count; i++)
            {
                //Get the loaded page.
                PdfLoadedPage page = loadedDocument.Pages[i] as PdfLoadedPage;
                for (int j = page.Annotations.Count - 1; j >= 0; j--)
                {
                    if (page.Annotations[j] is PdfLoadedUriAnnotation || page.Annotations[j] is PdfLoadedTextWebLinkAnnotation)
                    {
                        //Removes the annotation.
                        page.Annotations.RemoveAt(j);
                    }
                }
            }

            //Save the document.
            loadedDocument.Save(filePath);
            //Close the document.
            loadedDocument.Close(true);

            //This will open the PDF file so, the result will be seen in default PDF viewer
            //Process.Start("Hyperlink.pdf");
        }

        /// <summary>
        /// Create a Folder in SharePoint if it does not exist.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="newFolderLocation"></param>
        /// <param name="listName"></param>
        /// <param name="relativePath"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        private static bool CreateFolder(ClientContext context, string newFolderLocation, string listName, string relativePath, string newFolderName)
        {
            bool folderCreated = false;
            try
            {
                Web web = context.Web;

                bool isDevEnv = bool.Parse(ConfigurationManager.AppSettings["IsDevEnvironment"]);
                if (isDevEnv)
                {
                    listName = ConfigurationManager.AppSettings["DevSharePointFolderName"];
                }

                List list = web.Lists.GetByTitle(listName);

                ListItemCreationInformation newItem = new ListItemCreationInformation();
                newItem.UnderlyingObjectType = FileSystemObjectType.Folder;

                newItem.FolderUrl = newFolderLocation;
                if (!relativePath.Equals(string.Empty))
                {
                    newItem.FolderUrl += relativePath + "/";
                }

                LogHelper.LogInfo(newItem.FolderUrl + "," + newFolderName);
                //Check if Folder exists in SharePoint.
                bool folderExists = CheckIfFolderExists(context, list, newItem.FolderUrl, newFolderName);

                //Create the folder in SharePoint if does not exist.
                if (!folderExists)
                {
                    LogHelper.LogInfo("folder exists:" + folderExists);
                    newItem.LeafName = newFolderName;
                    Microsoft.SharePoint.Client.ListItem item = list.AddItem(newItem);
                    item["Author"] = 9;
                    item["Editor"] = 9;
                    item.Update();
                    context.ExecuteQuery();
                }
                folderCreated = true;
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);
                throw new Exception(ex.Message);
            }
            return folderCreated;
        }

        /// <summary>
        /// Upload a file to SharePoint.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="uploadFolderUrl"></param>
        /// <param name="uploadFilePath"></param>
        private static void UploadFile(ClientContext context, string uploadFolderUrl, string uploadFilePath)
        {
            var fileCreationInfo = new FileCreationInformation
            {
                Content = System.IO.File.ReadAllBytes(uploadFilePath),
                Overwrite = true,
                Url = Path.GetFileName(uploadFilePath)
            };
            var targetFolder = context.Web.GetFolderByServerRelativeUrl(uploadFolderUrl);
            var uploadFile = targetFolder.Files.Add(fileCreationInfo);
            context.Load(uploadFile);
            context.ExecuteQuery();
        }

        /// <summary>
        /// Upload a file to SharePoint.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="uploadFolderUrl"></param>
        /// <param name="uploadFilePath"></param>
        private static void UploadFileUsingSaveBinaryDirect(ClientContext context, string categoryFolderName, string newFolderName, string uploadFilePath)
        {
            using (var fs = new FileStream(uploadFilePath, FileMode.Open))
            {
                var fi = new FileInfo(Path.GetFileName(uploadFilePath));
                var title = ConfigurationManager.AppSettings["SharePointFolderName"];
                var list = context.Web.Lists.GetByTitle(title);
                context.Load(list.RootFolder);
                context.ExecuteQuery();
                var fileUrl = String.Format("{0}/{1}/{2}/{3}", list.RootFolder.ServerRelativeUrl, categoryFolderName, newFolderName, fi.Name);

                if (context.HasPendingRequest)
                    context.ExecuteQuery();
                Microsoft.SharePoint.Client.File.SaveBinaryDirect(context, fileUrl, fs, true);
            }
        }

        /// <summary>
        /// Check if folder exists in SharePoint.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="list"></param>
        /// <param name="targetFolderUrl"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        private static bool CheckIfFolderExists(ClientContext context, List list, string targetFolderUrl, string folderName)
        {
            LogHelper.LogInfo(targetFolderUrl);
            var targetFolder = context.Web.GetFolderByServerRelativeUrl(targetFolderUrl + folderName);
            context.Load(targetFolder);
            try
            {
                context.ExecuteQuery();
                return true;
            }
            catch (ServerUnauthorizedAccessException aex)
            {
                LogHelper.LogInfo(aex.Message);
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("Could not find file:" + targetFolderUrl + folderName);
                return false;
            }

            //FolderCollection folders = targetFolder.Folders;
            //context.Load(folders);
            //context.ExecuteQuery();

            //return folders.Any(x => x.Name == folderName);
        }

        /// <summary>
        /// Create a list item in SharePoint.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="fullName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="company"></param>
        /// <param name="street"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="covantaContact"></param>
        /// <param name="category"></param>
        /// <param name="subCategories"></param>
        /// <param name="region"></param>
        /// <param name="supplierDiversity"></param>
        /// <param name="signedNdaPath"></param>
        public static void CreateListItem(
            string applicationId,
            string fullName,
            string phone,
            string email,
            string company,
            string street,
            string city,
            string state,
            string covantaContact,
            string category,
            List<string> subCategories,
            string region,
            string supplierDiversity,
            string signedNdaPath,
            string zipcode,
            string country,
            string manager)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(signedNdaPath))
                {
                    string sharePointUrl = ConfigurationManager.AppSettings["SharePointListUrl"];
                    using (ClientContext context = new ClientContext(sharePointUrl))
                    {
                        //Provide SharePoint Credentials.
                        string passWd = ConfigurationManager.AppSettings["SharePointPassword"];
                        SecureString securePassWd = new SecureString();
                        foreach (var c in passWd.ToCharArray())
                        {
                            securePassWd.AppendChar(c);
                        }
                        context.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings["SharePointUserName"], securePassWd);
                        List myList = context.Web.Lists.GetByTitle(ConfigurationManager.AppSettings["SharePointListName"]);
                        ListItemCreationInformation itemInfo = new ListItemCreationInformation();
                        Microsoft.SharePoint.Client.ListItem myItem = myList.AddItem(itemInfo);

                        var emailRecipients = manager.Split(',');
                        List<FieldUserValue> fieldUserValues = new List<FieldUserValue>();
                        foreach (var emailRecipient in emailRecipients)
                        {
                            var result = Microsoft.SharePoint.Client.Utilities.Utility.ResolvePrincipal(context, context.Web, emailRecipient, Microsoft.SharePoint.Client.Utilities.PrincipalType.User, Microsoft.SharePoint.Client.Utilities.PrincipalSource.All, null, true);
                            context.ExecuteQuery();
                            if (result != null)
                            {
                                var user = context.Web.EnsureUser(result.Value.LoginName);
                                context.Load(user);
                                context.ExecuteQuery();

                                FieldUserValue userValue = new FieldUserValue();
                                userValue.LookupId = user.Id;
                                fieldUserValues.Add(userValue);
                            }
                        }

                        if (fieldUserValues.Any())
                        {
                            myItem[ConfigurationManager.AppSettings["SLCN_Manager"]] = fieldUserValues.ToArray();
                        }

                        myItem[ConfigurationManager.AppSettings["SLCN_ApplicationID"]] = applicationId;
                        myItem[ConfigurationManager.AppSettings["SLCN_FullName"]] = fullName;
                        myItem[ConfigurationManager.AppSettings["SLCN_PhoneNumber"]] = phone;
                        myItem[ConfigurationManager.AppSettings["SLCN_Email"]] = email;
                        myItem[ConfigurationManager.AppSettings["SLCN_Company"]] = company;
                        myItem[ConfigurationManager.AppSettings["SLCN_Street"]] = street;
                        myItem[ConfigurationManager.AppSettings["SLCN_City"]] = city;
                        myItem[ConfigurationManager.AppSettings["SLCN_State"]] = state;
                        myItem[ConfigurationManager.AppSettings["SLCN_CovantaContact"]] = covantaContact;
                        myItem[ConfigurationManager.AppSettings["SLCN_Category"]] = category;
                        myItem[ConfigurationManager.AppSettings["SLCN_SubCategory"]] = string.Join(",", subCategories);
                        myItem[ConfigurationManager.AppSettings["SLCN_Region"]] = region;
                        myItem[ConfigurationManager.AppSettings["SLCN_SupplierDiversity"]] = supplierDiversity;
                        myItem[ConfigurationManager.AppSettings["SLCN_SignedNDA"]] = signedNdaPath;
                        myItem[ConfigurationManager.AppSettings["SLCN_ZipCode"]] = zipcode;
                        myItem[ConfigurationManager.AppSettings["SLCN_Country"]] = country;

                        myItem.Update();
                        context.ExecuteQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);
                throw new Exception(ex.Message);
            }
        }
    }
}