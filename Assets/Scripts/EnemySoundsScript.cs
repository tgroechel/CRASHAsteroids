using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sid
{
    public class EnemySoundsScript : MonoBehaviour
    {
        public AudioClip[] walkClips;
        public AudioClip[] rollClips;
        public AudioSource legSource;

        private void playWalkSound()
        {
            AudioClip clip = GetRandomClip(walkClips);
            legSource.PlayOneShot(clip);
        }

        private void playRollSound()
        {
            AudioClip clip = GetRandomClip(rollClips);
            legSource.PlayOneShot(clip);
        }

        private AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips[UnityEngine.Random.Range(0, clips.Length)];
        }
    }
}
