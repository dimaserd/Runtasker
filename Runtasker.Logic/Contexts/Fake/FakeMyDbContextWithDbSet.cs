using FakeDbSet;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System.Data.Entity;

namespace Runtasker.Logic.Contexts.Fake
{
    public class FakeMyDbContextWithDbSet : IMyDbContextWithDbSet
    {
        #region Fields
        InMemoryDbSet<ApplicationUser> _users;

        InMemoryDbSet<OtherUserInfo> _otherUserInfos;
        #endregion
        public DbSet<ApplicationUser> Users
        {
            get
            {
                if(_users == null)
                {
                    _users = new InMemoryDbSet<ApplicationUser>();
                }
                return _users;
            }
        }

        public DbSet<OtherUserInfo> OtherUserInfos
        {
            get
            {
                if (_otherUserInfos == null)
                {
                    _otherUserInfos = new InMemoryDbSet<OtherUserInfo>();
                }
                return _otherUserInfos;
            }
        }

    }
}
