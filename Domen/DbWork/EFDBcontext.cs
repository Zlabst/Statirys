// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"

using System.Data.Entity;

namespace Domen.DbWork
{

    /// <summary>
    /// Инкапсулирует контекст обменка данными с БД
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class EFDBcontext : DbContext
    {
        static string connectionString = "Data Source=ms-sql-8.in-solve.ru;Initial Catalog=1gb_instaapi;Persist Security Info=True;User ID=1gb_textilehouse;Password=5f54a4f8ty";
        public EFDBcontext() : base(connectionString)
        {
            var ensureDLLIsCopied =
                System.Data.Entity.SqlServer.SqlProviderServices.Instance;

        }

        public DbSet<Account> Accounts { get; set; }
    }
}
