using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using UnityEngine;

public class AchievementPanel : Panel
{
    [SerializeField] private Animator animator;
    private AnimatorStateInfo _animatorStateInfo;

    [Header("Achievement", order = 0)]
    [SerializeField] private Achievement achievementPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private int amoutAchievement;
    private List<Achievement> _achievements;

    private void Start()
    {
        _achievements = new List<Achievement>();
        _animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (GamePlayController.Instance == null)
        {
            Messenger.Broadcast<float>(EventKey.HIDE_BOTTOM_AND_PUSH_UP_TITLE, _animatorStateInfo.length);
        }

        animator.enabled = true;
        GeneratePrefabs(achievementPrefab);
        LoadAchievementCurrent();
    }

    private void LoadAchievementCurrent()
    {
        int indexOpenAchievement = GamePrefs.GetLevelEggMax();
        
        for(int i = 0;i<indexOpenAchievement; i++)
        {
            _achievements[i].HideGround.SetActive(false);
            _achievements[i].Ground.SetActive(true);

            EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + "Level" + (i + 1));
            //Debug.Log(levelCurrent + " " + (eggInfor == null));
            _achievements[i].Egg.sprite = eggInfor.ImageAchivement;
            _achievements[i].Egg.SetNativeSize();
            if ((i+1) == 6 || (i+1) == 7)
            {
                _achievements[i].Egg.transform.position += Vector3.up * 0.05f;
            }
        }
    }

    private void GeneratePrefabs(Achievement prefab)
    {
        amoutAchievement = 14;
        for(int i=0;i < amoutAchievement;++i)
        {
            var achievement = Instantiate(prefab, content);
            _achievements.Add(achievement);
        }
    }

    public async void BackAsync()
    {
        if (GamePlayController.Instance == null)
        {
            Messenger.Broadcast<float>(EventKey.SHOW_BOTTOM_AND_PUSH_UP_TITLE, _animatorStateInfo.length);
        }
        else
        {
            Messenger.Broadcast(EventKey.SHOW_POPUP_END_GAME);
        }

        animator.SetBool("isClose", true);
        await Task.Delay((int)(_animatorStateInfo.length * 1000));
        PanelManager.Instance.ClosePanel(GameConfig.ACHIEVEMENT_PANEL);
    }
}
