using UnityEngine;
using UnityEngine.UI;

public class CemeteryView : MonoBehaviour
{
    [SerializeField] CemeteryCellView pawnView;
    [SerializeField] CemeteryCellView spearView;
    [SerializeField] CemeteryCellView horseView;
    [SerializeField] CemeteryCellView bishopView;
    [SerializeField] CemeteryCellView towerView;
    [SerializeField] CemeteryCellView silverView;
    [SerializeField] CemeteryCellView goldView;

    public void SetCemetaryView(View view)
    {
        pawnView.SetCemetaryView (view, PieceType.Pawn);
        spearView.SetCemetaryView (view, PieceType.Spear);
        horseView.SetCemetaryView (view, PieceType.Horse);
        bishopView.SetCemetaryView (view, PieceType.Bishop);
        towerView.SetCemetaryView (view, PieceType.Tower);
        silverView.SetCemetaryView (view, PieceType.Silver);
        goldView.SetCemetaryView (view, PieceType.Gold);
    }

    public void EnableCemetaryView(bool enabled = true)
    {
        pawnView.EnableButton (enabled);
        spearView.EnableButton (enabled);
        horseView.EnableButton (enabled);
        bishopView.EnableButton (enabled);
        towerView.EnableButton (enabled);
        silverView.EnableButton (enabled);
        goldView.EnableButton (enabled);
    }

    public void UpdateCellView(PieceType pieceType, int count)
    {
        switch (pieceType)
        {
            case PieceType.Pawn:
                pawnView.UpdateCountText(count); 
                break;
            case PieceType.Spear:
                spearView.UpdateCountText(count); 
                break;
            case PieceType.Horse:
                horseView.UpdateCountText(count);
                break;
            case PieceType.Bishop:
                spearView.UpdateCountText(count);
                break;
            case PieceType.Tower:
                spearView.UpdateCountText(count);
                break;
            case PieceType.Silver:
                spearView.UpdateCountText(count);
                break;
            case PieceType.Gold:
                spearView.UpdateCountText(count);
                break;
        }
    }

}
