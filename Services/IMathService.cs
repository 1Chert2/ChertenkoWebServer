namespace ChertenkoWebServer.Services;

public interface IMathService
{
    double Add(double first, double second);

    bool TryDivide(double dividend, double divisor, out double result);
}
