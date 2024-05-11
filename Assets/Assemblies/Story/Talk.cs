using System.Collections.Generic;

public interface Talk
{
    IReadOnlyCollection<TalkBit> Enumerate();
}
