using System.Threading.Tasks;
using Majipro.Hwid.Services.HwidProvision;

namespace Majipro.Hwid.Tests.Tests;

[TestClass]
public class WindowsHwidProvisionServiceTests : ProvisionServiceTestsBase
{
    [TestInitialize]
    public async Task TestInitializeAsync()
    {
        await TestInitializeAsync<WindowsHwidProvisionService>();
    }

    [TestMethod]
    public void Test_Command_With_Arguments()
    {
        CollectionAssert.AreEqual(new[] { "-Command \"(Get-WmiObject -Class Win32_ComputerSystemProduct).UUID\"" }, HwidProvisionService.Args);
        Assert.AreEqual("powershell", HwidProvisionService.Command);
    }
    
    [TestMethod]
    [DataRow("071C5AE2-E32F-4920-8790-9BEFCBF8CFBC", "071C5AE2-E32F-4920-8790-9BEFCBF8CFBC")]
    [DataRow("      071C5AE2-E32F-4920-8790-9BEFCBF8CFBC", "071C5AE2-E32F-4920-8790-9BEFCBF8CFBC")]
    [DataRow("071C5AE2-E32F-4920-8790-9BEFCBF8CFBC    ", "071C5AE2-E32F-4920-8790-9BEFCBF8CFBC")]
    public void When_Valid_StdOut_Is_Provided_Then_Return_Hwid(string stdoutRow, string expectedHwid)
    {
        bool result = HwidProvisionService.ParseHwidFromStdOut(stdoutRow, out var hwid);
        
        Assert.IsTrue(result);
        Assert.AreEqual(expectedHwid, hwid);
    }
}