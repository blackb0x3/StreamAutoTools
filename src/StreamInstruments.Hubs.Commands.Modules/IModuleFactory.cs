namespace StreamInstruments.Hubs.Commands.Modules;

public interface IModuleFactory
{
    IModule GetModule(string moduleName);
}