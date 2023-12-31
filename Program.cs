using IMUS_Coursework.Models;
using IMUS_Coursework.Services;

namespace IMUS_Coursework;

public class Program
{
    public static void Main()
    {
        var petriNet = new PetriNet(new PetriParameters
        {
            MaxQueueLength = 5,
            NumOfProcessingUnits = 4,
            ClientsIntensity = 0.5,
            ProcessingIntensity = 0.1,
            ImitationTime = 480,
            DisplayAllProcesses = false
        });
        
        petriNet.Run();
    }
}