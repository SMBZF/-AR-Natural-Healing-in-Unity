using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets
{
    [RequireComponent(typeof(Rigidbody))]
    public class ARContactSpawnTrigger : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The behavior to use to spawn objects.")]
        ObjectSpawner m_ObjectSpawner;

        public ObjectSpawner objectSpawner
        {
            get => m_ObjectSpawner;
            set => m_ObjectSpawner = value;
        }

        [SerializeField]
        [Tooltip("Whether to require that the AR Plane has an alignment of horizontal up to spawn on it.")]
        bool m_RequireHorizontalUpSurface;

        public bool requireHorizontalUpSurface
        {
            get => m_RequireHorizontalUpSurface;
            set => m_RequireHorizontalUpSurface = value;
        }

        [SerializeField]
        [Tooltip("Minimum distance to check for nearby objects before spawning a new one.")]
        float spawnCheckRadius = 10.0f;

        [SerializeField]
        [Tooltip("Cooldown time between spawns.")]
        float spawnCooldown = 1.5f;

        private float lastSpawnTime = -1f;

        // ������ˮƽ�����ֱ�����ٵ�����
        [SerializeField]
        [Tooltip("The object to destroy when touching a horizontal plane.")]
        GameObject horizontalObjectToDestroy;

        [SerializeField]
        [Tooltip("The object to destroy when touching a vertical plane.")]
        GameObject verticalObjectToDestroy;

        void Start()
        {
            if (m_ObjectSpawner == null)
#if UNITY_2023_1_OR_NEWER
                m_ObjectSpawner = FindAnyObjectByType<ObjectSpawner>();
#else
                m_ObjectSpawner = FindObjectOfType<ObjectSpawner>();
#endif
        }

        void OnTriggerEnter(Collider other)
        {
            // �����ײ�Ƿ�Ϊ��Ч������ƽ��
            if (!TryGetSpawnSurfaceData(other, out var surfacePosition, out var surfaceNormal))
                return;

            // ����Ƿ�����ȴʱ����
            if (Time.time - lastSpawnTime < spawnCooldown)
                return;

            // ������ɵ㸽���Ƿ�����������
            if (IsObjectNear(surfacePosition))
            {
                Debug.Log("Nearby object found, skipping spawn.");
                return;
            }

            // ������ײ����ƽ���ϵ������
            var infinitePlane = new Plane(surfaceNormal, surfacePosition);
            var contactPoint = infinitePlane.ClosestPointOnPlane(transform.position);

            // ������������
            m_ObjectSpawner.TrySpawnObject(contactPoint, surfaceNormal);

            // ����ȫ�� Shader ����
            UpdateShaderWithWorldPosition(contactPoint);

            lastSpawnTime = Time.time;

            // ���������ݴ�����ƽ��������������
            if (IsHorizontalPlane(surfaceNormal))
            {
                // ˮƽ��
                DestroyObject(horizontalObjectToDestroy);
            }
            else if (IsVerticalPlane(surfaceNormal))
            {
                // ��ֱ��
                DestroyObject(verticalObjectToDestroy);
            }
        }

        private void UpdateShaderWithWorldPosition(Vector3 position)
        {
            // ʹ��ȫ����������
            Shader.SetGlobalVector("_GlobalSpawnPosition", position);
        }

        public bool TryGetSpawnSurfaceData(Collider objectCollider, out Vector3 surfacePosition, out Vector3 surfaceNormal)
        {
            surfacePosition = default;
            surfaceNormal = default;

            var arPlane = objectCollider.GetComponent<ARPlane>();
            if (arPlane == null)
                return false;

            // ���Ҫ��ˮƽ�����ɣ�����⵽��ƽ�治��ˮƽ�棬�򷵻� false
            if (m_RequireHorizontalUpSurface && arPlane.alignment != PlaneAlignment.HorizontalUp)
                return false;

            // ��ȡƽ��ķ��ߺ�����λ��
            surfaceNormal = arPlane.normal;
            surfacePosition = arPlane.center;
            return true;
        }

        private bool IsObjectNear(Vector3 position)
        {
            // ������ɵ㸽���Ƿ��������ɵ�����
            Collider[] hitColliders = Physics.OverlapSphere(position, spawnCheckRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("SpawnedObject"))
                {
                    return true;
                }
            }
            return false;
        }

        private void DestroyObject(GameObject objectToDestroy)
        {
            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);  // ����ָ��������
            }
        }

        private bool IsHorizontalPlane(Vector3 surfaceNormal)
        {
            // �ж��Ƿ���ˮƽ�棨���߷���ӽ� (0, 1, 0)��
            return Mathf.Abs(surfaceNormal.y) > Mathf.Abs(surfaceNormal.x) && Mathf.Abs(surfaceNormal.y) > Mathf.Abs(surfaceNormal.z);
        }

        private bool IsVerticalPlane(Vector3 surfaceNormal)
        {
            // �ж��Ƿ�����ֱ�棨���߷���ӽ� (1, 0, 0) �� (0, 0, 1)��
            return Mathf.Abs(surfaceNormal.x) > Mathf.Abs(surfaceNormal.y) && Mathf.Abs(surfaceNormal.x) > Mathf.Abs(surfaceNormal.z) ||
                   Mathf.Abs(surfaceNormal.z) > Mathf.Abs(surfaceNormal.x) && Mathf.Abs(surfaceNormal.z) > Mathf.Abs(surfaceNormal.y);
        }
    }
}
