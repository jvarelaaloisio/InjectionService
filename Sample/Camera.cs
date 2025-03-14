namespace VarelaAloisio.InjectionService.Sample;
public interface ICamera;

public class Camera : ICamera
{
    public override string ToString()
        => nameof(Camera);
}

public class IsoCamera : Camera
{
    public override string ToString()
        => nameof(IsoCamera);
}