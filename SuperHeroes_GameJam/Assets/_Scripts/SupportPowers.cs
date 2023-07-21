using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportPowers : MonoBehaviour
{
    GameObject Box;
    Coroutine LiftingBox;
    Rigidbody BoxRigidBody;

    public void Lift()
    {
        Box = NearestGameObject(transform.position, 20f, "Box");
        if(Box != null)
        {
            StartCoroutine(LiftBox());           
        }
    }

    IEnumerator LiftBox()
    {
        float time = 0f;

        BoxRigidBody = Box.GetComponent<Rigidbody>();
        BoxRigidBody.useGravity = false;

        Vector3 startPos = Box.transform.position;

        while(time < 1f)
        {
            Box.transform.position = Vector3.Lerp(startPos, startPos + new Vector3(0f, 3f, 0f), time);
            time += Time.deltaTime * 3;
            yield return null;
        }
    }

    public void Throw()
    {
        if(LiftingBox != null)
        {
            StopCoroutine(LiftingBox);

            GameObject target = NearestGameObject(transform.position, 20f, "Enemy");

            if (target == null)
            {
                BoxRigidBody.useGravity = true;
            }
            else
            {
                Vector3 direction = Box.transform.position - target.transform.position;
                BoxRigidBody.useGravity = true;
                BoxRigidBody.AddForce(-direction * 3f, ForceMode.VelocityChange);

            }
        }
        else
        {
            GameObject target = NearestGameObject(transform.position, 20f, "Enemy");

            if (target == null)
            {
                BoxRigidBody.useGravity = true;
            }
            else
            {
                Vector3 direction = Box.transform.position - target.transform.position;
                BoxRigidBody.useGravity = true;
                BoxRigidBody.AddForce(-direction * 3f, ForceMode.VelocityChange);

            }
        }
    }



    public GameObject NearestGameObject(Vector3 pos, float radius, string layer)
    {
        LayerMask mask = LayerMask.GetMask(layer);

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

            if (Mathf.Abs(Vector3.Angle(transform.forward, collider.gameObject.transform.position)) < 90 && distanceToCurrent < 100)
            {
                if (distanceToCurrent < distanceToNearest)
                {
                    nearestObject = collider.gameObject;
                }
            }

        }

        return nearestObject;
    }
}
