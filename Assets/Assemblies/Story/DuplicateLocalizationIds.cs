using System.Collections.Generic;
using System.Linq;

/// <summary>
/// List of Localization Ids' that are to be ignored when duplicates are checked.
/// </summary>
public class DuplicateLocalizationIds
{
    private static readonly HashSet<string> Allowed = new HashSet<string>()
    {
        "Yes", "No", "Ok"
    };

    public static bool IsAllowed(string id) => Allowed.Any(i => i == id);
}
