using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public const int EnemyDamageIndex = 0;
    public const int PlayerDamageIndex = 0;

    [SerializeField]
    Transform canvasTransform;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    PrefabCacheData[] Files;

    Dictionary<string, GameObject> FileCache = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Prepare();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Load(string resourcePath)
    {
        GameObject go = null;

        if (FileCache.ContainsKey(resourcePath))
        {
            go = FileCache[resourcePath];
        }
        else
        {
            go = Resources.Load<GameObject>(resourcePath);
            if (!go)
            {
                Debug.LogError("Load Error! path = " + resourcePath);
                return null;
            }

            FileCache.Add(resourcePath, go);
        }

        return go;
    }

    public void Prepare()
    {
        for (int i = 0; i < Files.Length; i++)
        {
            GameObject go = Load(Files[i].filePath);
            SystemManager.Instance.DamageCacheSystem.GenerateCache(Files[i].filePath, go, Files[i].cacheCount, canvasTransform);
        }
    }

    public GameObject Generate(int index, Vector3 position, int damageValue, Color textColor)
    {
        if (index < 0 || index >= Files.Length)
        {
            Debug.LogError("Generate error! out of range! index = " + index);
            return null;
        }

        string filePath = Files[index].filePath;
        GameObject go = SystemManager.Instance.DamageCacheSystem.Archive(filePath);
        //go.transform.position = position;
        go.transform.position = Camera.main.WorldToScreenPoint(position);

        UIDamage damage = go.GetComponent<UIDamage>();
        damage.FilePath = filePath;
        damage.ShowDamage(damageValue, textColor);

        return go;

    }

    public bool Remove(UIDamage damage)
    {
        SystemManager.Instance.DamageCacheSystem.Restore(damage.FilePath, damage.gameObject);
        return true;
    }
}
