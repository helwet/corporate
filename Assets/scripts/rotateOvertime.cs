using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateOvertime : MonoBehaviour
{

	[SerializeField] private Vector3 _rotation = new Vector3(0,0,20);
	[SerializeField] private float _speed = 1;

	void Update() {
		transform.Rotate(_rotation * _speed * Time.deltaTime);
	}
}
