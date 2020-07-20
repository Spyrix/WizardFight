using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//The movement and instansiation script for the ice spell
[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Rigidbody))]
public class IceSpell : SpellObject
{
    Collider col;
    Vector3 movementVector;
    Transform spellTransform;
    Rigidbody spellRigidBody;
    float speed = .8f;
    internal float damage = 0f;
    bool move = false;
    [SerializeField]
    internal float spellCooldownTime = 4f;
    [SerializeField]
    internal GameObject aimPrefab;
    [SerializeField]
    internal Sprite spellIcon;
    
    // Start is called before the first frame update
    void Awake()
    {
        col = GetComponent<CapsuleCollider>();
        spellTransform = GetComponent<Transform>();
        spellRigidBody = GetComponent<Rigidbody>();
        //spellTransform.forward = new Vector3(1, 0, 0);
        spellRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement();
    }

    internal void movement()
    {
        if (move)
        {
            spellTransform.Translate(movementVector * speed, Space.World);
        }
    }


    internal void SetMove(bool a)
    {
        move = a;
    }

    public float GetDamage()
    {
        return damage;
    }

    //Set the direction in which the spell will move
    public void SetMovementVector(Vector3 v)
    {
        movementVector = v;
    }

    public void SetRotation(float y)
    {
        spellTransform.Rotate(new Vector3(0f, y, 90f), Space.Self);
    }

    //This function instansiates a copy of the prefab it's attached to at the specified position
    public override void Cast(Transform playerTransform)
    {
        GameObject spell = Instantiate(gameObject, playerTransform.position + new Vector3(playerTransform.forward.x, .5f, playerTransform.forward.z) * 2, Quaternion.identity);
        spell.GetComponent<IceSpell>().SetMovementVector(playerTransform.forward);
        spell.GetComponent<IceSpell>().SetRotation(playerTransform.rotation.eulerAngles.y+90);
        spell.GetComponent<IceSpell>().SetMove(true);
    }

    public override float GetCooldownTime()
    {
        return spellCooldownTime;
    }

    public override GameObject GetAimPrefab()
    {
        return aimPrefab;
    }

    public override Sprite GetSpellIcon()
    {
        return spellIcon;
    }
}
