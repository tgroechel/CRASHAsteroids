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
        public bool isBoss;
        private GameObject locationEffectsKuri; // Destination indicator for Kuri
        private NavMeshAgent agent;
        public float kuriDestinationOffset;
        public static bool kuriDestinationCalculated;
        public static Vector3 kuriDestination;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            if(isBoss)
            {
                locationEffectsKuri = GameObject.Find("LocationEffects").transform.GetChild(1).gameObject;
                kuriDestinationCalculated = false; // Single point of destination access for all scripts
            }
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

            // Wait till boss' velocity becomes zero before you execute do other stuff
            if(isBoss)
                StartCoroutine(WaitForBossStop());
        }

        private void playActivationSound()
        {
            if(isBoss)
            {
                // Stop detination effects for Kuri
                locationEffectsKuri.SetActive(false);
                kuriDestinationCalculated = false;
            }

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

        private IEnumerator WaitForBossStop()
        {
            // Wait for boss' velocityto become zero
            while (agent.velocity != Vector3.zero)
                yield return null;

            // Show destination indicator for Kuri using effects
            kuriDestination = transform.position + (Camera.main.transform.forward.normalized * kuriDestinationOffset);
            kuriDestination.y = transform.position.y;
            locationEffectsKuri.transform.position = kuriDestination;
            locationEffectsKuri.SetActive(true);
            kuriDestinationCalculated = true;

            // Play alarm sound to hint the player at moving Kuri
            playAlarmSound();
        }
    }
}
