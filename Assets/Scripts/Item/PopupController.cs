using UnityEngine;

public class PopupController : MonoBehaviour
{
    public void OnUseButtonClick()
    {
        // 아이템 사용 로직
        Debug.Log("아이템을 사용했습니다.");
        ClosePopup();
    }

    public void OnCancelButtonClick()
    {
        ClosePopup();
    }

    private void ClosePopup()
    {
        gameObject.SetActive(false); // 팝업을 비활성화
    }
}