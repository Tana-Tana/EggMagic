using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : Singleton<PanelManager>
{
    public Dictionary<string, Panel> panels = new();

    private void Start()
    {
        var list = GetComponentsInChildren<Panel>();
        foreach(var panel in list)
        {
            panels[panel.name] = panel;
        }    
    }

    public Panel GetPanel(string name)
    {
        if (IsExisted(name))
        {
            return panels[name];
        }
        //Load panel len tu resources
        Panel panel = Resources.Load<Panel>("Panel/" + name);
        //Sinh ra mot ban sao
        Panel newPanel = Instantiate(panel, transform);
        newPanel.name = name;
        newPanel.gameObject.SetActive(false);

        //luu lai vao trong map
        panels[name] = newPanel;
        return newPanel;
    }

    public void RemovePanel(string name)
    {
        if (IsExisted(name))
        {
            panels.Remove(name);
        }
    }

    public void OpenPanel(string name)
    {
        Panel panel = GetPanel(name);
        panel.Open();
    }

    public void ClosePanel(string name)
    {
        Panel panel = GetPanel(name);
        panel.Close();
    }

    public void CloseAll()
    {
        foreach (var panel in panels.Values)
        {
            panel.Close();
        }
    }

    private bool IsExisted(string name)
    {
        return panels.ContainsKey(name);
    }

}
