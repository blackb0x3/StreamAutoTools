using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StreamInstruments.Hubs.Commands.Modules.ModuleImplementations;

public abstract class ModuleBase
{
    protected static readonly Random Rng = new();

    public virtual Modules Type => Modules.Dummy;

    public virtual async Task<string> ExecuteFunctionAsync(string functionName, string[] functionParams)
    {
        var type = GetType();
        var method = type.GetMethod(functionName);

        if (method == null)
        {
            throw new Exception($"Function {functionName} does not exist on module {Type.ToString()}");
        }

        var isAsync = method.ReturnType == typeof(Task<string>);

        var result = isAsync
            ? await ExecuteAsync(method, functionParams)
            : ExecuteSync(method, functionParams);

        return result;
    }

    private string ExecuteSync(MethodInfo method, string[] functionParams)
    {
        return method.Invoke(this, functionParams).ToString();
    }

    private async Task<string> ExecuteAsync(MethodInfo method, string[] functionParams)
    {
        dynamic task = method.Invoke(this, functionParams);
        object result = await task;

        return result.ToString();
    }
}