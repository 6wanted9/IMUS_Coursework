namespace IMUS_Coursework.Models;

public class PetriParameters
{
    public int MaxQueueLength { get; init; }
    public int NumOfProcessingUnits { get; init; }
    public double ClientsIntensity { get; init; }
    public double ProcessingIntensity { get; init; }
    public int ImitationTime { get; init; }
    public bool DisplayAllProcesses { get; init; }
}