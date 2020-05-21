using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAimPrefab
{
    void SetPlayer(GameObject go);
    GameObject GetPlayer();
}
