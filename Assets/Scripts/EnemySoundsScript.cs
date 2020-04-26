using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Sid
{
    public class EnemySoundsScript : MonoBehaviour
    {
        public AudioClip[] walkClips;
        public AudioClip[] rollClips;
        public AudioClip deactivationClip, activationClip, alarmSound;
        public AudioSource legSource;
        public float volume, waitForSeconds;
        private NavMeshAgent agent;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void playWalkSound()
        {
            AudioClip clip = GetRandomClip(walkClips);
            if(agent.velocity != Vector3.zero)
            {
                legSource.Stop();
                legSource.loop = false;
                legSource.PlayOneShot(clip, volume);
                //AudioSource.PlayClipAtPoint(clip, agent.transform.position);
            }
        }

        private void playRollSound()
        {
            AudioClip clip = GetRandomClip(rollClips);
            legSource.Stop();
            legSource.loop = false;
            legSource.PlayOneShot(clip, volume);
            //AudioSource.PlayClipAtPoint(clip, agent.transform.position);
        }

        private void playDeactivationSound()
        {
            legSource.Stop();
            legSource.loop = false;
            legSource.PlayOneShot(deactivationClip, volume);
            StartCoroutine(SmallDelay());
            playAlarmSound();
            //AudioSource.PlayClipAtPoint(deactivationClip, agent.transform.position);
        }

        private void playActivationSound()
        {
            legSource.Stop();
            legSource.loop = false;
            legSource.PlayOneShot(activationClip, volume);
            //AudioSource.PlayClipAtPoint(deactivationClip, agent.transform.position);
        }

        private void playAlarmSound()
        {
            legSource.Stop();
            legSource.loop = true;
            legSource.clip = alarmSound;
            legSource.volume = volume;
            legSource.Play();
        }

        private AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips[UnityEngine.Random.Range(0, clips.Length)];
        }

        private IEnumerator SmallDelay()
        {
            yield return new WaitForSeconds(waitForSeconds);
        }
    }
}
