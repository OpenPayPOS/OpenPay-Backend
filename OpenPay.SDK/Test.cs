using OpenPay.Interface;

namespace OpenPay.SDK;

public interface ITestContext
{
    public string TestData { get; set; }
    public ITestService TestService { get; set; }
}

public abstract class PluginBase : IPlugin
{
    
    protected ITestContext? _testContext;

    public virtual string Name { get; }

    public void Init(ITestContext context)
    {
        _testContext = context;
    }

    public abstract int Run();
}

public interface IPlugin
{
    public string Name { get; }

    public void Init(ITestContext context);

    public int Run();
}