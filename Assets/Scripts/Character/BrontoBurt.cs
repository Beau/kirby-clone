﻿using UnityEngine;
using System.Collections;

public class BrontoBurt : EnemyBase {

	public float period = 1f;
	public float amplitude = 3f;
	public float xVelocity = 2f;

	private float startTime;
	private bool flyAway;

	private enum State {
		Begin, FastFlap, SlowFlap, Fall
	}

	#region implemented abstract members of EnemyBase

	protected override void goToDefaultState()
	{
		if (dir == Direction.Left) {
			xVelocity *= -1;
		}
		updateXVelocity(xVelocity);
		CurrentState = State.Begin;
	}

	#endregion

	public IEnumerator BeginEnterState() {
		startTime = Time.time;
		StartCoroutine(SineWaveStrategy());
		yield return null;
	}

	public IEnumerator SineWaveStrategy() {
		CurrentState = State.Fall;
		yield return new WaitForSeconds(period/2);
		CurrentState = State.FastFlap;
		yield return new WaitForSeconds(period);
		for (int i = 0; i < 2; i++) {
			CurrentState = State.Fall;
			yield return new WaitForSeconds(period);
			CurrentState = State.FastFlap;
			yield return new WaitForSeconds(period);
		}
		flyAway = true;
		CurrentState = State.FastFlap;
	}
	
	#region FastFlap
	
	public void FastFlapUpdate () {
		if (flyAway) {
			updateYVelocity(rigidbody2D.velocity.y + Time.deltaTime * 2);
		} else {
			float t = Time.time - startTime;
			updateYVelocity(amplitude * Mathf.Sin(t * Mathf.PI / period) * -1);
		}
	}

	#endregion

	#region FastFlap
	
	public void FallUpdate () {
		float t = Time.time - startTime;
		updateYVelocity(amplitude * Mathf.Sin(t * Mathf.PI / period) * -1);
	}
	#endregion
	
}

