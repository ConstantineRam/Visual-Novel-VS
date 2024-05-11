using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public readonly struct StoryData
{
    public StoryData(Translator translator)
    {
        Translator = translator;
    }

    public Translator Translator { get; }
}

public class Story<T,TBack, TCharacter> : UIElement<T> where T : UIElement where TBack : Enum where TCharacter : Enum
{
   [HorizontalLine(color: EColor.Green)]
   [SerializeField] private EnumMap<TBack, SceneBackground> backgroundLibrary;

   [SerializeField] internal BackgroundSlot backgroundParent;
   
   [HorizontalLine(color: EColor.Green)]
   [SerializeField] private EnumMap<TCharacter, Character> charactersLibrary;
   [SerializeField] internal CharacterSlot charactersParent;
   
    public int GetBackgroundsAmount => backgroundLibrary.Count();
    public int GetCharacterAmount => charactersLibrary.Count();
    internal SceneBackground GetCurrentBackground { get; private set; }
    internal Character GetCurrentCharacter { get; private set; }

    private StoryData _storyData;
    
    private IEnumerable<TBack> _backgroundKeyEnumValues;

    private IEnumerable<TBack> BackgroundKeyEnumValues
    {
        get
        {
            if (_backgroundKeyEnumValues != null) return _backgroundKeyEnumValues;
            
            _backgroundKeyEnumValues = Enum.GetValues(typeof(TBack)) as IEnumerable<TBack>;
            if (_backgroundKeyEnumValues == null)
            {
                throw new Exception($"Wrong type for {typeof(TBack)}");
            }

            return _backgroundKeyEnumValues;
        }
    }

    #region Test Related

    internal int BackgroundsCount => backgroundLibrary.Count();

    #endregion

    #region Unity Event Methods

    private void Awake()
    {
        AssignedScriptValidation.Validate(this);
        
        if (!IsAllBackgroundsAssigned())
        {
            throw new Exception($"{name} has missing backgrounds assigned.");
        }

        if (backgroundParent == null)
        {
            throw new Exception($"{name} has missing {typeof(BackgroundSlot)} assigned.");
        }

        if (charactersParent == null)
        {
            throw new Exception($"{name} has missing {typeof(CharacterSlot)} assigned.");
        }
        AssignDefaultBackground();
    }

    private void OnValidate() 
    {
        backgroundLibrary.TryRevise();
        charactersLibrary.TryRevise();
    }

    private void Reset() 
    {
        backgroundLibrary.TryRevise();
        charactersLibrary.TryRevise();
    }      

    #endregion

    /// <summary>
    /// Sets the background with the given key.
    /// </summary>
    /// <param name="back">The key of the background to set.</param>
    protected internal void SetBackground(TBack back)
    {
        if (GetCurrentBackground != null)
        {
            Destroy(GetCurrentBackground);
            GetCurrentBackground = null;
        }

        var backPrefab = backgroundLibrary[back];
        GetCurrentBackground = Instantiate(backPrefab, backgroundParent.transform);     
    }

    protected internal void SetCharacter(TCharacter character)
    {
        if (GetCurrentCharacter != null)
        {
            Destroy(GetCurrentCharacter);
            GetCurrentCharacter = null;
        }

        var characterPrefab = charactersLibrary[character];
        GetCurrentCharacter = Instantiate(characterPrefab, charactersParent.transform);     
    }    
    
    private void AssignDefaultBackground()
    {
        SetBackground(BackgroundKeyEnumValues.First());
    }

    internal bool IsAllCharactersAssigned()
    {
        if (!charactersLibrary.Any())
        {
            Debug.LogError($"Story '{GetType()}' has no {typeof(Character)} set.");
            return false;
        }
        
        foreach (var key in  Enum.GetValues(typeof(TCharacter)) )
        {
            if (key is not TCharacter value)
            {
                throw new Exception($"Unexpected Error enum with non {typeof(TCharacter)} type was used for {GetType()}");
            }

            if (charactersLibrary[value] == null) return false;
        }

        return true;      
    }

    /// <summary>
    /// Determines if all backgrounds have been assigned. </summary> <returns>
    /// Returns true if all backgrounds are assigned, otherwise false. </returns>
    /// /
    internal bool IsAllBackgroundsAssigned()
    {
        
        if (!backgroundLibrary.Any())
        {
            Debug.LogError($"Story '{GetType()}' has no {typeof(SceneBackground)} set.");
            return false;
        }
        
        foreach (var key in  Enum.GetValues(typeof(TBack)) )
        {
            if (key is not TBack value)
            {
                throw new Exception($"Unexpected Error enum with non {typeof(TBack)} type was used for {GetType()}");
            }

            if (backgroundLibrary[value] == null) return false;
        }

        return true;
    }

    
    protected override async Task OnInitialize(CancellationToken ct)
    {
        await base.OnInitialize(ct);

        if (_data is not StoryData storyData)
        {
            throw new Exception($"{this.name} got data {_data.TypeAsStringOrNull()}, but {typeof(StoryData)} expected.");
        }

        _storyData = storyData;
    }
}

