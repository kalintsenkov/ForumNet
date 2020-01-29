namespace DemoApp
{
    using ForumNet.Data;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        public static void Main()
        {
            using var db = new ForumDbContext();

            db.Database.EnsureDeleted();
            db.Database.Migrate();
        }
    }
}
