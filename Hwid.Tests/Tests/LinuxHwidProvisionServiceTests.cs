using System.Threading.Tasks;
using Majipro.Hwid.Services.HwidProvision;

namespace Majipro.Hwid.Tests.Tests;

[TestClass]
public sealed class LinuxHwidProvisionServiceTests : ProvisionServiceTestsBase
{
    [TestInitialize]
    public async Task TestInitializeAsync()
    {
        await TestInitializeAsync<LinuxHwidProvisionService>();
    }

    [TestMethod]
    public void Test_Command_With_Arguments()
    {
        CollectionAssert.AreEqual(new[] { "/usr/bin/cat", "/sys/class/dmi/id/product_uuid" }, HwidProvisionService.Args);
        Assert.AreEqual("/usr/bin/sudo", HwidProvisionService.Command);
    }
    
    [TestMethod]
    [DataRow("3e43ba91-a1e4-4250-8ba1-818671058b1e", "3e43ba91-a1e4-4250-8ba1-818671058b1e")]
    [DataRow("      3e43ba91-a1e4-4250-8ba1-818671058b1e", "3e43ba91-a1e4-4250-8ba1-818671058b1e")]
    [DataRow("3e43ba91-a1e4-4250-8ba1-818671058b1e    ", "3e43ba91-a1e4-4250-8ba1-818671058b1e")]
    public void When_Valid_StdOut_Is_Provided_Then_Return_Hwid(string stdoutRow, string expectedHwid)
    {
        bool result = HwidProvisionService.ParseHwidFromStdOut(stdoutRow, out var hwid);
        
        Assert.IsTrue(result);
        Assert.AreEqual(expectedHwid, hwid);
    }
    
    [TestMethod]
    [DataRow("cat: /sys/class/dmi/id/product_uuid: Permission denied")]
    [DataRow("cat: /sys/class/dmi/id/product_uuid: Permission denied   ")]
    [DataRow("   cat: /sys/class/dmi/id/product_uuid: Permission denied")]
    public void When_User_Do_Not_Have_Permissions_Then_Return_Nothing(string stdoutRow)
    {
        bool result = HwidProvisionService.ParseHwidFromStdOut(stdoutRow, out var hwid);
        
        Assert.IsFalse(result);
        Assert.AreEqual(default, hwid);
    }
}