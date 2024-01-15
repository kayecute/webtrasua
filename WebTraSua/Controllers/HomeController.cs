using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTraSua.Models;

namespace WebTraSua.Controllers
{
    public class HomeController : Controller
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        private string connectionString = ConfigurationManager.ConnectionStrings["QuanLiTraSuaConnectionString"].ToString();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            // Thực hiện truy vấn SQL để tìm kiếm dữ liệu
            var searchResults = new List<ChiTietSanPham>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Sử dụng SqlCommand để thực hiện truy vấn
                string query = "SELECT * FROM ChiTietSanPham WHERE TenSP LIKE @searchTerm";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Đọc dữ liệu từ cột và thêm vào danh sách kết quả
                            ChiTietSanPham result = new ChiTietSanPham
                            {
                                MaSP = Convert.ToInt32(reader["MaSP"]),
                                TenSP = reader["TenSP"].ToString(),
                                // Thêm các trường khác tương ứng
                            };

                            searchResults.Add(result);
                        }
                    }
                }
            }
            // Chuyển danh sách kết quả sang view để hiển thị
            return View("SearchResults", searchResults);
        }


    }
}