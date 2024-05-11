using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class AssignedScriptValidation
{
    public static void Validate(MonoBehaviour check)
    {
#if !UNITY_EDITOR
        return;
#endif 
        
        
        if (check == null)
        {
            Debug.LogError("");
            return;
        }

        var name = check.name;
        var root = check.transform;
        var parent = root.parent;
        while (parent != null)
        {
            name = string.Concat(parent.name, "/", name);
            root = parent;
            parent = root.parent;
        }
        
        var fields = check.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(f => !f.FieldType.IsValueType && HasRequiredAttributes(f.GetCustomAttributes(typeof(SerializeField), false) ) );
 
        foreach (var info in fields)
        {
            if (info.GetValue(check) == null)
                Debug.LogError($" {check.name} in {name} has unsigned field {info.Name} marked as SerializedField");
        }
    }

    private static bool HasRequiredAttributes(IEnumerable<object> attrs, bool checkForRequired = false)
    {
        foreach (var i in attrs )
        {
            if (i is SerializeField sf)
                return true;
        }

        return false;
    }
}