using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Members
    [SerializeField] private Transform player;
    private float xOffset = 6.0f;
    private float yOffset = 1.5f;
    private Vector3 targetPos;
    private Vector3 vel = Vector3.zero;
    private float smoothFactor = 0.25f;
    #endregion

    private void Update()
    {
        targetPos = new Vector3(player.position.x + xOffset, player.position.y + yOffset, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, smoothFactor);
    }
}
