using VarelaAloisio.InjectionService.Runtime;

namespace VarelaAloisio.InjectionService.Sample;

public class IsoElement(string name) : IInjectable
{
    public string Name { get; } = name;
    public InjectedField<IsoCamera> Camera { get; } = new();
    public void HandleInjection() { }
    public void ReportValues(TextWriter log)
        => log.WriteLine($"{Name}'s injected values:"
                         + $"\n\t{nameof(Camera)}: {Camera.Value}");
}