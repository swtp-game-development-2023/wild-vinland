using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour {
    [SerializeField] private BoxCollider2D punchHitbox;

    public void PunchHitboxEnable() {
        punchHitbox.enabled = true;
    }

    public void PunchHitboxDisable() {
        punchHitbox.enabled = false;
    }
}
