using OpenPay.Core;
using OpenPay.Core.Services;
using OpenPay.Interface;
using OpenPay.SDK;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;

ITestService testService = new TestService();


ITestContext context = new TestContext(testService);

try
{
    string[] pluginPaths = Directory.GetFiles(Path.GetFullPath(Path.Combine(
        Path.GetDirectoryName(
            Path.GetDirectoryName(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(typeof(Program).Assembly.Location))))), "plugins")));

    IEnumerable<IPlugin> plugins = pluginPaths.SelectMany(pluginPath =>
    {
        Assembly pluginAssembly = LoadPlugin(pluginPath);
        return CreatePlugin(pluginAssembly);
    }).ToList();

    // Load commands from plugins.

    if (args.Length == 0)
    {
        Console.WriteLine("Plugins: ");
        foreach (IPlugin plugin in plugins)
        {
            Console.WriteLine(plugin.Name);
            plugin.Init(context);
            plugin.Run();
        }
    }
    else
    {
        foreach (string commandName in args)
        {
            Console.WriteLine($"-- Called: {commandName} --");

            IPlugin? plugin = plugins.FirstOrDefault(c => c.Name == commandName);
            if (plugin == null)
            {
                Console.WriteLine("plugin unknown");
                return;
            }

            plugin.Init(context);
            Console.WriteLine();
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

static Assembly LoadPlugin(string path)
{
        Console.WriteLine($"Loading plugins from: {path}");
    PluginLoadContext context = new PluginLoadContext(path);
    return context.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(path)));
}

static IEnumerable<IPlugin> CreatePlugin(Assembly assembly)
{
    int count = 0;

    foreach (Type type in assembly.GetTypes())
    {
        if (typeof(IPlugin).IsAssignableFrom(type))
        {
            IPlugin result = Activator.CreateInstance(type) as IPlugin;
            if (result != null)
            {
                count++;
                yield return result;
            }
        }
    }

    if (count == 0)
    {
        string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.Name));
        throw new ApplicationException(
            $"Can't find any type which implements ICommand in {assembly} from {assembly.Location}.\n" +
            $"Available types: {availableTypes}");
    }
}