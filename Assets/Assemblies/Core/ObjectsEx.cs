
public static class ObjectsEx
{
    public static string TypeAsStringOrNull(this object data)
        => data == null ? $"Null" :  data.GetType().ToString();
}
