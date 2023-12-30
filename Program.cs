using IMUS_Coursework.Models;
using IMUS_Coursework.Services;

namespace IMUS_Coursework;

public class Program
{
    public static void Main()
    {
        var petriNet = new PetriNet(new PetriParameters
        {
            MaxQueueLength = 6,
            NumOfProcessingUnits = 1,
            ClientsIntensity = 2,
            ProcessingIntensity = 1,
            ImitationTime = 50,
            DisplayAllProcesses = false
        });
        
        petriNet.Run();
    }
}