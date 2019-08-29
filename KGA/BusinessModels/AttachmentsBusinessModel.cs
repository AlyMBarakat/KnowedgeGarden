using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace KGA.Models
{
    public class AttachmentsBusinessModel
    {
        public AttachmentsDataModel attachment = new AttachmentsDataModel();
        public void uploadData(HttpPostedFileBase filePosted)
        {
            try
            {
                attachment.FileName = Path.GetFileName(filePosted.FileName);
                attachment.ContentType = filePosted.ContentType;

                using (Stream fs = filePosted.InputStream)
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        attachment.Data = br.ReadBytes((Int32)fs.Length);
                        string CS = ConfigurationManager.ConnectionStrings["KGDBconnection"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(CS))
                        {
                            string strQuery = "insert into Attachments(UserName, ProblemID, FileName, ContentType, Data) values (@UserName, @ProblemID, @FileName, @ContentType, @Data)";
                            using (SqlCommand cmd = new SqlCommand(strQuery))
                            {
                                cmd.Connection = con;
                                cmd.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar).Value = attachment.UserName;
                                cmd.Parameters.Add("@ProblemID", System.Data.SqlDbType.Int).Value = attachment.ProblemID;
                                cmd.Parameters.Add("@FileName", System.Data.SqlDbType.VarChar).Value = attachment.FileName;
                                cmd.Parameters.Add("@ContentType", System.Data.SqlDbType.VarChar).Value = "application/vnd.ms-word";
                                cmd.Parameters.Add("@Data", System.Data.SqlDbType.VarBinary).Value = attachment.Data;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }
                }
            }
            catch( Exception ex)
            {
                Debug.WriteLine("No file is entered");
            }
            
        }

        public void DownloadFile(int FileID)
        {
            
            //byte[] bytes;
            string strQuery = ConfigurationManager.ConnectionStrings["KGDBconnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strQuery))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select FileName, Data, ContentType from Attachments where FileID=@id";
                    cmd.Parameters.AddWithValue("@id", FileID);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        attachment.Data = (byte[])sdr["Data"];
                        attachment.ContentType = sdr["ContentType"].ToString();
                        attachment.FileName = sdr["FileName"].ToString();
                    }
                    con.Close();
                }
            }        
        }
    }
}