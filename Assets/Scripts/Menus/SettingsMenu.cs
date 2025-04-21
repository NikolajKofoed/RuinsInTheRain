using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingsMenu : MonoBehaviour
{
	public AudioMixer audioMixer;
	public TMP_Dropdown resolutionDropdown;
	Resolution[] resolutions;
	List<string> options = new List<string>();

	void Start()
	{

        resolutions = Screen.resolutions;

		resolutionDropdown.ClearOptions();

		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " X " + resolutions[i].height;
			options.Add(option);

			if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
			{
				currentResolutionIndex = i;
			}
		}

		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue();
	}

	public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}
}
