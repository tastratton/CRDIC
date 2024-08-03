using System;

namespace OldGreeter;

public interface IGreeter
{
    void Greet(string name) => Console.WriteLine($"Hello, {name ?? "anonymous"}");
}
