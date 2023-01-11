using CovantaWebForms.Classes;
using CovantaWebForms.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace CovantaWebForms
{
    public partial class SupplierForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Load ReCaptcha if it is enabled in web.config.
                bool recaptchaValidationEnabled = bool.Parse(ConfigurationManager.AppSettings["EnableRecaptchaValidation"]);
                if (recaptchaValidationEnabled)
                {
                    divRecaptcha.Visible = true;
                }

                //Load Categoried DropDown.
                BindCategoryDropDown();

                //Load Region DropDown.
                BindRegionDropDown();

                //Load Supplier Diversity DropDown.
                PopulateSupplierDiversityDropDown();
            }
        }

        /// <summary>
        /// Submit event for Supplier Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int categoryId = int.Parse(ddlCategories.SelectedValue);
            if (RecaptchaHelper.IsReCaptchValid(Request) && categoryId > 0)
            {
                if (fileAttachments.HasFile)
                {
                    if (FileHelper.IsFileMimeTypeValid(fileAttachments))
                    {
                        bool hasException = false;
                        try
                        {
                            //Retrieve Category object based on selected category by User.
                            Category category = DataHelper.GetCategories().FirstOrDefault(x => x.CategoryID == categoryId);

                            //Retrieve list of Sub Categories selcted by User from CheckBx List.
                            List<string> subCategories = cblSubCatecories.Items.Cast<ListItem>()
                                                                    .Where(li => li.Selected)
                                                                    .Select(li => li.Value)
                                                                    .ToList();

                            //Get region name selected by User.
                            string region = ddlRegion.SelectedValue;
                            if (region == "Select Region")
                            {
                                region = string.Empty;
                            }

                            if (category != null)
                            {
                                //Insert a Record in database with details provided by supplier.
                                string supplierId = DataHelper.InsertForm(
                                    txtFullName.Text,
                                    txtPhone.Text,
                                    txtEmail.Text,
                                    txtCompany.Text,
                                    txtStreet.Text,
                                    txtCity.Text,
                                    txtState.Text,
                                    txtZipCode.Text,
                                    txtCountry.Text,
                                    txtCovantaContact.Text,
                                    category.CategoryName,
                                    subCategories,
                                    region,
                                    ddlSupplierDiversity.SelectedValue == "Yes");

                                if (int.Parse(supplierId) > 0)
                                {
                                    //Upload attached file to SharePoint if record is inserted in database.
                                    string supplierDataFilePath = PDFHelper.CreateSupplierDataPdf(
                                        supplierId,
                                        txtFullName.Text,
                                        txtPhone.Text,
                                        txtEmail.Text,
                                        txtCompany.Text,
                                        txtStreet.Text,
                                        txtCity.Text,
                                        txtState.Text,
                                        txtZipCode.Text,
                                        txtCountry.Text,
                                        txtCovantaContact.Text,
                                        category.CategoryName,
                                        subCategories,
                                        region,
                                        ddlSupplierDiversity.SelectedValue
                                        );
                                    string fileUrl = SharePointHelper.UploadFileToSharePoint(
                                        supplierId,
                                        txtCompany.Text,
                                        txtFullName.Text,
                                        fileAttachments,
                                        category,
                                        supplierDataFilePath);

                                    if (!string.IsNullOrWhiteSpace(fileUrl))
                                    {
                                        SharePointHelper.CreateListItem(
                                        supplierId,
                                        txtFullName.Text,
                                        txtPhone.Text,
                                        txtEmail.Text,
                                        txtCompany.Text,
                                        txtStreet.Text,
                                        txtCity.Text,
                                        txtState.Text,
                                        txtCovantaContact.Text,
                                        category.CategoryName,
                                        subCategories,
                                        region,
                                        ddlSupplierDiversity.SelectedValue,
                                        fileUrl,
                                        txtZipCode.Text,
                                        txtCountry.Text,
                                        category.EmailRecipients);

                                        //Update file path in Supplier table after file is uploaded to SharePoint.
                                        //DataHelper.UpdateFilePathInSupplierForm(supplierId, fileUrl);

                                        //FileInfo fileInfo = new FileInfo(fileAttachments.FileName);
                                        //string filePath = string.Format("{0}{1}-{2} (NDA){3}", Path.GetTempPath(), supplierId, txtCompany.Text, fileInfo.Extension);

                                        //fileAttachments.SaveAs(filePath); // Save uploaded PDF to a local directory.

                                        try
                                        {
                                            //Remove external links in uploaded PDF file.
                                            //SharePointHelper.RemovePdfAnnotations(filePath);

                                            //Generate HTML for email message to be sent to Covanta team.
                                            //string emailMessage = EmailHelper.GetSupplierFormHtml(
                                            //supplierId,
                                            //txtFullName.Text,
                                            //txtPhone.Text,
                                            //txtEmail.Text,
                                            //txtCompany.Text,
                                            //txtStreet.Text,
                                            //txtCity.Text,
                                            //txtState.Text,
                                            //txtZipCode.Text,
                                            //txtCountry.Text,
                                            //txtCovantaContact.Text,
                                            //category.CategoryName,
                                            //subCategories,
                                            //region,
                                            //ddlSupplierDiversity.SelectedValue,
                                            ////fileUrl,
                                            //false
                                            //);

                                            //Email Subject.
                                            //string subject = string.Format("Supplier Application {0} – {1} ({2})", supplierId, txtCompany.Text, category.CategoryName);

                                            //Send email to Covanta team.
                                            //EmailHelper.SendEmail(category.EmailRecipients, subject, emailMessage, filePath);

                                            //Generate HTML for email message to be sent to Submitter.
                                            string submitterMessage = EmailHelper.GetSupplierFormHtml(
                                            supplierId,
                                            txtFullName.Text,
                                            txtPhone.Text,
                                            txtEmail.Text,
                                            txtCompany.Text,
                                            txtStreet.Text,
                                            txtCity.Text,
                                            txtState.Text,
                                            txtZipCode.Text,
                                            txtCountry.Text,
                                            txtCovantaContact.Text,
                                            category.CategoryName,
                                            subCategories,
                                            region,
                                            ddlSupplierDiversity.SelectedValue,
                                            //fileUrl,
                                            true
                                            );

                                            //Send email to Submitter.
                                            EmailHelper.SendEmail(txtEmail.Text, ConfigurationManager.AppSettings["SubmitterEmailSubject"], submitterMessage, string.Empty);

                                            GC.Collect();
                                            GC.WaitForPendingFinalizers();
                                            //File.Delete(filePath);
                                        }
                                        catch (Exception ex)
                                        {
                                            LogHelper.ErrorLogging(ex);

                                            GC.Collect();
                                            GC.WaitForPendingFinalizers();
                                            //File.Delete(filePath);
                                            throw new Exception(ex.Message);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.ErrorLogging(ex);
                            string technicalErrorMail = string.Format("<a href='mailto:{0}'>{1}</a>", ConfigurationManager.AppSettings["TechnicalErrorMail"], ConfigurationManager.AppSettings["TechnicalErrorMail"]);
                            string technicalErrorMessage = ConfigurationManager.AppSettings["TechnicalErrorMessage"].Replace("#TechnicalErrorMail#", technicalErrorMail);
                            lblError.Text = technicalErrorMessage;
                            lblError.Visible = true;
                            hasException = true;
                        }

                        if (!hasException)
                        {
                            //Redirect to Thank You page.
                            Response.Redirect("thankyou.aspx");
                        }
                    }
                    else
                    {
                        lblError.Text = "Invalid file uploaded !!";
                        lblError.Visible = true;
                    }
                }
            }
            else if (!RecaptchaHelper.IsReCaptchValid(Request))
            {
                divRecaptcha.Style.Add("border", "2px solid red");
                divRecaptcha.Style.Add("width", "306px");
            }
        }

        /// <summary>
        /// Load Categories DropDown.
        /// </summary>
        protected void BindCategoryDropDown()
        {
            List<Category> categories = DataHelper.GetCategories();

            ddlCategories.DataTextField = "CategoryName";
            ddlCategories.DataValueField = "CategoryID";
            ddlCategories.DataSource = categories;
            ddlCategories.DataBind();

            ddlCategories.Items.Insert(0, new ListItem("Select Category", "0"));
        }

        /// <summary>
        /// Trigger event when the value is changed in Categories DropDown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectCategoryId = int.Parse(ddlCategories.SelectedValue);
            if (selectCategoryId > 0)
            {
                BindSubCategoryCheckBoxList(selectCategoryId);
                divSubCategory.Visible = true;
            }
            else
            {
                divSubCategory.Visible = false;
            }
        }

        /// <summary>
        /// Load Sub Categories CheckBoxList.
        /// </summary>
        /// <param name="categoryId"></param>
        protected void BindSubCategoryCheckBoxList(int categoryId)
        {
            List<SubCategory> subCategories = DataHelper.GetSubCategoriesByCategoryID(categoryId);

            cblSubCatecories.DataTextField = "SubCategoryName";
            cblSubCatecories.DataValueField = "SubCategoryName";
            cblSubCatecories.DataSource = subCategories;
            cblSubCatecories.DataBind();
        }

        /// <summary>
        /// Load Regions DropDown.
        /// </summary>
        protected void BindRegionDropDown()
        {
            List<Region> regions = DataHelper.GetRegions();

            ddlRegion.DataTextField = "RegionName";
            ddlRegion.DataValueField = "RegionName";
            ddlRegion.DataSource = regions;
            ddlRegion.DataBind();

            ddlRegion.Items.Insert(0, new ListItem("Select Region", "0"));
        }

        /// <summary>
        /// Custom validate Supplier Form fields on form submit.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Check if Company is not empty.
            bool is_companyValid = txtCompany.Text != "";
            txtCompany.BorderColor = is_companyValid ? System.Drawing.Color.Gray : System.Drawing.Color.Red;

            //Check if Full Name is not empty.
            bool is_fullNameValid = txtFullName.Text != "";
            txtFullName.BorderColor = is_fullNameValid ? System.Drawing.Color.Gray : System.Drawing.Color.Red;

            //Check if Phone Number is not empty.
            bool is_phoneValid = IsPhoneNumber(txtPhone.Text);
            txtPhone.BorderColor = is_phoneValid ? System.Drawing.Color.Gray : System.Drawing.Color.Red;


            //Check if Zip/Postal code is not empty.
            bool is_zipCodeValid = IsZipCode(txtZipCode.Text);
            txtZipCode.BorderColor = is_zipCodeValid ? System.Drawing.Color.Gray : System.Drawing.Color.Red;

            //Check if Email is valid.
            bool is_emailValid = txtEmail.Text != "" && new EmailAddressAttribute().IsValid(txtEmail.Text);
            txtEmail.BorderColor = is_emailValid ? System.Drawing.Color.Gray : System.Drawing.Color.Red;

            //Check if Street is not empty.
            bool is_streetValid = txtStreet.Text != "";
            txtStreet.BorderColor = is_streetValid ? System.Drawing.Color.Gray : System.Drawing.Color.Red;

            //Check if City is not empty.
            bool is_cityValid = txtCity.Text != "";
            txtCity.BorderColor = is_cityValid ? System.Drawing.Color.Gray : System.Drawing.Color.Red;

            //Check if State is not empty.
            bool is_stateValid = txtState.Text != "";
            txtState.BorderColor = is_stateValid ? System.Drawing.Color.Gray : System.Drawing.Color.Red;

            //Check if valid Category is selected.
            bool is_categoryValid = ddlCategories.SelectedValue != "0";
            ddlCategories.BorderColor = is_categoryValid ? System.Drawing.Color.Gray : System.Drawing.Color.Red;

            //Check if valid Supplier Diversity is selected.
            bool is_supplierDiversityValid = !string.IsNullOrWhiteSpace(ddlSupplierDiversity.SelectedValue);
            ddlSupplierDiversity.BorderColor = is_supplierDiversityValid ? System.Drawing.Color.Gray : System.Drawing.Color.Red;

            //Check if file is selected.
            bool is_fileAttached = fileAttachments.HasFile;
            divFileAttachment.Style.Add("border", "1px solid red");

            args.IsValid = is_supplierDiversityValid && is_fileAttached && is_companyValid && is_fullNameValid && is_phoneValid && is_emailValid && is_streetValid && is_cityValid && is_stateValid && is_categoryValid;
            LogHelper.LogInfo(args.IsValid.ToString());
        }

        /// <summary>
        /// Load Supplier Diverisity DropDown.
        /// </summary>
        protected void PopulateSupplierDiversityDropDown()
        {
            ddlSupplierDiversity.Items.Add(new ListItem("Are you a Diverse Supplier?", ""));
            ddlSupplierDiversity.Items.Add(new ListItem("Yes", "Yes"));
            ddlSupplierDiversity.Items.Add(new ListItem("No", "No"));
        }

        protected bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^\d{10}$").Success;
        }

        protected bool IsZipCode(string number)
        {
            return Regex.Match(number, @"^\d{5}$").Success;
        }
    }
}