using UnityEngine;
using UnityEditor;
using System;

public class TaskTimer : EditorWindow {

	bool showReviewMessage;

	string countDown;
	TimeSpan t;
	DateTime daysLeft;
	DateTime startDate;

	public int hours = 1;
	public int minutes = 30;

	bool isCounting = false;
	string deadline;

	[MenuItem("Window/Timers/Task")]
	static void Init()
	{
		TaskTimer tt = (TaskTimer)EditorWindow.GetWindow (typeof(TaskTimer));
		tt.Show ();

		EditorPrefs.SetString ("TaskInProgress", "NA");
	}

	void OnGUI()
	{
		if (!isCounting && EditorPrefs.GetString ("TaskInProgress") == "NA") {
			ShowSetup();
		} else if (!isCounting && EditorPrefs.GetString ("TaskInProgress") != "NA") {
			ResumeTime();
			isCounting = true;
		} else if (isCounting) {
			ShowTimer();
		}
	}

	void OnInspectorUpdate()
	{
		if (isCounting) {	
			if(daysLeft.ToString().Contains("1/1/0001"))
				daysLeft = DateTime.Parse (deadline);

			startDate = DateTime.Now;
			t = daysLeft - startDate;
			countDown = string.Format ("{0} : {1} : {2}", t.Hours, t.Minutes, t.Seconds);

			Repaint ();
		}
	}

	void ShowSetup()
	{
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
		hours = EditorGUILayout.IntField ("Hours: ", hours);
		minutes = EditorGUILayout.IntField ("Minutes: ", minutes);
		
		if (GUILayout.Button (new GUIContent ("Start Timer", "This starts the timer"))) {
			GetTime ();
			isCounting = true;
		}
	}
	
	void ShowTimer()
	{
		GUILayout.BeginVertical ();
		GUILayout.FlexibleSpace ();
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.Label ("Hours : Minutes : Seconds", EditorStyles.boldLabel);
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.Label (countDown, EditorStyles.boldLabel);
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button (new GUIContent ("<<", "Subtract an hour.")))
			RemoveHour ();
		if (GUILayout.Button (new GUIContent ("<", "Subtract 30 minutes.")))
			RemoveHalf ();
		if (GUILayout.Button (new GUIContent ("X", "End the Timer."))) {
			isCounting = false;
			EditorPrefs.SetString ("TaskInProgress", "NA");
		}
		if (GUILayout.Button (new GUIContent (">", "Add 30 minutes.")))
			AddHalf ();
		if (GUILayout.Button (new GUIContent (">>", "Add an hour.")))
			AddHour ();
		GUILayout.EndHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.FlexibleSpace ();
		GUILayout.EndVertical ();
	}


	DateTime savedTime;
	void GetTime()
	{
		DateTime Now = DateTime.Now;
		TimeSpan Later = new TimeSpan (hours, minutes, 0);
		DateTime Result = Now.Add (Later);
		savedTime = Result;

		deadline = Result.Month + "/" + Result.Day + "/" + Result.Year + " " + Result.Hour + ":" + Result.Minute + ":" + Result.Second; 
		EditorPrefs.SetString ("TaskInProgress", deadline);
		daysLeft = DateTime.Parse (deadline);
	}

	void AddHour()
	{
		TimeSpan NewHour = new TimeSpan (1, 0, 0);
		DateTime Result = savedTime.Add (NewHour);
		savedTime = Result;

		deadline = Result.Month + "/" + Result.Day + "/" + Result.Year + " " + Result.Hour + ":" + Result.Minute + ":" + Result.Second; 
		EditorPrefs.SetString ("TaskInProgress", deadline);
		daysLeft = DateTime.Parse (deadline);
	}

	void RemoveHour()
	{
		TimeSpan NewHour = new TimeSpan (1, 0, 0);
		DateTime Result = savedTime.Subtract (NewHour);
		savedTime = Result;
		
		deadline = Result.Month + "/" + Result.Day + "/" + Result.Year + " " + Result.Hour + ":" + Result.Minute + ":" + Result.Second; 
		EditorPrefs.SetString ("TaskInProgress", deadline);
		daysLeft = DateTime.Parse (deadline);
	}

	void AddHalf()
	{
		TimeSpan NewHalfHour = new TimeSpan (0, 30, 0);
		DateTime Result = savedTime.Add (NewHalfHour);
		savedTime = Result;
		
		deadline = Result.Month + "/" + Result.Day + "/" + Result.Year + " " + Result.Hour + ":" + Result.Minute + ":" + Result.Second; 
		EditorPrefs.SetString ("TaskInProgress", deadline);
		daysLeft = DateTime.Parse (deadline);
	}

	void RemoveHalf()
	{
		TimeSpan NewHalfHour = new TimeSpan (0, 30, 0);
		DateTime Result = savedTime.Subtract (NewHalfHour);
		savedTime = Result;
		
		deadline = Result.Month + "/" + Result.Day + "/" + Result.Year + " " + Result.Hour + ":" + Result.Minute + ":" + Result.Second; 
		deadline = EditorPrefs.GetString ("TaskInProgress");
		daysLeft = DateTime.Parse (deadline);
	}

	void ResumeTime()
	{
		deadline = EditorPrefs.GetString ("TaskInProgress");
		daysLeft = DateTime.Parse (deadline);
	}

	void OnFocus()
	{
		if (EditorPrefs.GetInt ("EditorTimersReview", 0) >= 0)
			EditorPrefs.SetInt ("EditorTimersReview", EditorPrefs.GetInt ("EditorTimersReview", 0) + 1);

		if (EditorPrefs.GetInt ("EditorTimersReview", 0) >= 50)
			showReviewMessage = true;
	}
}