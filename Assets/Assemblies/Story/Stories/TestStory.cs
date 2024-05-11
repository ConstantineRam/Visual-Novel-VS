
[UIInfo("TestStory/Story")]
public class TestStory : Story<TestStory, TestStoryBackgrounds, TestStoryCharacter>
{
   public static StoryData TestStoryData = new StoryData(new TranslatorStub());

}


public class TestStoryNoAttribute : Story<TestStoryNoAttribute, TestStoryBackgrounds, TestStoryCharacter>
{

}