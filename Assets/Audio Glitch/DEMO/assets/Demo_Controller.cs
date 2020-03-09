using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Demo_Controller : MonoBehaviour
{

	public AudioGlitch audio_glitch;
	public Button btn_start;
	public Button btn_end;


	void Start ()
	{
		btn_start.onClick.AddListener (()=>{	audio_glitch.Start_Process();	});
		btn_end.onClick.AddListener   (()=>{	audio_glitch.Stop_Process();    });
	}

}
