namespace LibraryManagement.Services
{
    public static class SessionHelper
    {
        public static bool IsLoggedIn(HttpContext context)
        {
            return context.Session.GetInt32("MaTaiKhoan") != null;
        }

        public static string GetRole(HttpContext context)
        {
            return context.Session.GetString("VaiTro") ?? string.Empty;
        }

        public static string GetHoTen(HttpContext context)
        {
            return context.Session.GetString("HoTen") ?? string.Empty;
        }

        public static int GetMaTaiKhoan(HttpContext context)
        {
            return context.Session.GetInt32("MaTaiKhoan") ?? 0;
        }

        public static bool IsAdmin(HttpContext context)
        {
            return GetRole(context) == "Admin";
        }

        public static bool IsThuThu(HttpContext context)
        {
            return GetRole(context) == "ThuThu";
        }

        public static bool IsDocGia(HttpContext context)
        {
            return GetRole(context) == "DocGia";
        }
    }
}