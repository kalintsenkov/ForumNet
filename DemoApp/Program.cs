namespace DemoApp
{
    using Microsoft.EntityFrameworkCore;
    using ForumNet.Data;

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
