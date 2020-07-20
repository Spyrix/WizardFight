using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Interface for the aimprefabs, graphics used to display the directions in which you are shooting spells
public interface IAimPrefab
{
    void SetPlayer(GameObject go);
    GameObject GetPlayer();
}
