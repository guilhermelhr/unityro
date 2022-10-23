using System;
using UnityEngine;
using UnityEngine.Audio;

internal class SoundSettingsWindow : DraggableUIWindow {

    [SerializeField] 
    private AudioMixer AudioMixer;

    public void SetMasterLevel(float level) {
        AudioMixer.SetFloat("MasterVolume", GetDbValue(level));
    }

    public void SetEffectsLevel(float level) {
        AudioMixer.SetFloat("EffectsVolume", GetDbValue(level));
    }

    public void SetBgmLevel(float level) {
        AudioMixer.SetFloat("BGMVolume", GetDbValue(level));
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    private float GetDbValue(float level) => Mathf.Log10(level) * 20;

    internal void Show() {
        gameObject.SetActive(true);
    }
}
