using OpenPay.Interface;

namespace OpenPay.Core.Services;

internal class TestService : ITestService
{
    public string Test()
    {
        return "test";
    }
}