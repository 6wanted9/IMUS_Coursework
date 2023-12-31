using IMUS_Coursework.Models;

namespace IMUS_Coursework.Services;

class PetriNet
{
    private const int TimeUnitMs = 100;
    private readonly int _maxQueueLength;
    private readonly int _numOfProcessingUnits;
    private readonly int _clientsIntensity;
    private readonly int _processingIntensity;
    private readonly int _imitationTime;
    private readonly bool _displayAllProcesses;

    private int _clientsInQueue;
    private int _occupiedProcessingUnits;
    
    private int _possibleClients;
    private int _missedClients;
    private int _processedClients;
    
    private readonly object _lockObject = new();
    private bool _stopTransitions;
    private readonly List<int> _queryLengths = new();

    public PetriNet(PetriParameters parameters)
    {
        _maxQueueLength = parameters.MaxQueueLength;
        _numOfProcessingUnits = parameters.NumOfProcessingUnits;
        _clientsIntensity = parameters.ClientsIntensity;
        _processingIntensity = parameters.ProcessingIntensity;
        _imitationTime = parameters.ImitationTime;
        _displayAllProcesses = parameters.DisplayAllProcesses;

        _clientsInQueue = 0;
        _occupiedProcessingUnits = 0;
        _missedClients = 0;
        _processedClients = 0;
    }
    
    public void Run()
    {
        var clientsIntensityTimeout = TimeUnitMs / _clientsIntensity;
        var processingIntensityTimeout = TimeUnitMs / (_processingIntensity * _numOfProcessingUnits);
        var minTimeout = Math.Min(clientsIntensityTimeout, processingIntensityTimeout);
        var transition1 = GenerateThread(Transition1, clientsIntensityTimeout);
        var transition2 = GenerateThread(Transition2, minTimeout);
        var transition3 = GenerateThread(Transition3, minTimeout);
        var transition4 = GenerateThread(Transition4, processingIntensityTimeout);
        var transition5 = GenerateThread(Transition5, clientsIntensityTimeout);

        StartTransitions(transition1, transition2, transition3, transition4, transition5);
        Thread.Sleep(TimeUnitMs * _imitationTime);
        StopTransition();
        DisplayResult();
    }

    private void Transition1()
    {
        lock (_lockObject)
        {
            if (_possibleClients == 0)
            {
                DisplayIfApplied("Transition 1: New client arrived.");
                _possibleClients++;
            }
        }
    }
    
    private void Transition2()
    {
        lock (_lockObject)
        {
            _queryLengths.Add(_clientsInQueue);
            if (_possibleClients > 0 && _clientsInQueue < _maxQueueLength)
            {
                DisplayIfApplied("Transition 2: Moved to queue.");
                _possibleClients--;
                _clientsInQueue++;
            }
        }
    }

    private void Transition3()
    {
        lock (_lockObject)
        {
            if (_clientsInQueue > 0 && _occupiedProcessingUnits < _numOfProcessingUnits)
            {
                DisplayIfApplied("Transition 3: Started processing.");
                _clientsInQueue--;
                _occupiedProcessingUnits++;
            }
        }
    }

    private void Transition4()
    {
        lock (_lockObject)
        {
            if (_occupiedProcessingUnits > 0)
            {
                DisplayIfApplied("Transition 4: Finished processing.");
                _occupiedProcessingUnits--;
                _processedClients++;
            }
        }
    }
    
    private void Transition5()
    {
        lock (_lockObject)
        {
            if (_possibleClients > 0 && _clientsInQueue == _maxQueueLength)
            {
                DisplayIfApplied("Transition 5: Skipped a client.");
                _possibleClients--;
                _missedClients++;
            }
        }
    }

    private Thread GenerateThread(Action transition, int timeout)
    {
        return new Thread(() =>
        {
            while (!_stopTransitions)
            {
                transition();
                Thread.Sleep(timeout);
            }
        });
    }
    
    private static void StartTransitions(params Thread[] threads)
    {
        foreach (var thread in threads)
        {
            thread.Start();
        }
    }

    private void StopTransition()
    {
        _stopTransitions = true;
    }
    
    private void DisplayResult()
    {
        var generalAmountOfClients = _processedClients + _missedClients + _clientsInQueue;
        var chanceOfProcessing = generalAmountOfClients == 0 ? 0 : (double) _processedClients / generalAmountOfClients;
        Console.WriteLine($"General amount of clients: {generalAmountOfClients}");
        Console.WriteLine($"Processed clients: {_processedClients}");
        Console.WriteLine($"Missed clients: {_missedClients}");
        Console.WriteLine($"Clients remaining in queue: {_clientsInQueue}");
        Console.WriteLine($"Chance of processing: {chanceOfProcessing}");
        Console.WriteLine($"Average query length: {_queryLengths.Sum() / _queryLengths.Count}");
    }

    private void DisplayIfApplied(string text)
    {
        if (_displayAllProcesses)
        {
            Console.WriteLine(text);
        }
    }
}