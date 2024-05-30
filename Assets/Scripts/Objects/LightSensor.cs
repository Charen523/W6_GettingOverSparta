using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSensor : MonoBehaviour
{
    [Header("UI")]
    public GameObject WarnPanel;

    [Header("Ray Count")]
    public float maxDistance = 10f; // ������ �ִ� �Ÿ� ����
    public int rayCount = 20; // �߻��� ������ ����
    public float coneAngle = 70f; // ������ ���� (�ݰ�)
    public LayerMask detectionLayer;

    private List<Ray> rays = new List<Ray>();
    private bool playerDetected = false;
    
    private void Start()
    {
        MakeRay();
    }

    void Update()
    {
        bool playerCurrentlyDetected = false;

        foreach (var ray in rays)
        {
            // Raycast�� ����Ͽ� �浹 ����
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, detectionLayer))
            {
                // �浹�� ������Ʈ�� �÷��̾����� Ȯ��
                if (hit.collider.CompareTag("Player"))
                {
                    playerCurrentlyDetected = true;
                    if (!WarnPanel.activeSelf)
                    {
                        WarnPanel.SetActive(true);
                    }
                }
            }
        }

        // �÷��̾ �������� �ʾ��� ��
        if (!playerCurrentlyDetected && playerDetected)
        {
            WarnPanel.SetActive(false);
        }

        playerDetected = playerCurrentlyDetected;
    }

    private void MakeRay()
    {
        //ť���� �Ʒ� �߽��� �������� ���� �߻�
        Vector3 origin = transform.position + new Vector3(0, -transform.localScale.y / 2, 0);

        // ���� ���
        float angleStep = 360f / rayCount;
        float currentAngle = 0f;

        for (int i = 0; i < rayCount; i++)
        {
            // ������ ���� ���� ���
            float radianAngle = currentAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(radianAngle), -Mathf.Tan(coneAngle * Mathf.Deg2Rad), Mathf.Cos(radianAngle));

            // Ray �߻�
            Ray ray = new Ray(origin, direction);
            rays.Add(ray);

            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.white, 10f);

            currentAngle += angleStep;
        }

        // �� ������ ���Է� �߰� ���� ����
        angleStep = 360f / rayCount;
        currentAngle = 0;

        for (int i = 0; i < rayCount; i++)
        {
            // ������ ���� ���� ���
            float radianAngle = currentAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(radianAngle), -Mathf.Tan((coneAngle + 10) * Mathf.Deg2Rad), Mathf.Cos(radianAngle));

            // Ray �߻�
            Ray ray = new Ray(origin, direction);
            rays.Add(ray);

            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 10f);

            currentAngle += angleStep;
        }
    }
}
