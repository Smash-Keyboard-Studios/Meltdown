using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamGenerator : MonoBehaviour
{
    public class Target : MonoBehaviour
    {
    // PLACEHOLDER CODE: PLEASE REPLACE WITH ACTUAL CODE TO CHECK THE COLLISION WITH THE FIRE BULLETS.

    // Example detection using the OnCollisionEnter event.
         
        private void OnCollisionEnter(Collision collision)
        {
            PlaceHolderFireBulletCollision(collision);
        }

        private void PlaceHolderFireBulletCollision(Collision collision)
        {
            // Insert Collision Check with Fire Bullet here, e.g. collision.gameObject.CompareTag("FireBullet")
            // If true, set the public variable: GaugeIndicator.MoveToNextPoint = true;
        }
    }
}
