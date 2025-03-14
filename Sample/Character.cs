using VarelaAloisio.InjectionService.Runtime;

namespace VarelaAloisio.InjectionService.Sample;

public class Character : IInjectable
{
    public InjectedField<float> MaxHealthPoints { get; } = new(){Id = "Car Max Health Point"};
    public InjectedField<float> MaxTravelDistance { get; } = new(){Id = "Car travel distance"};
    //Since we don't specify an ID, the value for it will be the default ("", an empty string)
    public InjectedField<ICamera> Camera { get; } = new();

    public float HealthPoints { get; private set; } = -1;
    public void HandleInjection()
        => HealthPoints = MaxHealthPoints;

    public void ReportValues(TextWriter log)
        => log.WriteLine($"{nameof(Character)} injected values:"
                         + $"\n\t{nameof(HealthPoints)}: {HealthPoints}"
                         + $"\n\t{nameof(MaxTravelDistance)}: {MaxTravelDistance.Value}"
                         + $"\n\t{nameof(Camera)}: {Camera.Value}");
}