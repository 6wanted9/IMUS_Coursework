namespace IMUS_Coursework.Models;

public class PetriParameters
{
    public int MaxQueueLength { get; init; }
    public int NumOfProcessingUnits { get; init; }
    public int ClientsIntensity { get; init; }
    public int ProcessingIntensity { get; init; }
    public int ImitationTime { get; init; }
    public bool DisplayAllProcesses { get; init; }
}