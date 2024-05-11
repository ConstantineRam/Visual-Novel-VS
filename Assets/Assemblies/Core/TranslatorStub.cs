
public class TranslatorStub : Translator
{
    public const string TranslationMark = "(translated)";
    public string Translate(in string toTranslate)
        => string.Concat(toTranslate, TranslationMark); 
    
}
