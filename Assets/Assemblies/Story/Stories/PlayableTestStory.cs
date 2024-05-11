public enum TestStoryBackgrounds
{
    Background0,
    Background1
}

public enum TestStoryCharacter
{
    CharacterKitsune,
    CharacterReaper,
    CharacterOtherGuy,
    
}
[UIInfo("TestStory/PlayableTestStory")]
public class PlayableTestStory : Story<PlayableTestStory, TestStoryBackgrounds, TestStoryCharacter>
{

}

