using System.Threading.Tasks;
using Majipro.Hwid.Services.HwidProvision;

namespace Majipro.Hwid.Tests.Tests;

[TestClass]
public sealed class OsXHwidProvisionServiceTests : ProvisionServiceTestsBase
{
    [TestInitialize]
    public async Task TestInitializeAsync()
    {
        await TestInitializeAsync<OsXHwidProvisionService>();
    }

    [TestMethod]
    public void Test_Command_With_Arguments()
    {
        CollectionAssert.AreEqual(new[] { "-rd1", "-c", "IOPlatformExpertDevice" }, HwidProvisionService.Args);
        Assert.AreEqual("ioreg", HwidProvisionService.Command);
    }
    
    [TestMethod]
    [DataRow("\"IOPlatformSerialNumber\" = \"C02ZZ000ML7H\"", "C02ZZ000ML7H")]
    [DataRow("      \"IOPlatformSerialNumber\" = \"C02ZZ000ML7H\"", "C02ZZ000ML7H")]
    [DataRow("\"IOPlatformSerialNumber\" = \"ABC\"", "ABC")]
    [DataRow("\"IOPlatformSerialNumber\" = \"A=BC\"", "A=BC")]
    public void When_Valid_StdOut_Is_Provided_Then_Return_Hwid(string stdoutRow, string expectedHwid)
    {
        bool result = HwidProvisionService.ParseHwidFromStdOut(stdoutRow, out var hwid);
        
        Assert.IsTrue(result);
        Assert.AreEqual(expectedHwid, hwid);
    }
}