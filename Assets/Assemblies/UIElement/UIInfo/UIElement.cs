using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class UIElement : MonoBehaviour, NullObject, Initializable
{

    protected object _data;
    public bool IsInitialized { get; private set; }

    /// <summary>
    /// Initializes the story asynchronously. Overwrite this class for any specific story based initialization.
    /// </summary>
    /// <param name="ct">The cancellation token used to cancel the initialization process.</param>
    /// <returns>A task representing the initialization operation.</returns>
    protected virtual async Task OnInitialize(CancellationToken ct) { }

    public async Task Initialize(object data, CancellationToken ct)
    {
        _data = data;
        await OnInitialize(ct);
        IsInitialized = true;
    }     
    
    public static async Task<UIElement> CreateNullObject(CancellationToken ct)
    {
        var res = await ResourceLoader.Load<UIElementNullObject>("Core/NullObject").ConfigureAwait(true);

        if (res != null) return res;
        Debug.LogError($"Unexpected Error. Can't create null object for {typeof(UIElement)}.");
        return null;
    }

    public bool IsNullObject { get; protected set; } = false;
}

public abstract class UIElement<T> : UIElement where T : UIElement
{
   // ReSharper disable once StaticMemberInGenericType
    private static UIElement _prefab;
    public static async Task<UIElement> Load(CancellationToken ct, RectTransform parent, object data)
    {
        if (_prefab != null) return Instantiate(_prefab, parent);
        _prefab = await LoadPrefabAsResource(ct);
        var result = _prefab.IsNullObject ? await CreateNullObject(ct) : Instantiate(_prefab, parent);

        if (result.IsNullObject)
        {
            Debug.LogError($"Can't create {typeof(T)}  prefab is {_prefab == null}.");
            return result;
        }

        await result.Initialize(data, ct);
        return result;
    }
    internal static UIInfoAttribute GetUIInfo()
        => typeof(T).GetCustomAttribute<UIInfoAttribute>() ?? UIInfoAttribute.CreateNullObject();

    // ReSharper disable once StaticMemberInGenericType
    private static UIElement _resource = default;
    internal static async Task<UIElement> LoadPrefabAsResource(CancellationToken ct)
    {
        if (_resource == null)
        {
            var att = GetUIInfo();
            if (att.IsNullObject)
            {
                Debug.LogError($"{typeof(T)} has no {typeof(UIInfoAttribute)} attached.");
                return await UIElement.CreateNullObject(ct);
            }
            _resource = await ResourceLoader.Load<UIElement>(att.Path).ConfigureAwait(true);    
        }

        if (_resource != null) return _resource;
        
        Debug.LogError($"Can't load resource for {typeof(T)}, no {typeof(UIInfoAttribute)} attached.");
        return  await UIElement.CreateNullObject(ct);
    }
}


