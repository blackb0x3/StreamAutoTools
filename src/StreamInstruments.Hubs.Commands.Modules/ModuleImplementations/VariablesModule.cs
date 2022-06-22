using System.Globalization;
using Microsoft.EntityFrameworkCore;
using StreamInstruments.DataAccess;
using StreamInstruments.DataObjects;
using StreamInstruments.Extensions;

namespace StreamInstruments.Hubs.Commands.Modules.ModuleImplementations;

public class VariablesModule : ModuleBase, IModule
{
    private readonly StreamInstrumentsContext _context;

    public VariablesModule(StreamInstrumentsContext context)
    {
        _context = context;
    }

    public override Modules Type => Modules.Variable;

    public async Task<string> Get(string variableName)
    {
        var variable = await FindVariableAsync(variableName);

        return variable.RawValue;
    }

    public async Task<string> Set(string variableName, string newValue)
    {
        var variable = await FindVariableAsync(variableName);

        if (variable.Type is not (VariableType.Text or VariableType.Custom))
        {
            // TODO
            // log warning that directly setting variable that is not
            // text or custom can cause intended behaviour to break stuff
        }

        variable.RawValue = newValue;

        await _context.SaveChangesAsync(default);

        return variable.RawValue;
    }

    public async Task<string> Increment(string variableName)
    {
        var variable = await FindVariableAsync(variableName);

        if (variable.Type != VariableType.Number)
        {
            throw new Exception($"Variable {variableName} cannot be incremented! It is a {variable.Type.ToDescription()} type!");
        }

        if (!int.TryParse(variable.RawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedInt))
        {
            throw new Exception($"Variable {variableName} cannot be incremented! '{variable.RawValue}' cannot be interpreted as an integer!");
        }

        variable.RawValue = (++parsedInt).ToString();

        await _context.SaveChangesAsync(default);

        return variable.RawValue;
    }

    public async Task<string> Decrement(string variableName)
    {
        var variable = await FindVariableAsync(variableName);

        if (variable.Type != VariableType.Number)
        {
            throw new Exception($"Variable {variableName} cannot be decremented! It is a {variable.Type.ToDescription()} type!");
        }

        if (!int.TryParse(variable.RawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedInt))
        {
            throw new Exception($"Variable {variableName} cannot be decremented! '{variable.RawValue}' cannot be interpreted as an integer!");
        }

        variable.RawValue = (--parsedInt).ToString();

        await _context.SaveChangesAsync(default);

        return variable.RawValue;
    }

    public async Task<string> Toggle(string variableName)
    {
        var variable = await FindVariableAsync(variableName);

        if (variable.Type != VariableType.Boolean)
        {
            throw new Exception($"Variable {variableName} cannot be toggled! It is a {variable.Type.ToDescription()} type!");
        }

        if (!bool.TryParse(variable.RawValue, out var parsedBool))
        {
            throw new Exception($"Variable {variableName} cannot be incremented! '{variable.RawValue}' cannot be interpreted as an integer!");
        }

        variable.RawValue = (!parsedBool).ToString();

        await _context.SaveChangesAsync(default);

        return variable.RawValue;
    }

    private async Task<Variable> FindVariableAsync(string variableName)
    {
        var variable = await _context.Variables
            .FirstOrDefaultAsync(v => string.Equals(v.Name, variableName, StringComparison.OrdinalIgnoreCase));

        if (variable is null)
        {
            throw new Exception($"Variable {variableName} does not exist!");
        }

        return variable;
    }
}