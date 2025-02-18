using System;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using UnityEngine;

public class SettingPanel : Panel
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject musicOn;
    [SerializeField] private GameObject musicOff;
    [SerializeField] private GameObject soundOn;
    [SerializeField] private GameObject soundOff;

    private AnimatorStateInfo _animatorStateInfo;
    private void Start()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.OPEN_POPUP_AUDIO);
        LoadMusicAndSoundStatus();
        _animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if(GamePlayController.Instance != null)
        {
            animator.enabled = false;
        }
        else
        {
            Messenger.Broadcast<float>(EventKey.HIDE_BOTTOM_AND_PUSH_UP_TITLE, _animatorStateInfo.length);
            animator.enabled = true;
        }
    }

    private void LoadMusicAndSoundStatus()
    {
        if(GamePrefs.GetMusic() == 1)
        {
            musicOn.SetActive(true);
            musicOff.SetActive(false);
        }
        else
        {
            musicOn.SetActive(false);
            musicOff.SetActive(true);
        }

        if(GamePrefs.GetSound() == 1)
        {
            soundOn.SetActive(true); 
            soundOff.SetActive(false);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
    }

    public async void Back()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.TAP_AUDIO);
        if (GamePlayController.Instance != null)
        {
            GamePlayController.Instance.checkContinue = true;
        }
        else
        {
            animator.SetBool("isClose", true);
            Messenger.Broadcast<float>(EventKey.SHOW_BOTTOM_AND_PUSH_UP_TITLE, _animatorStateInfo.length);
            await Task.Delay((int)(_animatorStateInfo.length*1000));
        }
        PanelManager.Instance.ClosePanel(GameConfig.SETTING_PANEL);
    }

    public void OpenFacebookLink()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.TAP_AUDIO);
        if (!string.IsNullOrEmpty(GameConfig.FACEBOOK_LINK))
        {
            Application.OpenURL(GameConfig.FACEBOOK_LINK);
        }
        else
        {
            Debug.LogWarning("Lỗi link facebook");
        }
    }

    public void OpenGithubLink()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.TAP_AUDIO);

        if (!string.IsNullOrEmpty (GameConfig.GITHUB_LINK))
        {
            Application.OpenURL (GameConfig.GITHUB_LINK);
        }
        else
        {
            Debug.Log("Lỗi link github");
        }
    }

    public void OpenGroupLink()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.TAP_AUDIO);

        if (!string.IsNullOrEmpty(GameConfig.GROUP_LINK))
        {
            Application.OpenURL(GameConfig.GROUP_LINK);
        }
        else
        {
            Debug.Log("Lỗi link Group");
        }
    }

    public void SetStatusSound()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.TAP_AUDIO);
        //
        if (soundOn.activeSelf)
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
            GamePrefs.SetSound(0);
        }
        else
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
            GamePrefs.SetSound(1);
        }
        SoundController.Instance.ChangeStatusSound();
    }

    public void SetStatusMusic()
    {
        SoundController.Instance.PlayOneShotAudio(GameConfig.TAP_AUDIO);
        //
        if (musicOn.activeSelf)
        {
            musicOn.SetActive(false);
            musicOff.SetActive(true);
            GamePrefs.SetMusic(0);
        }
        else
        {
            musicOn.SetActive(true);
            musicOff.SetActive(false);
            GamePrefs.SetMusic(1);
        }
        SoundController.Instance.ChangeStatusMusic();
    }
}
