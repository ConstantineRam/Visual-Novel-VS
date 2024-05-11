using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

public static class ResourceLoader 
{
    public static async Task<Object> Load(string path)
        => await Resources.LoadAsync(path) as GameObject;

    public static async Task<T> Load<T>(string path) where T : Object
        => await Resources.LoadAsync<T>(path) as T;


}