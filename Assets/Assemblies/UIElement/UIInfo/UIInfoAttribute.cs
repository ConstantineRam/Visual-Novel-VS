using System;

[AttributeUsage(AttributeTargets.Class)]
public class UIInfoAttribute : Attribute, NullObject
{
    public static UIInfoAttribute CreateNullObject()
        => new UIInfoAttributeNullObject();
    public string Path { get; }

    public UIInfoAttribute( string path)
    {
        Path = path;
    }

    public bool IsNullObject { get; protected set; } = false;
}

public class UIInfoAttributeNullObject : UIInfoAttribute
{
    public UIInfoAttributeNullObject() : base(string.Empty)
    {
        IsNullObject = true;
    }
}