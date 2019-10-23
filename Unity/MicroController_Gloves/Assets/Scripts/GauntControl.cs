using UnityEngine;
using UnityEngine.UI;

public class GauntControl : MonoBehaviour
{
	private Animation gauntletAnim;

	public Slider thumb_slider;
	public Slider index_slider;
	public Slider middle_slider;
	public Slider ring_slider;
	public Slider pinky_slider;

	void Start()
	{
		gauntletAnim = GetComponent<Animation>();

		gauntletAnim["Thumb"].layer = 5;
		gauntletAnim["Index_Finger"].layer = 1;
		gauntletAnim["Middle_Finger"].layer = 2;
		gauntletAnim["Ring_Finger"].layer = 3;
		gauntletAnim["Pinky_Finger"].layer = 4;

		gauntletAnim.Play("Thumb");
		gauntletAnim.Play("Index_Finger");
		gauntletAnim.Play("Middle_Finger");
		gauntletAnim.Play("Ring_Finger");
		gauntletAnim.Play("Pinky_Finger");

		gauntletAnim["Thumb"].speed = 0;
		gauntletAnim["Index_Finger"].speed = 0;
		gauntletAnim["Middle_Finger"].speed = 0;
		gauntletAnim["Ring_Finger"].speed = 0;
		gauntletAnim["Pinky_Finger"].speed = 0;
		
	}

	void Update()
	{
		gauntletAnim["Thumb"].normalizedTime = thumb_slider.normalizedValue;
		gauntletAnim["Index_Finger"].normalizedTime = index_slider.normalizedValue;
		gauntletAnim["Middle_Finger"].normalizedTime = middle_slider.normalizedValue;
		gauntletAnim["Ring_Finger"].normalizedTime = ring_slider.normalizedValue;
		gauntletAnim["Pinky_Finger"].normalizedTime = pinky_slider.normalizedValue;
	}
}
