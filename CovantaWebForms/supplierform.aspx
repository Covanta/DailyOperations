<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="supplierform.aspx.cs" Inherits="CovantaWebForms.SupplierForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        /*-- AC Form: Global Styles --*/
        .ac-form-div,
        .ac-form-div * {
            box-sizing: border-box;
        }

        .ac-form-row {
            margin-bottom: 10px;
            width: 100%;
        }

            .ac-form-row br {
                display: none;
            }

            .ac-form-row input,
            .ac-form-row select,
            .ac-form-row textarea {
                border: 1px solid #aaa;
                font-family: sans-serif;
                padding: 7px 5px;
                width: 100%;
            }

        .ac-form-control input[type=submit] {
            background-color: #333;
            border: none;
            color: #fff;
            cursor: pointer;
            padding: 10px 25px;
            width: auto;
        }

        .ac-form-captcha input {
            margin-top: 5px;
            max-width: 225px;
        }

        .ac-form-captcha img {
            margin-bottom: 10px;
        }

        .ac-form-captcha br {
            display: block;
        }

        .ac-form-message,
        .ac-form-message span {
            background: none !important;
            padding: 0 !important;
        }

        .ac-form-field-invalid,
        .ac-form-field-invalid {
            background-color: #fffafa !important;
            border-color: #d9534f !important;
        }

        .ac-form-checkboxlist input {
            width: 5%;
        }

        .ac-form-checkboxlist {
            width: 100%;
        }
    </style>
    <script src="scripts/jquer-1.2.6.js"></script>
    <script type="text/javascript">
        function ValidateTextBox(source, args) {
            var is_companyValid = $("#txtCompany").val() != "";
            $("#txtCompany").css("border-color", is_companyValid ? "white" : "red");

            var is_fullNameValid = $("#txtFullName").val() != "";
            $("#txtFullName").css("border-color", is_fullNameValid ? "white" : "red");

            var is_phoneValid = $("#txtPhone").val() != "";
            $("#txtPhone").css("border-color", is_phoneValid ? "white" : "red");

            var is_emailValid = ValidateEmail($("#txtEmail").val());
            $("#txtEmail").css("border-color", is_emailValid ? "white" : "red");

            var is_streetValid = $("#txtStreet").val() != "";
            $("#txtStreet").css("border-color", is_streetValid ? "white" : "red");

            var is_cityValid = $("#txtCity").val() != "";
            $("#txtCity").css("border-color", is_cityValid ? "white" : "red");

            var is_stateValid = $("#txtState").val() != "";
            $("#txtState").css("border-color", is_stateValid ? "white" : "red");

            var is_categoryValid = validateCategory();
            $("#Category").css("border-color", is_categoryValid ? "white" : "red");

            args.IsValid = is_companyValid && is_fullNameValid && is_phoneValid && is_emailValid && is_streetValid && is_cityValid && is_stateValid && is_categoryValid;
        }

        function ValidateEmail(mail) {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(myForm.emailAddr.value)) {
                return (true)
            }
            return (false)
        }

        function validateCategory() {
            if ($('#Category').val() == "0") {
                return (false);
            }
            else {
                return (true);
            }
        }
    </script>
    <title>Covanta - Supplier Form</title>
