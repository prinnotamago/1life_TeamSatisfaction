using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GyroDebugLog : MonoBehaviour {

    private string _gyro;
    private Text _debugLog;

    private void Awake() { Input.gyro.enabled = true; }

    private void Start () {
        _debugLog = GetComponent<Text>();

        _debugLog.text = "test";
	}
	
	// Update is called once per frame
	private void Update () {
        //       _gyro = "Input.gyro.rotationRate:" + Input.gyro.rotationRate +
        //"\nInput.gyro.rotationRateUnbiased:" + Input.gyro.rotationRateUnbiased +
        //    "\nInput.gyro.gravity:" + Input.gyro.gravity +
        //    "\nInput.gyro.useAcceleration:" + Input.gyro.userAcceleration +
        //    "\nInput.gyro.attitude:" + Input.gyro.attitude +
        //    "\nInput.gyro.enabled:" + Input.gyro.enabled +
        //    "\nInput.gyro.updateInterval:" + Input.gyro.updateInterval;

        _gyro = "\nInput.gyro.gravity:" + Input.gyro.gravity;

        _debugLog.text = _gyro;
    }
}
