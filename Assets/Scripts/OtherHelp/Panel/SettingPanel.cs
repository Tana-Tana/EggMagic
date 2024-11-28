using System;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using UnityEngine;

public class SettingPanel : Panel
{
    [SerializeField] private Animator animator;

    private AnimatorStateInfo _animatorStateInfo;
    private void Start()
    {
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

    public async void Back()
    {
        if(GamePlayController.Instance != null)
        {
            GamePlayController.Instance.CheckContinue = true;
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
        if(!string.IsNullOrEmpty(GameConfig.FACEBOOK_LINK))
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
        if(!string.IsNullOrEmpty (GameConfig.GITHUB_LINK))
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
        if(!string.IsNullOrEmpty(GameConfig.GROUP_LINK))
        {
            Application.OpenURL(GameConfig.GROUP_LINK);
        }
        else
        {
            Debug.Log("Lỗi link Group");
        }
    }
}
