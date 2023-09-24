using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadbobSystem : MonoBehaviour {

	[Header("Walking Bob Settings")]
	[Range(0.001f, 0.01f)]
	public float Amount = 0.002f;
	[Range(1f, 30f)] 
	public float Frequency = 10.0f;
	[Range(10f, 100f)] 
	public float Smooth = 10.0f;

    [Header("Running Bob Settings")]
    [Range(0.001f, 0.01f)]
    public float RunAmount = 0.002f;
    [Range(1f, 30f)]
    public float RunFrequency = 10.0f;
    [Range(10f, 100f)]
    public float RunSmooth = 10.0f;

	public bool isRunning;
    Vector3 StartPos;
	
	void Start() {
    	
		StartPos = transform.localPosition;
    }

	void Update() {
    	
	    CheckForHeadbobTrigger();
		StopHeadbob();
	    
    }
    
	private void CheckForHeadbobTrigger() {
        	
		float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;
	        
		if (inputMagnitude > 0) {
		    if(isRunning)	
				StartHeadBobRunning();
			else
				StartHeadBob();
		}
	}
        
	private Vector3 StartHeadBob() {
        	
		Vector3 pos = Vector3.zero;
		pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);
		pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);
		transform.localPosition += pos;
		    
		return pos;
	}

    private Vector3 StartHeadBobRunning()
    {

        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * RunFrequency) * RunAmount * 1.4f, RunSmooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * RunFrequency / 2f) * RunAmount * 1.6f, RunSmooth * Time.deltaTime);
        transform.localPosition += pos;

        return pos;
    }

    private void StopHeadbob() {
        	
		if (transform.localPosition == StartPos) return;
		transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, 1 * Time.deltaTime);
		
	}
}
