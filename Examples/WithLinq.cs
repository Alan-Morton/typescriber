using ToTypeScript.Tests;
using ToTypeScript.Test.Examples.Dom;

namespace ToTypeScript.Test.Examples
{
    [TestClass]
    public class WithLinq : BaseTest 
    {
        [TestMethod]
        public void WithLinqExample()
        {
            string returnValue = string.Empty;
            returnValue = LinqExtention.ToTypeScriptString<AccountVM>();
            Assert.IsNotNull(returnValue);
            ValidateTypeScript(returnValue);
        }

        [TestMethod]
        public void WithLinqFodExample()
        {
            List<AccountVM> accountList = new List<AccountVM>();
            AccountVM accountVM = new AccountVM();
            accountVM.AccountName = "AlanDashboard";
            accountVM.Id = "gyshkjas";
            accountList.Add(accountVM);
            string returnValue = "";
            returnValue = accountList.FirstOrDefault().ToTypeScriptString();
            Assert.IsNotNull(returnValue);
            ValidateTypeScript(returnValue);
        }
    }
}
