using VarelaAloisio.InjectionService.Runtime;

namespace VarelaAloisio.InjectionService.Sample;

/*
 * This example has been created by Juan Pablo Varela.
 * GitHub: https://github.com/jvarelaaloisio
 */
public static class Program
{
    public static void Main(params string[] args)
    {
        /*
         * Let's say we have an injectable class that we want to set up before using.
         * We'll use the car class, which has 2 floats with IDs and a camera with empty ID to inject.
         */
        var myCar = new Character();

        /* You can add this to a ServiceLocator/ServiceProvider. */
        IInjectionService injectionService = new Runtime.InjectionService();
        injectionService.AddOrReplace<Character>("", null);

        /* Let's add the camera reference, we can just leave the id empty if we're going to use this as the default option. */
        var camera = new IsoCamera();
        injectionService.TryAdd<ICamera>(string.Empty, camera);

        /* We can also add the camera with a specific ID, just in case we change our minds in the future and have multiple cameras loaded. */
        injectionService.TryAdd<ICamera>("Camera", camera);

        /*
         * We can also add multiple references or values with the AddRange method. This can make it easier to dump a lot of data at once
         * (For example, when we download settings from an endpoint).
         */
        injectionService.TryAddRange<float>(new Dictionary<string, object> { { "Car Max Health Point", 10f }, { "Car travel distance", 999f }});

        /* We can now inject our car with all it's dependencies. */
        var result = injectionService.Inject(myCar);

        /*
         * We can expect this output:
         * Character injected values:
               HealthPoints: 10
               MaxTravelDistance: 999
               Camera: IsoCamera
         */
        if (result is Character car)
            car.ReportValues(Console.Out);

        /*
         * Sometimes we have a custom script that inherits from a common used class.
         * Some examples are Enemies being Characters or, for this use case, the Camera is also an IsoCamera.
         * In this example, the Car only needs an ICamera and doesn't care about the implementation of it.
         * On the other hand, we have an IsoElement that has an InjectedField for an IsoCamera, so the ICamera ref won't be picked up.
         * To be able to get to the IsoCamera without mapping it again, we can just create a variant (variants are just a copy of the dictionary, but with a different key).
         */
        injectionService.TryAddVariant<ICamera, IsoCamera>();

        /* Now we can create our IsoElement and inject it. */
        var isoElement = new IsoElement("IsoElement");

        /* We don't need to use the result value of the injection, since it's done in the isoElement we give it as a parameter. */
        injectionService.Inject(isoElement);

        /*
         * We can expect this output:
         * IsoElement's injected values:
               Camera: IsoCamera
         */
        isoElement.ReportValues(Console.Out);

        /* We can replace the camera if needed. */
        injectionService.AddOrReplace<ICamera>("Camera", new Camera());

        /* Let's create a new car but change the id for its camera value. */
        var secondCar = new Character { Camera = { Id = "Camera" } };
        injectionService.Inject(secondCar);

        /*
         * We can expect this output:
         * Character injected values:
               HealthPoints: 10
               MaxTravelDistance: 999
               Camera: Camera
         */
        secondCar.ReportValues(Console.Out);
    }
}