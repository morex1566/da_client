using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class WeaponAttachmentTreeBuilder
{
    private const string BuiltAttachmentPrefix = "[WeaponSlot]";

    public static void BuildSlots(Transform root, List<WeaponAttachment> attachments, WeaponAttachment attachmentPrefab)
    {
        if (attachmentPrefab == null)
        {
            return; // 시작 파츠가 없으면 슬롯 생성 불가
        }

        BuildSlots(root, attachments, new[] { attachmentPrefab });
    }

    public static void BuildSlots(Transform root, List<WeaponAttachment> attachments, IReadOnlyList<WeaponAttachment> attachmentPrefabs)
    {
        if (root == null)
        {
            return; // 부착물을 생성할 루트가 없으면 빌드 불가
        }

        if (attachments == null)
        {
            return; // 결과를 담을 리스트가 없으면 빌드 불가
        }

        ClearBuiltChildren(root);
        attachments.Clear();

        if (attachmentPrefabs == null)
        {
            return; // 생성할 프리팹 목록이 없으면 기존 슬롯 정리만 수행
        }

        for (int i = 0; i < attachmentPrefabs.Count; i++)
        {
            var attachmentPrefab = attachmentPrefabs[i];
            if (attachmentPrefab == null)
            {
                continue; // 빈 프리팹 항목은 건너뜀
            }

            var attachment = InstantiateAttachment(attachmentPrefab, root, $"Root_{i:00}");
            if (attachment == null)
            {
                continue; // 생성 실패한 파츠는 트리에 추가하지 않음
            }

            attachments.Add(attachment);
            BuildChildSlots(attachment);
        }
    }

    public static T FindWeaponAttachment<T>(IReadOnlyList<WeaponAttachment> attachments) where T : WeaponAttachment
    {
        if (attachments == null)
        {
            return null; // 탐색할 부착물 목록이 없으면 실패
        }

        foreach (var attachment in attachments)
        {
            T result = FindWeaponAttachmentInTree<T>(attachment);
            if (result != null)
            {
                return result; // 첫 번째 매칭 파츠 반환
            }
        }

        return null;
    }

    private static void BuildChildSlots(WeaponAttachment parentAttachment)
    {
        if (parentAttachment == null)
        {
            return; // 부모 파츠가 없으면 하위 슬롯 생성 불가
        }

        ClearBuiltChildren(parentAttachment.transform);

        if (parentAttachment.Data == null)
        {
            return; // 데이터가 없으면 슬롯 규칙을 알 수 없음
        }

        if (parentAttachment.Data.Slots == null)
        {
            return; // 슬롯 목록이 없으면 하위 파츠 생성 없음
        }

        for (int i = 0; i < parentAttachment.Data.Slots.Count; i++)
        {
            var slot = parentAttachment.Data.Slots[i];
            if (CanBuildSlot(slot) == false)
            {
                continue; // 슬롯 타입과 프리팹 타입이 맞지 않으면 건너뜀
            }

            var childAttachment = InstantiateAttachment(slot.Prefab, parentAttachment.transform, $"{slot.AllowedType}_{i:00}");

            BuildChildSlots(childAttachment);
        }
    }

    private static bool CanBuildSlot(WeaponAttachmentSlot slot)
    {
        if (slot == null)
        {
            return false; // 슬롯 정의가 없으면 생성 불가
        }

        if (slot.Prefab == null)
        {
            return false; // 슬롯에 들어갈 프리팹이 없으면 생성 불가
        }

        if (slot.Prefab.Data == null)
        {
            return true; // 데이터 없는 프리팹은 타입 검증 없이 허용
        }

        return slot.Prefab.Data.Type == slot.AllowedType;
    }

    private static WeaponAttachment InstantiateAttachment(WeaponAttachment prefab, Transform parent, string label)
    {
        WeaponAttachment instanceObj = null;

#if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            instanceObj = PrefabUtility.InstantiatePrefab(prefab, parent) as WeaponAttachment;
        }
        else
#endif
        {
            instanceObj = UnityEngine.Object.Instantiate(prefab, parent);
        }

        if (instanceObj == null)
        {
            return null; // Prefab 인스턴스 생성 실패
        }

        instanceObj.gameObject.name = $"{BuiltAttachmentPrefix}_{label}_{prefab.GetType().Name}";
        return instanceObj;
    }

    private static void ClearBuiltChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            if (IsBuiltAttachment(child) == false)
            {
                continue; // 수동 배치된 자식은 삭제하지 않음
            }

            DestroyBuiltChild(child.gameObject);
        }
    }

    private static bool IsBuiltAttachment(Transform child)
    {
        return
            child.name.StartsWith(BuiltAttachmentPrefix) ||
            child.name.EndsWith("_Slot");
    }

    private static void DestroyBuiltChild(GameObject child)
    {
#if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            UnityEngine.Object.DestroyImmediate(child);
            return;
        }
#endif

        UnityEngine.Object.Destroy(child);
    }

    private static T FindWeaponAttachmentInTree<T>(WeaponAttachment attachment) where T : WeaponAttachment
    {
        if (attachment == null)
        {
            return null; // 빈 노드는 탐색 대상이 아님
        }

        if (attachment is T result)
        {
            return result; // 현재 파츠가 요청 타입이면 반환
        }

        Transform transform = attachment.transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            T childResult = FindWeaponAttachmentInTransform<T>(child);
            if (childResult != null)
            {
                return childResult; // 하위 Transform에서 찾은 파츠 반환
            }
        }

        return null;
    }

    private static T FindWeaponAttachmentInTransform<T>(Transform transform) where T : WeaponAttachment
    {
        var attachment = transform.GetComponent<WeaponAttachment>();
        if (attachment is T result)
        {
            return result; // 현재 Transform의 파츠가 요청 타입이면 반환
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            T childResult = FindWeaponAttachmentInTransform<T>(transform.GetChild(i));
            if (childResult != null)
            {
                return childResult; // 자식 Transform에서 찾은 파츠 반환
            }
        }

        return null;
    }
}
