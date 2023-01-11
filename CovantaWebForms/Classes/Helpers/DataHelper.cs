using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CovantaWebForms.Classes.Helpers
{
    public static class DataHelper
    {
        /// <summary>
        /// Insert a record in database for submitted Supplier Form.
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="company"></param>
        /// <param name="street"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zipCode"></param>
        /// <param name="country"></param>
        /// <param name="covantaContact"></param>
        /// <param name="category"></param>
        /// <param name="subCategories"></param>
        /// <param name="region"></param>
        /// <param name="supplierDiversity"></param>
        /// <returns></returns>
        public static string InsertForm(
            string fullName,
            string phone,
            string email,
            string company,
            string street,
            string city,
            string state,
            string zipCode,
            string country,
            string covantaContact,
            string category,
            List<string> subCategories,
            string region,
            bool supplierDiversity)
        {
            string returnValue = string.Empty;
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["covConnString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "InsertDetailsIntoSupplierForm";

                        command.Parameters.AddWithValue("@covantaContact", covantaContact);
                        command.Parameters.AddWithValue("@company", company);
                        command.Parameters.AddWithValue("@fullName", fullName);
                        command.Parameters.AddWithValue("@phone", phone);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@street", street);
                        command.Parameters.AddWithValue("@city", city);
                        command.Parameters.AddWithValue("@state", state);
                        command.Parameters.AddWithValue("@zipcode", zipCode);
                        command.Parameters.AddWithValue("@country", country);
                        command.Parameters.AddWithValue("@category", category);
                        command.Parameters.AddWithValue("@subCategory", String.Join(",", subCategories));
                        command.Parameters.AddWithValue("@region", region);
                        command.Parameters.AddWithValue("@files", string.Empty);
                        command.Parameters.AddWithValue("@supplierDiversity", supplierDiversity);

                        connection.Open();
                        object obj = command.ExecuteScalar();

                        returnValue = obj.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);
                throw new Exception(ex.Message);
            }

            return returnValue;
        }

        /// <summary>
        /// Retrieve Categories from database.
        /// </summary>
        /// <returns></returns>
        public static List<Category> GetCategories()
        {
            List<Category> list = new List<Category>();
            string commandString = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["covConnString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetCategories";

                        connection.Open();
                        // load dataSet
                        DS.Load(command.ExecuteReader(), LoadOption.OverwriteChanges, new string[] { "tableList" });

                        tableList = DS.Tables["tableList"];

                        // load objects from DataTable if at least 1 row returned
                        if (tableList.Rows.Count > 0)
                        {
                            foreach (DataRow dr in tableList.Rows)
                            {
                                Category details = new Category();
                                details = LoadCategoriesData(dr);
                                list.Add(details);
                            }
                        }

                        //set null reference to uneeded objects
                        tableList = null;
                        DS = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);
                throw new Exception(ex.Message);
            }
            return list;
        }

        /// <summary>
        /// Map categories data to class object.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static Category LoadCategoriesData(DataRow dr)
        {
            BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

            int categoryID = bindObj.ToInteger("CategoryID");
            string categoryName = bindObj.ToStringValue("CategoryName");
            string emailRecipients = bindObj.ToStringValue("EmailRecipients");
            string sharePointFolderName = bindObj.ToStringValue("SharePointFolderName");
            Category obj = new Category();

            obj.CategoryID = categoryID;
            obj.CategoryName = categoryName;
            obj.EmailRecipients = emailRecipients;
            obj.SharePointFolderName = sharePointFolderName;

            return obj;
        }

        /// <summary>
        /// Retrieve Sub Categories by Category ID.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static List<SubCategory> GetSubCategoriesByCategoryID(int categoryId)
        {
            List<SubCategory> list = new List<SubCategory>();
            string commandString = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["covConnString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetSubCategoriesByCategoryID";
                        command.Parameters.AddWithValue("@categoryId", categoryId);

                        connection.Open();
                        // load dataSet
                        DS.Load(command.ExecuteReader(), LoadOption.OverwriteChanges, new string[] { "tableList" });

                        tableList = DS.Tables["tableList"];

                        // load objects from DataTable if at least 1 row returned
                        if (tableList.Rows.Count > 0)
                        {
                            foreach (DataRow dr in tableList.Rows)
                            {
                                SubCategory details = new SubCategory();
                                details = LoadSubCategoriesData(dr);
                                list.Add(details);
                            }
                        }

                        //set null reference to uneeded objects
                        tableList = null;
                        DS = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);
                throw new Exception(ex.Message);
            }
            return list;
        }

        /// <summary>
        /// Map Sub Categories data to class object.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static SubCategory LoadSubCategoriesData(DataRow dr)
        {
            BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

            int subCategoryID = bindObj.ToInteger("SubCategoryID");
            string subCategoryName = bindObj.ToStringValue("SubCategoryName");
            int categoryID = bindObj.ToInteger("CategoryID");
            SubCategory obj = new SubCategory();

            obj.SubCategoryID = subCategoryID;
            obj.SubCategoryName = subCategoryName;
            obj.CategoryID = categoryID;

            return obj;
        }

        /// <summary>
        /// Get Email Recipients by Category ID.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static string GetEmailRecipientsByCategoryID(int categoryId)
        {
            string emailRecipients = string.Empty;
            string commandString = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["covConnString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetEmailRecipientsByCategoryID";
                        command.Parameters.AddWithValue("@categoryId", categoryId);

                        connection.Open();
                        // load dataSet
                        DS.Load(command.ExecuteReader(), LoadOption.OverwriteChanges, new string[] { "tableList" });

                        tableList = DS.Tables["tableList"];

                        // load objects from DataTable if at least 1 row returned
                        if (tableList.Rows.Count > 0)
                        {
                            foreach (DataRow dr in tableList.Rows)
                            {
                                emailRecipients = LoadEmailRecipients(dr);
                            }
                        }

                        //set null reference to uneeded objects
                        tableList = null;
                        DS = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);
                throw new Exception(ex.Message);
            }
            return emailRecipients;
        }

        /// <summary>
        /// Map Email Recipients to class object.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static string LoadEmailRecipients(DataRow dr)
        {
            BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

            string emailRecipients = bindObj.ToStringValue("EmailRecipients");

            return emailRecipients;
        }

        /// <summary>
        /// Retrieve Regions form database.
        /// </summary>
        /// <returns></returns>
        public static List<Region> GetRegions()
        {
            List<Region> list = new List<Region>();
            string commandString = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["covConnString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetRegions";

                        connection.Open();
                        // load dataSet
                        DS.Load(command.ExecuteReader(), LoadOption.OverwriteChanges, new string[] { "tableList" });

                        tableList = DS.Tables["tableList"];

                        // load objects from DataTable if at least 1 row returned
                        if (tableList.Rows.Count > 0)
                        {
                            foreach (DataRow dr in tableList.Rows)
                            {
                                Region details = new Region();
                                details = LoadRegions(dr);
                                list.Add(details);
                            }
                        }

                        //set null reference to uneeded objects
                        tableList = null;
                        DS = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);
                throw new Exception(ex.Message);
            }
            return list;
        }

        /// <summary>
        /// Map Regions data to class object.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static Region LoadRegions(DataRow dr)
        {
            BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

            int regionID = bindObj.ToInteger("RegionID");
            string regionName = bindObj.ToStringValue("RegionName");
            Region obj = new Region();

            obj.RegionID = regionID;
            obj.RegionName = regionName;

            return obj;
        }

        /// <summary>
        /// Update file path in Supplier table.
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="filePath"></param>
        public static void UpdateFilePathInSupplierForm(string supplierId, string filePath)
        {
            string returnValue = string.Empty;
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["covConnString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "UpdateFilesColumnInSupplierForm";

                        LogHelper.LogInfo(supplierId + "," + filePath);
                        command.Parameters.AddWithValue("@supplierId", supplierId);
                        command.Parameters.AddWithValue("@filePath", filePath);

                        connection.Open();
                        object obj = command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieve User by Email address.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static FormsUser GetUserByEmail(string email)
        {
            FormsUser user = new FormsUser();
            string commandString = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["covConnString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetUserDetailsByEmail";
                        command.Parameters.AddWithValue("@email", email);

                        connection.Open();
                        // load dataSet
                        DS.Load(command.ExecuteReader(), LoadOption.OverwriteChanges, new string[] { "tableList" });

                        tableList = DS.Tables["tableList"];

                        // load objects from DataTable if at least 1 row returned
                        if (tableList.Rows.Count > 0)
                        {
                            user = LoadUser(tableList.Rows[0]);
                        }

                        //set null reference to uneeded objects
                        tableList = null;
                        DS = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);
                throw new Exception(ex.Message);
            }
            return user;
        }

        /// <summary>
        /// Map User data to class object.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static FormsUser LoadUser(DataRow dr)
        {
            BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

            int userID = bindObj.ToInteger("UserID");
            string name = bindObj.ToStringValue("Name");
            string email = bindObj.ToStringValue("Email");
            string password = bindObj.ToStringValue("Password");
            FormsUser obj = new FormsUser();

            obj.UserID = userID;
            obj.Name = name;
            obj.Email = email;
            obj.Password = password;

            return obj;
        }
    }
}