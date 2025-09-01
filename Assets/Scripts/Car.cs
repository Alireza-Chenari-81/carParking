using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Car : MonoBehaviour
{
    public Route route;

    public Transform buttomtransform;
    public Transform bodytransform;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] ParticleSystem smokeFX;
    [SerializeField] Rigidbody rb;
    [SerializeField] float danceValue;
    [SerializeField] float durationMultiplier;

    private void Start()
    {
        bodytransform.DOLocalMoveY(danceValue, .1f).SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear); 
    }

    public void Move(Vector3[] path)
    {
        rb.DOLocalPath(path, 2f * durationMultiplier, (PathType)path.Length)
            .SetLookAt(.01f,false)
            .SetEase(Ease.Linear);
      
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Car othercar))
        {
            stopdancinganim();
            rb.DOKill(false);

            Vector3 hitpoint = collision.contacts[0].point;
            AddExplosionForce(hitpoint);
            smokeFX.Play();

            Game.Instance.onCarCollision?.Invoke();
        }
    }
    private void AddExplosionForce(Vector3 points)
    {
        rb.AddExplosionForce(500f, points, 5f);
        rb.AddForceAtPosition(Vector3.up * 2f, points,ForceMode.Impulse);
        rb.AddTorque(new Vector3(GetRandomAngle(), GetRandomAngle(), GetRandomAngle()));   
    }
    private float GetRandomAngle()
    {
        float angle = 10f;
        float rand = Random.value;
        return rand > .5f? angle : -angle;
    }
    public void stopdancinganim()
    {
        bodytransform.DOKill(true); 
    }

    public void setColor(Color color)
    {
        meshRenderer.sharedMaterials[0].color = color;
    }

}
