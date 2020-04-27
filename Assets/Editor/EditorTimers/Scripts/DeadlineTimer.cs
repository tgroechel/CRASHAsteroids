using UnityEngine;
using UnityEditor; 
using System;  

[Serializable]
public class DeadlineTimer : EditorWindow {
	bool showReviewMessage; 
	   
	string countDown; 
	DateTime daysLeft;
	DateTime startDate;
	TimeSpan t;
	
	int year;

	public enum Month
	{
		Janurary, February, March,
		April, May, June,
		July, August, September,
		October, November, December
	};
	[SerializeField]
	public Month month;
	int monthNum = 1;
	[SerializeField]
	public int day = DateTime.Now.Day;
	[SerializeField]
	public bool PM = true;  
	[SerializeField] 
	string pm;
	[SerializeField]
	public int hour;
	string deadline;
	
	bool isCounting = false;

	[MenuItem("Window/Timers/Deadline")]
	static void Init()
	{
		//Get existing open window or if none, make a new one:
		DeadlineTimer dt = (DeadlineTimer)EditorWindow.GetWindow (typeof(DeadlineTimer));
		dt.Show ();

		EditorPrefs.SetString ("DeadlineInProgress", "NA");
		if (dt.isCounting == true)
			Debug.Log ("Called");
	}

	void OnGUI()
	{
		if (!isCounting && EditorPrefs.GetString ("DeadlineInProgress") == "NA") {
			if (showReviewMessage) {
				EditorGUILayout.HelpBox ("I apologize for the prompt, but if you could just take a second to review Editor Timers it really helps me out. And thank you for your support.", MessageType.Warning, true);
				EditorGUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Sure!")) {
					showReviewMessage = false;
					EditorPrefs.SetInt ("EditorTimersReview", -1);
					Application.OpenURL ("http://u3d.as/QYf");
				}
				if (GUILayout.Button ("No, thanks.")) {
					showReviewMessage = false;
					EditorPrefs.SetInt ("EditorTimersReview", -1);
				}
				EditorGUILayout.EndHorizontal ();
				GUILayout.Space (10);
			}

			GUILayout.Label ("Timer Setup", EditorStyles.boldLabel);
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ();
			GUILayout.Label ("Month");
			month = (Month)EditorGUILayout.EnumPopup ("", month, GUILayout.MaxWidth (80));
			GUILayout.EndVertical ();
			GUILayout.BeginVertical ();
			GUILayout.Label ("Day");
			day = EditorGUILayout.IntField ("", day, GUILayout.MaxWidth (30));
			GUILayout.EndVertical ();
			GUILayout.BeginVertical ();
			GUILayout.Label ("Hour");
			hour = EditorGUILayout.IntField ("", hour, GUILayout.MaxWidth (30));
			GUILayout.EndVertical ();
			GUILayout.BeginVertical ();
			GUILayout.Label ("PM?");
			PM = EditorGUILayout.Toggle ("", PM);
			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

			if (GUILayout.Button ("Start Timer")) {
				GetMonth ();
				GetTime ();
				isCounting = true;
			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndVertical (); 
		} else if (!isCounting && EditorPrefs.GetString ("DeadlineInProgress") != "NA") {
			ResumeTime(); 
			isCounting = true;
		} else if(isCounting) {
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace(); 
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Days : Hours : Minutes : Seconds", EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label (countDown, EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			if(GUILayout.Button ("Clear Countdown")){
				isCounting = false;
				EditorPrefs.SetString ("DeadlineInProgress", "NA");
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
		}
	}

	void OnInspectorUpdate()
	{
		if (isCounting) {
			if(daysLeft.ToString().Contains("1/1/0001"))
				daysLeft = DateTime.Parse (deadline);

			startDate = DateTime.Now;
			t = daysLeft - startDate; 
			countDown = string.Format ("{0} : {1} : {2} : {3}", t.Days, t.Hours, t.Minutes, t.Seconds);
			Repaint ();
		}
	}
	
	void GetTime()
	{
		if (PM)
			pm = "PM";
		else
			pm = "AM";
		
		deadline = monthNum + "/" + day + "/" + DateTime.Now.Year + " " + hour + ":00" + ":00 " + pm; 
		EditorPrefs.SetString ("DeadlineInProgress", deadline);
		daysLeft = DateTime.Parse (deadline);
	}

	void ResumeTime()
	{
		deadline = EditorPrefs.GetString ("DeadlineInProgress");
		daysLeft = DateTime.Parse (deadline);
	}

	void GetMonth()
	{
		switch (month)
		{
		case Month.Janurary:
			monthNum = 1;
			break;
		case Month.February:
			monthNum = 2;
			break;
		case Month.March:
			monthNum = 3;
			break;
		case Month.April:
			monthNum = 4;
			break;
		case Month.May:
			monthNum = 5;
			break;
		case Month.June:
			monthNum = 6;
			break;
		case Month.July:
			monthNum = 7;
			break;
		case Month.August:
			monthNum = 8;
			break;
		case Month.September:
			monthNum = 9;
			break;
		case Month.October:
			monthNum = 10;
			break;
		case Month.November:
			monthNum = 11;
			break;
		case Month.December:
			monthNum = 12;
			break;
		}
	}

	void OnFocus()
	{
		if (EditorPrefs.GetInt ("EditorTimersReview", 0) >= 0)
			EditorPrefs.SetInt ("EditorTimersReview", EditorPrefs.GetInt ("EditorTimersReview", 0) + 1);
		
		if (EditorPrefs.GetInt ("EditorTimersReview", 0) >= 50)
			showReviewMessage = true;
	}
}