// Разработано YV
// Эл. почта : yura21998@mail.ru
// Более на "github.com/yurijvolkov"

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Domen.DbWork
{
    /// <summary>
    /// Инкапсулирует интерфейс для работы с БД
    /// </summary>
    public class EFRepository
    {
        private EFDBcontext context = new EFDBcontext();

        public IEnumerable<Account> Accounts
        { get { return context.Accounts; } }

        public void Delete(long accountId)
        {
            Account old = context.Accounts.First(x => x.InstaId == accountId);
            context.Entry(old).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }
        public void Save(Account account)
        {
            if (context.Accounts.Select(x => x)
                            .Where(x => x.InstaId == account.InstaId)
                            .Count() == 0)
            {
                context.Accounts.Add(account);
            }
            else
            {
                Account old = context.Accounts.First(x => x.InstaId == account.InstaId);
                old.Followers = account.Followers;
                old.Following = account.Following;
                old.Likes = account.Likes;
                old.AccessToken = account.AccessToken;
                old.Start = account.Start;
                old.Media = account.Media;
                context.Entry(old).State = EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}
