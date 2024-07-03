using OpenPay.Interface;
using OpenPay.SDK;

namespace OpenPay.Core;

public class TestContext : ITestContext
{
    public string TestData { get; set; }
    public ITestService TestService { get; set; }
    public TestContext(ITestService testService)
    {
        TestData = "123";
        TestService = testService;
    }
}