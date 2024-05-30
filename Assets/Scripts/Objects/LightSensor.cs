using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSensor : MonoBehaviour
{
    [Header("UI")]
    public GameObject WarnPanel;

    [Header("Ray Count")]
    public float maxDistance = 10f; // 레이의 최대 거리 설정
    public int rayCount = 10; // 발사할 레이의 개수
    public float coneAngle = 5f; // 원뿔의 각도 (반각)
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
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.white);
            // Raycast를 사용하여 충돌 감지
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, detectionLayer))
            {
                // 충돌한 오브젝트가 플레이어인지 확인
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

        // 플레이어가 감지되지 않았을 때
        if (!playerCurrentlyDetected && playerDetected)
        {
            WarnPanel.SetActive(false);
        }

        playerDetected = playerCurrentlyDetected;
    }

    private void MakeRay()
    {
        // 여기서는 큐브의 아래 중심을 기준으로 레이 발사
        Vector3 origin = transform.position + new Vector3(0, -transform.localScale.y / 2, 0);

        // 각도 계산
        float angleStep = 360f / rayCount;
        float currentAngle = 0f;

        for (int i = 0; i < rayCount; i++)
        {
            // 각도에 따른 방향 계산
            float radianAngle = currentAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(radianAngle), -Mathf.Tan(coneAngle * Mathf.Deg2Rad), Mathf.Cos(radianAngle));

            // Ray 발사
            Ray ray = new Ray(origin, direction);
            rays.Add(ray);

            currentAngle += angleStep;
        }

        // 더 촘촘한 원뿔로 추가 레이 생성
        float innerAngleStep = angleStep / 2;
        currentAngle = innerAngleStep / 2;

        for (int i = 0; i < rayCount; i++)
        {
            // 각도에 따른 방향 계산
            float radianAngle = currentAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(radianAngle), -Mathf.Tan((coneAngle / 2) * Mathf.Deg2Rad), Mathf.Cos(radianAngle));

            // Ray 발사
            Ray ray = new Ray(origin, direction);
            rays.Add(ray);

            currentAngle += innerAngleStep;
        }
    }
}
