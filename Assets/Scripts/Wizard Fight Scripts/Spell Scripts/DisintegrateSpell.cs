using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* The class that holds constants for the disintegrate spell object.
 * Also defines instansiation behavior.
 */
[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Rigidbody))]
public class DisintegrateSpell : SpellObject
{
    Collider col;
    Vector3 movementVector;
    [SerializeField]
    internal Transform spellTransform;
    [SerializeField]
    internal Rigidbody spellRigidBody;
    internal float speed = .8f;
    internal float damage = -25f;
    bool move = false;
    [SerializeField]
    internal float spellCooldownTime = 2f;
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

    public void SetMovementVector(Vector3 v)
    {
        movementVector = v;
    }

    public void SetRotation(float y)
    {
        spellTransform.Rotate(new Vector3(0f,y,90f), Space.World);

    }

    public override void Cast(Transform playerTransform)
    {
        GameObject spell = Instantiate(gameObject, playerTransform.position + new Vector3(playerTransform.forward.x, .5f, playerTransform.forward.z) * 3, Quaternion.identity);
        spell.GetComponent<DisintegrateSpell>().SetMovementVector(playerTransform.forward);
        spell.GetComponent<DisintegrateSpell>().SetRotation(playerTransform.rotation.eulerAngles.y + 90);
        spell.GetComponent<DisintegrateSpell>().SetMove(true);
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
