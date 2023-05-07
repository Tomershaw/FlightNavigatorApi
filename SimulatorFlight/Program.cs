using SimulatorFlight.Model;
using System.Net.Http.Json;

public class Program
{
    static readonly HttpClient client = new() { BaseAddress = new Uri("https://localhost:7088") };
    static void Main(string[] args)
    {
        var timer = new System.Timers.Timer(5000);
        timer.Elapsed += (s, e) => CreateFlight();
        timer.Elapsed += (s, e) => ChangeTimerInterval(s!);
        timer.Start();
        Console.WriteLine("simulator started");
        Console.ReadKey();
    }

    private static async void CreateFlight()
    {
        var flight = new FlightDto();

        Console.WriteLine($"Flight sent:");
        Console.WriteLine($"{flight}");
        try
        {
            var response = await client.PostAsJsonAsync("/api/ApiControllerFlights", flight);
            Console.WriteLine("Respnse:");
            Console.WriteLine(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static void ChangeTimerInterval(object source)
    {
        var timer = source as System.Timers.Timer;
        Random rnd = new Random();
        timer!.Interval = rnd.Next(5000, 10000);
    }


}