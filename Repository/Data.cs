using EBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EBilling.Repository;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace EBilling.Repository
{
    public class Data : IData
    {
        public string connectionString { get; set; }
        public Data()
        {
            connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
        }
        public void saveBillingDetails(BillDetail details)
        {
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                details.TotalAmount = details.Items.Sum(i => i.Price * i.Quantity);
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_saveEBillingDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CoustomerName", details.CoustomerName);
                    cmd.Parameters.AddWithValue("@MobileNumber", details.MobileNumber);
                    cmd.Parameters.AddWithValue("@Address", details.Address);
                    cmd.Parameters.AddWithValue("@TotalAmount", details.TotalAmount);


                    SqlParameter outputpara = new SqlParameter();
                    outputpara.DbType = DbType.Int32;
                    outputpara.Direction = ParameterDirection.Output;
                    outputpara.ParameterName = "@Id";
                    cmd.Parameters.Add(outputpara);
                    cmd.ExecuteNonQuery();
                    int id = int.Parse(outputpara.Value.ToString());

                    if (details.Items.Count > 0)
                    {
                        saveBillItems(details.Items, con, id);
                    }


                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public void saveBillItems(List<Items> items, SqlConnection con, int id)
        {
            try
            {
                string qry = "insert into BillingItems(ProductName,Price,Quantity,BillId)values";
                foreach (var item in items)
                {
                    qry += string.Format("('{0}',{1},{2},{3}),", item.ProductName, item.Price, item.Quantity, id);
                }
                qry = qry.Remove(qry.Length - 1);
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<BillDetail> GetAllDetail()
        {
            List<BillDetail> list = new List<BillDetail>();
            BillDetail detail;
            SqlConnection con = new SqlConnection(connectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("sp_getAllEBillingDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    detail = new BillDetail();
                    detail.Id = int.Parse(reader["Id"].ToString());
                    detail.CoustomerName = reader["CoustomerName"].ToString();
                    detail.MobileNumber = (reader["MobileNumber"].ToString());
                    detail.Address = reader["Address"].ToString();
                    detail.TotalAmount = int.Parse(reader["TotalAmount"].ToString());
                    list.Add(detail);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return list;
        }

       
        public BillDetail GetDetail(int Id )
        {
            SqlConnection con = new SqlConnection(connectionString);
            BillDetail detail = new BillDetail();
            Items item;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_getEBillingDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                   
                }

                while (reader.Read())
                {
                    detail.Id = int.Parse(reader["BillID"].ToString());
                    detail.CoustomerName = reader["CoustomerName"].ToString();
                    detail.MobileNumber = reader["MobileNumber"].ToString();
                    detail.Address = reader["address"].ToString();
                    detail.TotalAmount = int.Parse(reader["TotalAmount"].ToString());

                    item = new Items();
                    item.Id = int.Parse(reader["ItemId"].ToString());
                    item.ProductName = reader["ProductName"].ToString();
                    item.Price = int.Parse(reader["Price"].ToString());
                    item.Quantity = int.Parse(reader["Quantity"].ToString());
                    detail.Items.Add(item);

                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return detail;
        }

    }

}