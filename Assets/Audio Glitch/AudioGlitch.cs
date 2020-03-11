using UnityEngine;
using UnityEngine.Events;
using System.Collections;


[ExecuteInEditMode()]
[AddComponentMenu("Audio/Audio Glitch")]
public class AudioGlitch : MonoBehaviour 
{
	
	[Space(10)][Header("Main Audio Settings")]
	[SerializeField()] AudioClip main_clip;
	[Tooltip("The Main audio volume. (not affect on Glitches)")]
	[SerializeField()] [Range(0,1)] float volume = 1;
	[Tooltip("Processing automatically, when application Start.")]
	[SerializeField()] bool play_at_start = false;
	[Tooltip("Continue playing the Main audio, while any Glitch is playing.")]
	[SerializeField()] bool continue_when_glitching = true;

	[Space(10)][Header("Glitch Audio Settings")]
	[SerializeField()] _Glitch[] glitches;

	[Space(10)][Header("Events")]
	[Tooltip("Is called, when the Main audio is starting. (every time when Glitch ends)")]
	[SerializeField()] UnityEvent OnNormalPlay;
	[Tooltip("Is called, when the Glitch audio starts.")]
	[SerializeField()] UnityEvent OnGlitchPlay;
	[Tooltip("Is called, when the Process finish.")]
	[SerializeField()] UnityEvent OnFinish;


	// private fields
	float pause_time;
	bool in_process;
	[SerializeField()][HideInInspector()] AudioSource a_sourceMain;
	[SerializeField()][HideInInspector()] AudioSource a_sourceGlitch;
	[SerializeField()][HideInInspector()] bool is_created;







#region PUBLIC
	public void Start_Process()
	{
		if(!in_process) // Don't start again, while in process
			StartCoroutine ("Process");
	}

	public void Stop_Process()
	{
		StopCoroutine ("Process");

		a_sourceMain.Stop ();
		a_sourceGlitch.Stop ();

		if (OnFinish != null)
			OnFinish.Invoke ();
		
		in_process = false;
	}
#endregion


#region PRIVATE
	void Start ()
	{
		if (!Application.isPlaying)
			return;

		if(play_at_start)
			Start_Process ();
	}

	IEnumerator Process()
	{
		in_process = true;

		a_sourceMain.clip = main_clip;
		a_sourceMain.volume = volume;
		a_sourceMain.Play ();

		if (OnNormalPlay != null)
			OnNormalPlay.Invoke ();
		

		for (int i = 0; i < glitches.Length; i++) 
		{
			float current_wait_time = 0;
			if(i == 0)
				current_wait_time = glitches[i].start_time;
			else
				current_wait_time = glitches[i].start_time - glitches[i-1].start_time;

			yield return new WaitForSeconds(current_wait_time);

			a_sourceGlitch.clip = glitches[i].clip;
			a_sourceGlitch.time = 0;
			a_sourceGlitch.volume = glitches[i].volume;
			a_sourceGlitch.Play ();

			if (!continue_when_glitching)
			{
				pause_time = a_sourceMain.time;
				a_sourceMain.Stop ();
			}
			else
				a_sourceMain.volume = glitches[i].main_volume;

			if (OnGlitchPlay != null)
				OnGlitchPlay.Invoke ();

			float delta = -1;
			if(i != glitches.Length - 1)
			delta = glitches[i].clip.length - (glitches[i+1].start_time - glitches[i].start_time);
			if(delta < 0)
				delta = 0;

			if((i != glitches.Length - 1  &&  delta == 0) || i == glitches.Length - 1)
			{
				yield return new WaitForSeconds(glitches[i].clip.length);

				if(!continue_when_glitching)
				{
					a_sourceMain.time = pause_time;
					a_sourceMain.volume = volume;
					a_sourceMain.Play ();
				}
				else
					a_sourceMain.volume = volume;


				if (OnNormalPlay != null)
					OnNormalPlay.Invoke ();
			}

		}

		yield return new WaitForSeconds(a_sourceMain.clip.length - a_sourceMain.time);

		if (OnFinish != null)
			OnFinish.Invoke ();

		in_process = false;
	}
#endregion



#region Glitch Class
	[System.Serializable]
	class _Glitch
	{
		[Tooltip("Audio Clip of this Glitch.")]
		public AudioClip clip;
		[Tooltip("A delay from start, when this Glitch will play.")]
		public float start_time;
		[Range(0,1)][Tooltip("The volume of this Glitch.")]
		public float volume = 1;
		[Range(0,1)][Tooltip("The volume of your main audio, while this Glitch is playing.")]
		public float main_volume = 0.3f;
	}
#endregion



#if UNITY_EDITOR
	void OnEnable()
	{
		if (Application.isPlaying)
			return;
		if (is_created)
			return;
		
		a_sourceMain = gameObject.AddComponent<AudioSource> ();
		a_sourceGlitch = gameObject.AddComponent<AudioSource> ();
		a_sourceMain.playOnAwake = false;
		a_sourceGlitch.playOnAwake = false;

		is_created = true;
	}
#endif

}