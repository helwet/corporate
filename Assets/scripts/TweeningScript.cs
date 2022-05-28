using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using System.Reflection;

public enum EasingTypes{
		EaseLinear,EaseAnimationCurve,EaseSpring,
		EaseInQuad,EaseOutQuad,EaseInOutQuad,
		EaseInCubic,EaseOutCubic,EaseInOutCubic,
		EaseInQuart,EaseOutQuart,EaseInOutQuart,
		EaseInQuint,EaseOutQuint,EaseInOutQuint,
		EaseInSine,EaseOutSine,EaseInOutSine,
		EaseInExpo,EaseOutExpo,EaseInOutExpo,
		EaseInCirc,EaseOutCirc,EaseInOutCirc,
		EaseInBounce,EaseOutBounce,EaseInOutBounce,
		EaseInBack,EaseOutBack,EaseInOutBack,
		EaseInElastic,EaseOutElastic,EaseInOutElastic,
		EasePunch,EaseShake,
	};

public class TweeningScript : MonoBehaviour
{
	public AnimationCurve animationCurve;
	public GameObject animated;

	public EasingTypes easing;
	public LeanTweenType easeType;
	//public UIAnimationsTypes animType;
	public float duration = 5;
	public float delay = 0;

	public bool loop = false;
	public bool pingpong = false;

	public Vector3 from;
	public Vector3 to;


	private Vector3 startLocation;

    // Start is called before the first frame update
    void Start()
    {
		startLocation = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void demoEaseTypes() {
			string easeName = easeTypes[0];
		int lineDrawScale = 10;
			Transform obj1 = GameObject.Find(easeName).transform.Find("Line");
			float obj1val = 0f;
			LTDescr lt = LeanTween.value(obj1.gameObject, 0f, 1f, 5f).setOnUpdate((float val) => {
				Vector3 vec = obj1.localPosition;
				vec.x = obj1val * lineDrawScale;
				vec.y = val * lineDrawScale;

				obj1.localPosition = vec;

				obj1val += Time.deltaTime / 5f;
				if (obj1val > 1f)
					obj1val = 0f;
			});
			if (easeName.IndexOf("AnimationCurve") >= 0) {
				lt.setEase(animationCurve);
			}
			else {
				MethodInfo theMethod = lt.GetType().GetMethod("set" + easeName);
				theMethod.Invoke(lt, null);
			}

			if (easeName.IndexOf("EasePunch") >= 0) {
				lt.setScale(1f);
			}
			else if (easeName.IndexOf("EaseOutBounce") >= 0) {
				lt.setOvershoot(2f);
			}
		LeanTween.delayedCall(gameObject, 10.1f, demoEaseTypes);
	}

	private string[] easeTypes = new string[]{
		"EaseLinear","EaseAnimationCurve","EaseSpring",
		"EaseInQuad","EaseOutQuad","EaseInOutQuad",
		"EaseInCubic","EaseOutCubic","EaseInOutCubic",
		"EaseInQuart","EaseOutQuart","EaseInOutQuart",
		"EaseInQuint","EaseOutQuint","EaseInOutQuint",
		"EaseInSine","EaseOutSine","EaseInOutSine",
		"EaseInExpo","EaseOutExpo","EaseInOutExpo",
		"EaseInCirc","EaseOutCirc","EaseInOutCirc",
		"EaseInBounce","EaseOutBounce","EaseInOutBounce",
		"EaseInBack","EaseOutBack","EaseInOutBack",
		"EaseInElastic","EaseOutElastic","EaseInOutElastic",
		"EasePunch","EaseShake",
	};

	/*
	// Rotate Example
		LeanTween.rotateAround( avatarRotate, Vector3.forward, 360f, 5f);

		// Scale Example
		LeanTween.scale( avatarScale, new Vector3(1.7f, 1.7f, 1.7f), 5f).setEase(LeanTweenType.easeOutBounce);
		LeanTween.moveX( avatarScale, avatarScale.transform.position.x + 5f, 5f).setEase(LeanTweenType.easeOutBounce); // Simultaneously target many different tweens on the same object 

		// Move Example
		LeanTween.move( avatarMove, avatarMove.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInQuad);

		// Delay
		LeanTween.move( avatarMove, avatarMove.transform.position + new Vector3(-6f, 0f, 1f), 2f).setDelay(3f);

		// Chain properties (delay, easing with a set repeating of type ping pong)
		LeanTween.scale( avatarScale, new Vector3(0.2f, 0.2f, 0.2f), 1f).setDelay(7f).setEase(LeanTweenType.easeInOutCirc).setLoopPingPong( 3 );
	
		// Call methods after a certain time period
		LeanTween.delayedCall(gameObject, 0.2f, advancedExamples);*/
}
