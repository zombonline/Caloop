using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    public bool flagRaised = false;
    [SerializeField] public int type = 0;
    List<GameObject> itemList = new List<GameObject>();
    private void Awake()
    {
        foreach(ScenePersist item in FindObjectsOfType<ScenePersist>())
        {
            if (item.type == type)
            {
                itemList.Add(item.gameObject);
            }
        }

        if (itemList.Count > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ToggleFlag(bool val)
    {
        flagRaised = val;
    }
}
