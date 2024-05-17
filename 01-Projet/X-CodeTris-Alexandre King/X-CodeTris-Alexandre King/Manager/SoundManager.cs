using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    /// <summary>
    /// Manager of the sound. Everything that is sound related is managed here
    /// </summary>
    static public class SoundManager
    {
        //Variables
        static SoundPlayer _soundPlayer = new SoundPlayer();
        static string _musicPath = "../../music/";
        static string _tetrisSongFile = "TetrisSong.wav";
        static string _supsensSongFile = "suspens.wav";
        //string _QVGDMFile = "TetrisSong.wav"; //musique de qui veut gagner des millions        

        /// <summary>
        /// Play the Tetris theme song
        /// </summary>
        static public void PlayTetrisThemeSong()
        {
            _soundPlayer.SoundLocation = _musicPath + _tetrisSongFile;
            _soundPlayer.PlayLooping();
        }
        /// <summary>
        /// Play the suspens
        /// </summary>
        static public void PlaySuspensSong()
        {
            _soundPlayer.SoundLocation = _musicPath + _supsensSongFile;
            _soundPlayer.PlayLooping();
        }

        /// <summary>
        /// Stops the music
        /// </summary>
        static public void StopMusic()
        {
            _soundPlayer.Stop();
        }
    }
}