</head>
<body>
    <form id="form1" runat="server">
        <p style="text-align: justify;font-family: sans-serif;line-height: 20px;">Thank you for your interest in becoming a supplier to Covanta. Before you submit the form below, please download <a href="/files/covanta nda.pdf" target="_blank">Covanta Confidentiality agreement</a>, sign it, then submit it using the “Signed NDA” attachment field in the form.</p>
        <asp:Label ID="lblError" runat="server" Text="Your form could not be submitted due to technical difficulty. Please contact support <a href='mailto:supplieronboarding@covanta.com'>supplieronboarding@covanta.com</a> for further assistance." Visible="false" ForeColor="Red"/>
        <div id="acForm_25437">
            <div class="ac-form-div">
                <div class="ac-form-row">
                    <asp:TextBox ID="txtCovantaContact" runat="server" CssClass="ac-form-input ac-form-field-req" PlaceHolder="Covanta Contact" />
                    <asp:CustomValidator Display="None" ValidationGroup="valGroup" ID="CustomValidator1" runat="server" ErrorMessage="" ControlToValidate="txtCovantaContact" ClientValidationFunction="ValidateTextBox" EnableClientScript="false" ValidateEmptyText="True" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                </div>

                <div class="ac-form-row">
                    <asp:TextBox ID="txtCompany" runat="server" CssClass="ac-form-input ac-form-field-req" PlaceHolder="Company *" />
                </div>

                <div class="ac-form-row">
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="ac-form-input ac-form-field-req" PlaceHolder="Full Name *" />
                </div>

                <div class="ac-form-row">
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="ac-form-input ac-form-field-req" PlaceHolder="Phone *" />
                </div>

                <div class="ac-form-row">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="ac-form-input ac-form-field-req" PlaceHolder="Email *" />
                </div>

                <div class="ac-form-row">
                    <asp:TextBox ID="txtStreet" runat="server" CssClass="ac-form-input ac-form-field-req" PlaceHolder="Street *" />
                </div>

                <div class="ac-form-row">
                    <asp:TextBox ID="txtCity" runat="server" CssClass="ac-form-input ac-form-field-req" PlaceHolder="City *" />
                </div>

                <div class="ac-form-row">
                    <asp:TextBox ID="txtState" runat="server" CssClass="ac-form-input ac-form-field-req" PlaceHolder="State/Province *" />
                </div>

                <div class="ac-form-row">
                    <asp:TextBox ID="txtZipCode" runat="server" CssClass="ac-form-input ac-form-field-req" PlaceHolder="Zip/Postal Code" type="number"/>
                </div>

                <div class="ac-form-row">
                    <asp:TextBox ID="txtCountry" runat="server" CssClass="ac-form-input ac-form-field-req" Text="USA" ReadOnly="true" Enabled="false" />
                    <span style="text-align: justify;font-family: sans-serif;line-height:12px;font-size:10pt">If outside the US, please email <a href="mailto:supplieronboarding@covanta.com">supplieronboarding@covanta.com</a> for instructions.</span>
                </div>

                <div class="ac-form-row">
                    <asp:DropDownList ID="ddlCategories" runat="server" CssClass="ac-form-input ac-form-field-req" OnSelectedIndexChanged="ddlCategories_SelectedIndexChanged" AutoPostBack="true" />
                </div>

                <div class="ac-form-row" style="border: 1px solid #aaa; padding: 7px 5px; font-family: sans-serif; font-size: 13px;" id="divSubCategory" runat="server" visible="false">
                    <span style="padding-left: 5px;">Sub Categories:</span>
                    <asp:CheckBoxList ID="cblSubCatecories" runat="server" CssClass="ac-form-checkboxlist" Style="margin-top: 10px;" />
                </div>

                <div class="ac-form-row">
                    <asp:DropDownList ID="ddlRegion" runat="server" CssClass="ac-form-input ac-form-field-req" />
                </div>

                <div class="ac-form-row" style="border: 1px solid #aaa; padding: 7px 5px; font-family: sans-serif; font-size: 13px;" id="divFileAttachment" runat="server">
                    <span style="padding-left: 5px;">Signed NDA:</span>
                    <asp:FileUpload ID="fileAttachments" runat="server" class="ac-form-input" accept=".pdf, .jpg, .jpe, .png" Style="border: none;" />
                </div>

                <div class="ac-form-row">
                    <asp:DropDownList ID="ddlSupplierDiversity" runat="server" CssClass="ac-form-input ac-form-field-req" />
                </div>

                <div class="ac-form-row" id="divRecaptcha" runat="server" visible="false">
                    <div class="ac-form-control ac-form-captcha">
                        <div class="g-recaptcha" data-sitekey="6LfcyJgUAAAAAOQWxXlI_4qMFfu0Cn7Yhx3jaP0f">
                        </div>
                        <span id="152631_errorMessage"></span>
                    </div>
                </div>

                <div class="ac-form-row">
                    <br />
                    <div class="ac-form-control">
                        <asp:Button runat="server" ID="btnSubmit" class="ac-form-submit" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="valGroup" />
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src='https://www.google.com/recaptcha/api.js'></script>
</body>
</html>
