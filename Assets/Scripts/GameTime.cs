﻿using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour {

	public static GameTime Instance;

	public System.Action OnGameEnded;

	private Game game;
	public float totalTime = 60f;
	public float timePassed;

	public float totalFileDestroyTime = 1f;

	void Awake(){
		Instance = this;
	}


	void Start(){
		game = Game.Instance;
	}

	void Update(){
		timePassed += Time.deltaTime;
		if(timePassed >= totalTime){
			StartCoroutine(EndGame());
		}
	}

	public float GetTimePercentage(){
		return timePassed/totalTime;
	}

	private IEnumerator EndGame(){
		print ("wait for it...");
		int bestPlayer = game.GetWinner();

		if(OnGameEnded != null) OnGameEnded();

		yield return new WaitForSeconds(2f);

		print ("Player " + bestPlayer + " won!");
	}
}