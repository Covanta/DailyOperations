using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Covanta.BusinessLogic;
using System.Configuration;
using Covanta.Entities;
using System.Text;

namespace Covanta.UI.FontlineEmployeeVerification
{
    public partial class _Default : System.Web.UI.Page
    {
        #region private fields
        string _covIntegrationConnString;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // first time loading page
            if (!Page.IsPostBack)
            {
                //InitializeForm();               

                //LoadForm();
            }
        }

        #region buttons

        protected void btn_GetRecords_Click(object sender, EventArgs e)
        {
             _covIntegrationConnString = ConfigurationManager.ConnectionStrings["covIntegrationConnString"].ConnectionString;
           int rowCount = poplateDataGrid();
           setMessage(rowCount);
        }

        protected void btn_generatePassword_Click(object sender, EventArgs e)
        {
            txtBox_newPassword.Text = createPassword();
        }

        

        #endregion

        #region private methods

        private int poplateDataGrid()
        {
            
            //show AD info

            //List<Covanta.Entities.ActiveDirectoryInfo> list = new List<Entities.ActiveDirectoryInfo>();
            //string lastname = txtBox_LastName.Text;
            //EmployeeDataManager manager = new EmployeeDataManager(_covIntegrationConnString);
            //list = manager.GetActiveDirectoryInfoListByLastName(lastname);
            //GridView1.DataSource = list;
            //GridView1.DataBind();

            //show All Employee info

            //List<EmployeeInfo> list = new List<EmployeeInfo>();
            //string lastname = txtBox_LastName.Text;
            //EmployeeDataManager manager = new EmployeeDataManager(_covIntegrationConnString);
            //list = manager.GetEmployeeInfoListByLastName(lastname);
            //GridView1.DataSource = list;
            //GridView1.DataBind();

            //show limited Employee info needed for  Frontline app

            List<EmployeeInfoForFrontLineVerification> list = new List<EmployeeInfoForFrontLineVerification>();
            string lastname = txtBox_LastName.Text;
            EmployeeDataManager manager = new EmployeeDataManager(_covIntegrationConnString);
            list = manager.GetEmployeeInfoForFrontLineVerificationListByLastName(lastname);
            GridView1.DataSource = list;
            GridView1.DataBind();
            return list.Count();
        }

        private void setMessage(int rowCount)
        {
            if (txtBox_LastName.Text == string.Empty)
            {
                lbl_Message1.Text = "Please enter a Last Name";
                return;
            }

            lbl_Message1.Text = txtBox_LastName.Text;

            if (rowCount == 0)
            {
                lbl_Message1.Text = lbl_Message1.Text + " is not a valid name in Active Directory";
            }
            else
            {
               // lbl_Message1.Text = lbl_Message1.Text + " is a valid name in Active Directory";
                lbl_Message1.Text = string.Empty;
            }
        }

        #endregion

        #region private methods

  
        private string createPassword()
        {
            string myPwd = string.Empty;
            System.Random myRdn = new Random();

            ASCIIEncoding myASCII = new ASCIIEncoding();
            double rdn;
            int nn;

            for (int i = 1; i <= 8; i++)
            {
                rdn = myRdn.NextDouble();

                int n = Convert.ToInt32((4 * rdn) + 1);

                switch (n)
                {
                    case 1:
                    case 2:
                        nn = Convert.ToInt32((26 * rdn) + 1) + 96;
                        myPwd += Convert.ToChar(nn).ToString();

                        break;

                    case 3:
                        rdn = myRdn.NextDouble();
                        nn = Convert.ToInt32((26 * rdn) + 1) + 64;
                        myPwd += Convert.ToChar(nn).ToString();

                        break;

                    default:
                        rdn = myRdn.NextDouble();
                        nn = Convert.ToInt32((8 * rdn) + 2) + 47;
                        myPwd += Convert.ToChar(nn).ToString();

                        break;
                }
            }
            return myPwd;
        }
      
        #endregion

    }
}