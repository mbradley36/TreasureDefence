using System;
using System.Collections;
using UnityEngine;
 
/// <summary>
/// Wrapper around coroutines that allows them to be stopped manually at a later time
/// </summary>
public class StoppableCoroutine : IEnumerator
{
	// Wrapped generator method
	protected IEnumerator generator;
 
	public StoppableCoroutine(IEnumerator generator)
	{
		this.generator = generator;
	}
 
	// Stop the coroutine form being called again
	public void Stop()
	{
		generator = null;
	}
 
	// IEnumerator.MoveNext
	public bool MoveNext()
	{
		if (generator != null) {
			return generator.MoveNext();
		} 
		else {
			return false;
		}
	}
 
	// IEnumerator.Reset
	public void Reset()
	{
		if (generator != null) {
			generator.Reset();
		}
	}
 
	// IEnumerator.Current
	public object Current {
		get {
			if (generator != null) {
				return generator.Current;
			} 
			else {
				throw new InvalidOperationException();
			}
		}
	}
}