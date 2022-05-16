namespace Core.BinarySerializer;

public abstract class BinaryBase
{
    public const int EmptyArrayHeader = -1;
    private readonly Stream _baseStream;
    private readonly int _initialStreamCapacity;

    public BinaryBase(Stream baseStream, int initialStreamCapacity)
    {
        _baseStream = baseStream;
        _initialStreamCapacity = initialStreamCapacity;
    }

    public void Reset()
    {
        _baseStream.Position = 0L;
        _baseStream.SetLength(0L);
        if (_baseStream is MemoryStream memoryStream)
        {
            memoryStream.Capacity = _initialStreamCapacity;
        }
    }

    public void ResetLengthAndPosition()
    {
        _baseStream.Position = 0L;
        _baseStream.SetLength(0L);
    }
        
    public void ResetPosition()
    {
        _baseStream.Position = 0L;
    }

    public long CurrentPosition()
    {
        return _baseStream.Position;
    }
}