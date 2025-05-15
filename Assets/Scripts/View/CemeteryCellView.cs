using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CemeteryCellView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countText;

    View view;
    Button buttonComponent;

    PieceType pieceType;

    private void Awake()
    {
        buttonComponent = GetComponent<Button>();
    }

    public void SetCemetaryView(View view, PieceType pieceType)
    {
        this.view = view;
        this.pieceType = pieceType;
    }

    public void Start()
    {
        countText.text = "0";
    }

    private void OnEnable()
    {
        buttonComponent.onClick.AddListener(SelectCemetaryCell);
    }

    private void OnDisable()
    {
        buttonComponent.onClick.RemoveListener(SelectCemetaryCell);
    }

    public void EnableButton(bool enabled = true)
    {
        buttonComponent.enabled = enabled;
    }

    public void UpdateCountText(int count)
    {
        countText.text = count.ToString();
    }

    public void SelectCemetaryCell()
    {
        view?.SelectCemetaryPiece(pieceType);
    } 
    
}
