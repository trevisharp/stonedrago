using System;

namespace Stonedrago.Exceptions;

public class InvalidPGNException(string command) : Exception
{
    public override string Message => 
    $"""
        A invalid PGN (Portable Game Notation) was received.
        The PGN contains a invalid command "{command}".
    """;
}