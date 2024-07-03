using OpenPay.SDK;

namespace OpenPay.TestPlugin;

public class TestPlugin : PluginBase
{
    public override string Name { get { return "TestPlugin"; } }

    public override int Run()
    {
        string result = _testContext.TestService.Test();
        Console.WriteLine(result);
        return 0;
    }
}