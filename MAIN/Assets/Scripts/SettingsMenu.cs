using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        // will make the game fullscreen when option is ticked
    }
    
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        // will access the all the available resolutions
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height )
        {
            currentResolutionIndex = i;
        }
            // will change the resolution when the option is ticked
            // will show the available options of resolutions
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        // will change the resolution to the selected option
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
