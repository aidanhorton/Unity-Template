using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{
    private const string masterVolumeParameter = "MasterVolume";
    private const string qualityLevelPref = "QualityLevel";
    private const string fullScreenPref = "FullScreen";
    private const string screenResolutionPref = "ScreenResolution";
    private const string fovSliderPref = "FOV";
    private const string SensitivitySliderPref = "Sensitivity";

    public bool MenuIsOpen { get; private set; }

    [Header("Menu Components")]
    [SerializeField] private GameObject menuParent;

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private GameObject resolutionHeader;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider fovSlider;
    [SerializeField] private Slider sensitivitySlider;

    public bool IsDesktopBuild;
    public bool IsMainMenuSettings;

    [SerializeField] private MonoBehaviour[] ScriptsToDisable;

    private Resolution[] screenResolutions;

    protected override void Awake()
    {
        base.Awake();
        
        this.volumeSlider.onValueChanged.AddListener(this.SetMasterVolume);
        this.qualityDropdown.onValueChanged.AddListener(this.SetQuality);
        this.fullscreenToggle.onValueChanged.AddListener(this.SetFullScreen);
        this.resolutionDropdown.onValueChanged.AddListener(this.SetResolution);
        this.fovSlider.onValueChanged.AddListener(this.SetFov);
        this.sensitivitySlider.onValueChanged.AddListener(this.SetSensitivity);

        this.screenResolutions = Screen.resolutions;
        
        if (!this.IsDesktopBuild)
        {
            this.resolutionHeader.SetActive(false);
            this.resolutionDropdown.gameObject.SetActive(false);
            this.fullscreenToggle.gameObject.SetActive(false);
        }

        this.menuParent.SetActive(false);
    }
    
    private void Start()
    {
        // Set default values.
        this.volumeSlider.value = PlayerPrefs.GetFloat(masterVolumeParameter, this.volumeSlider.value);
        this.SetMasterVolume(this.volumeSlider.value);

        this.qualityDropdown.value = PlayerPrefs.GetInt(qualityLevelPref, this.qualityDropdown.value);
        this.SetQuality(this.qualityDropdown.value);

        this.fullscreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt(fullScreenPref));
        this.SetFullScreen(this.fullscreenToggle.isOn);

        this.fovSlider.value = PlayerPrefs.GetFloat(fovSliderPref, this.fovSlider.value);
        this.SetFov(this.fovSlider.value);

        this.sensitivitySlider.value = PlayerPrefs.GetFloat(SensitivitySliderPref, this.sensitivitySlider.value);
        this.SetSensitivity(this.sensitivitySlider.value);

        // Select resolution dropdown.
        var resolutionOptions = new List<string>();
        var currentResolutionIndex = 0;
        for (var i = 0; i < this.screenResolutions.Length; i++)
        {
            resolutionOptions.Add($"{this.screenResolutions[i].width} x {this.screenResolutions[i].height}");

            // Set current resolution to be selected in dropdown.
            if (this.screenResolutions[i].width == Screen.currentResolution.width && this.screenResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        if (PlayerPrefs.HasKey(screenResolutionPref))
        {
            currentResolutionIndex = PlayerPrefs.GetInt(screenResolutionPref);
        }

        this.resolutionDropdown.ClearOptions();
        this.resolutionDropdown.AddOptions(this.screenResolutions.Select(resolution => $"{resolution.width} x {resolution.height}").ToList());
        this.resolutionDropdown.value = currentResolutionIndex;
        this.resolutionDropdown.RefreshShownValue();

        this.SetResolution(this.resolutionDropdown.value);
    }

    public void ShowSettings()
    {
        this.menuParent.SetActive(true);
    }

    public void HideSettings()
    {
        this.menuParent.SetActive(true);
    }
    
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(masterVolumeParameter, this.volumeSlider.value);
        PlayerPrefs.SetInt(qualityLevelPref, this.qualityDropdown.value);
        PlayerPrefs.SetInt(fullScreenPref, Convert.ToInt32(this.fullscreenToggle.isOn));
        PlayerPrefs.SetInt(screenResolutionPref, this.resolutionDropdown.value);
        PlayerPrefs.SetFloat(fovSliderPref, this.fovSlider.value);
        PlayerPrefs.SetFloat(SensitivitySliderPref, this.sensitivitySlider.value);
    }

    private void Update()
    {
        if (this.IsMainMenuSettings) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.menuParent.activeSelf)
            {
                // Disable menu.
                this.MenuIsOpen = false;

                //Time.timeScale = 1;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                foreach (var script in this.ScriptsToDisable)
                {
                    script.enabled = true;
                }
            }
            else
            {
                // Enable menu.
                this.MenuIsOpen = true;

                //Time.timeScale = 0;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                foreach (var script in this.ScriptsToDisable)
                {
                    script.enabled = false;
                }
            }

            this.menuParent.SetActive(!this.menuParent.activeSelf);
        }
    }

    // Volume between 0 and 1.
    private void SetMasterVolume(float volume)
    {
        this.mixer.SetFloat(masterVolumeParameter, Mathf.Log10(volume) * 30f);
    }

    private void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private void SetFullScreen(bool isFullScreen)
    {
        if (Application.isEditor || this.IsDesktopBuild) return;
        
        Screen.fullScreen = isFullScreen;
    }

    private void SetResolution(int resolutionIndex)
    {
        if (Application.isEditor || this.IsDesktopBuild) return;
        
        Screen.SetResolution(this.screenResolutions[resolutionIndex].width, this.screenResolutions[resolutionIndex].height, Screen.fullScreen);
    }

    private void SetFov(float fov)
    {
        if (Camera.main is null) return;
        
        Camera.main.fieldOfView = fov;
    }

    private void SetSensitivity(float sensitivity)
    {
        // Set sensitivity.
        //if (PlayerController.Instance == null) return;

        //PlayerController.Instance.Sensitivity = sensitivity;
    }
}
