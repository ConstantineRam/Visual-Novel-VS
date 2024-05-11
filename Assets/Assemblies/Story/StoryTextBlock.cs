using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public readonly struct StoryTextBlockData
{
    public readonly Translator Translator;
    public readonly TalkBit TalkBit;

    public StoryTextBlockData(Translator translator, TalkBit talkBit)
    {
        Translator = translator;
        TalkBit = talkBit;
    }
}

public abstract class StoryTextBlock : MonoBehaviour, TextBased
{
  [Required][SerializeField] private TextMeshProUGUI textBox;

  private StoryTextBlockData _data;
  public bool IsInitialized { get; private set; }


  public abstract Type SupportedTalkBit();

  //private string _currentTextID;
  

   private void DisplayText()
   {
       textBox.text = _data.Translator.Translate(_data.TalkBit.GetLocalizationId ?? string.Empty);
   }

   private void Awake()
   {
      AssignedScriptValidation.Validate(this);
   }

   
   public void Initialize(object data)
   {
       if (data is not StoryTextBlockData textBlockData)
       {
         throw new Exception($" {this.name} got {data.TypeAsStringOrNull()} as data, should be {typeof(StoryTextBlockData)}");
         return;
       }

       _data = textBlockData;
       IsInitialized = true;
   }

   public string GetLocalizationId => _data.TalkBit.GetLocalizationId ?? TextBased.NoID;
   public string GetShownString => (textBox != null ? textBox.text : TextBased.MissingTextField) ?? TextBased.NoLocalization;
}
