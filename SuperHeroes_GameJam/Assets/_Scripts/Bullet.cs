using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    public float speed = 5f;

    public AnimationCurve NoiseCurve;

    float timer;

    public Transform Target;

    Rigidbody Rigidbody;

    

    public Vector2 MinNoise = new Vector3(-3f , -0.25f);
    public Vector2 MaxNoise = new Vector3(3f, 1f);

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();

        if (Target == null)
        {
            //Rigidbody.velocity = transform.forward * speed;
            var nearestOnject = NearestGameObject(transform.position, 20f);

            if (nearestOnject != null)
            {
                Target = nearestOnject.transform;
                StartCoroutine(Chase());
            }
            else
            {
                Rigidbody.velocity = transform.forward * speed;
            }

        }

        
    }  

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 1)
        {
            Destroy(gameObject);
        }   
    }


    IEnumerator Chase()
    {

        Vector3 startPos = transform.position;
        float time = 0;


        var direction = startPos - Target.position;

        if (Mathf.Abs(Vector3.Angle(transform.forward, Target.position)) < 45 && direction.sqrMagnitude < 400)
        {
            Vector2 noise = new Vector2(Random.Range(MinNoise.x, MaxNoise.x), Random.Range(MinNoise.y, MaxNoise.y));
            Vector3 BulletDirection = new Vector3(Target.position.x, Target.position.y, Target.position.z) - startPos;
            Vector3 HorizontalNoise = Vector3.Cross(BulletDirection, Vector3.up).normalized;

            float noisePos = 0f;

            while (time < 1f && Target != null)
            {
                noisePos = NoiseCurve.Evaluate(time);

                transform.position = Vector3.Lerp(startPos, Target.position, time) + new Vector3(HorizontalNoise.x * noisePos * noise.x, noisePos * noise.y, HorizontalNoise.z * noisePos * noise.x);
                transform.LookAt(transform.transform);
                time += Time.deltaTime;
                yield return null;
            }

        }
        else
        {
            Rigidbody.velocity = transform.forward * speed;
        }


        if (Target == null)
        {
            Rigidbody.velocity = transform.forward * speed;
        }

        yield return null;
    }


    public GameObject NearestGameObject(Vector3 pos, float radius)
    {
        LayerMask mask = LayerMask.GetMask("Enemy");

        Collider[] gameObjectsInRadius = Physics.OverlapSphere(pos, radius, mask);

        if (gameObjectsInRadius.Length == 0)
        {
            return null;
        }

        GameObject nearestObject = gameObjectsInRadius[0].gameObject;

        foreach (Collider collider in gameObjectsInRadius)
        {
            var distanceToNearest = Vector3.SqrMagnitude(pos - nearestObject.transform.position);
            var distanceToCurrent = Vector3.SqrMagnitude(pos - collider.gameObject.transform.position);

            var direction = pos - collider.gameObject.transform.position;

            if (Mathf.Abs(Vector3.Angle(transform.forward, collider.gameObject.transform.position)) < 20 && distanceToCurrent < 400)
            {
                if (distanceToCurrent < distanceToNearest)
                {
                    nearestObject = collider.gameObject;
                }
            }

        }

        return nearestObject;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        
    }


    private void OnTriggerEnter(Collider other)
    {

        if (!NetworkClient.active)
        {
            return;
        }

        if(other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            uint id = PlayerData.Instance.MyCharacterGO.GetComponent<NetworkIdentity>().netId;
            enemy.DecreaseEnemyHealth(5, id);
        }
    }

  

}
