using IMUS_Coursework.Models;
using IMUS_Coursework.Services;

namespace IMUS_Coursework;

public class Program
{
    public static void Main()
    {
        var petriNet = new PetriNet(new PetriParameters
        {
            MaxQueueLength = 25,
            NumOfProcessingUnits = 1,
            ClientsIntensity = 0.033,
            ProcessingIntensity = 0.033,
            ImitationTime = 1440,
            DisplayAllProcesses = false
        });
        
        petriNet.Run();
    }
}