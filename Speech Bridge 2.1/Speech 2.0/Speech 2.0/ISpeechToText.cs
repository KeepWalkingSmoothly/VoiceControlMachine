using System.Threading.Tasks;
public interface ISpeechToText
{
    Task<SpeechToTextResult> SpeechToTextAsync();
}