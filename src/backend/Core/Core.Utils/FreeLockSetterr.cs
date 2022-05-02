namespace Core.Utils;

public class FreeLockSetterr<T> where T:class
{
    private volatile T _value;

    public FreeLockSetterr(T value)
    {
        _value = value;
    }

    public T Value => _value;

    public T Swap(Func<T, T> func)
    {
        T original, temp;
        do
        {
            original = _value;
            temp = func(original);
        } while (Interlocked.CompareExchange(ref original, temp, _value) != original);

        return original;
    }
}