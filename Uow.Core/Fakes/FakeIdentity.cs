using System;
using System.Security.Principal;

namespace Uow.Core.Fakes
{
    public class FakeIdentity : IIdentity
    {
        public FakeIdentity(string userName)
        {
            Name = userName;
        }

        public string AuthenticationType => throw new NotImplementedException();

        public bool IsAuthenticated => !string.IsNullOrEmpty(Name);

        public string Name { get; }
    }
}