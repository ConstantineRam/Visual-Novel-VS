using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;

public readonly struct ChatData
{

    public ChatData(Translator translator)
    {
        Translator = translator;
    }

    public Translator Translator { get; }

    internal static ChatData TestChatData = new ChatData(new TranslatorStub());
}

public class Chat : MonoBehaviour
{
    
    [HorizontalLine(color: EColor.Blue)]
    [Required][SerializeField] private RectTransform scrollViewContent;
    [Required][SerializeField] private CharacterTalkTextBlock CharacterTextBlockPrefab;

    private readonly List<StoryTextBlock> _textBlocksPrefabs = new List<StoryTextBlock>();
    
    private readonly List<StoryTextBlock> _activeTextBlocks = new List<StoryTextBlock>(); 
    private ChatData Data { get; set; }
    public bool IsInitialized { get; private set; }

    private void Awake()
    {
        AssignedScriptValidation.Validate(this);
        
        _textBlocksPrefabs.Add(CharacterTextBlockPrefab);
    }

    internal void Initialize(ChatData data)
    {
        Data = data;
        IsInitialized = true;
    }

    public void Push(Talk talk)
    {
        
        if (talk == null)
        {
            Debug.LogError($"Null at talk");
            return;
        }

        foreach (var talkBit in talk.Enumerate())  
        {
            Push(talkBit);
        }
    }

    public void Push(TalkBit newBit)
    {
        if (newBit == null)
        {
            Debug.LogError($"Null at newBit");
            return;
        }

        StoryTextBlock go = default;
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var textBlock in _textBlocksPrefabs)
        {
            if (newBit.GetType() != textBlock.SupportedTalkBit()) continue;
            go = Instantiate(textBlock, scrollViewContent);
            break;
        }
        

        if (go == default)
        {
            Debug.LogError($"Unable to create game object for {newBit.GetType()}");
            return;
        }
        go.Initialize(new StoryTextBlockData(Data.Translator, newBit) );
        _activeTextBlocks.Add(go);
    }

    public int Count()
        => _activeTextBlocks.Count();
}
