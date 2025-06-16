namespace Blog_API.Utilities
{
    enum RoleMap
    {
        admin ,
        contributor,
        reader
    }
    public class SD
    {
        public static string SessionToken { get; set; } = "This is secret token";
        /*
         * admin: vienvietvo Pvcb@123
         * reader" reader1 Pvcb@123
         *Postman: Authorization, Bearer Token: nhap token vo 
         */
    }
}
