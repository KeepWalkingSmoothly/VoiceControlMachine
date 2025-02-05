using Android.Media;
using Xamarin.Forms;
using Speech_2._0.Droid;

[assembly: Dependency(typeof(AudioRender))]
namespace Speech_2._0.Droid
{
    public class AudioRender: IAudioService
    {
        public void PlayAudioFile(string filename)
        {
            var player = new MediaPlayer();
            var file = global::Android.App.Application.Context.Assets.OpenFd(filename); 
            player.SetDataSource(file.FileDescriptor, file.StartOffset, file.Length);
            player.Prepared += (s, e) =>
            {
                player.Start();
            };
            player.Prepare();
        }
    }
}