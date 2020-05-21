using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

interface IEnemyState
{
     void Enter();
     void Exit();
}

public class EnemyIdleState : IEnemyState
{
    public EnemyIdleState()
    {

    }
    public void Enter()
    {

    }
    public void Exit()
    {

    }
}