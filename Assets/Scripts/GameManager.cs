using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EnumTypes;

public class GameManager : MonoBehaviour 
{
    [SerializeField] private PieceSpawner pieceSpawner;    
    [SerializeField] private ActionBar actionBar;          
    [SerializeField] private GameObject winScreen;         
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private int typesCount;

    private int remainingPieces = 0;     
    private bool gameActive = false;
    private bool isAnimated = false;
    
    private List<GameObject> allPieces;

    private void Awake() {
        Instance = this;
    }

    public static GameManager Instance { get; private set; }

    private void Start() {
        actionBar.OnTripleMatch += HandleTripleMatch;
        actionBar.OnTrayFull += HandleTrayFull;

        StartGame();
    }


    private void StartGame()
    {
        gameActive = true;

        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        endScreen.SetActive(false);

        actionBar.ResetBar();

        remainingPieces = pieceSpawner.SpawnPieces(typesCount);
    }


    public void OnPieceClicked(BaseShape piece) 
    {
        if (!gameActive || this.isAnimated) return;

        this.isAnimated = true;
        ShapeCombination combo = new ShapeCombination(piece.Figure, piece.Color, piece.Animal);

        piece.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        
        piece.transform.DOMove(actionBar.GetWorldPositionFreeContainer(), 1f).OnComplete(() =>
        {
            this.isAnimated = false;
            Destroy(piece.gameObject);
            actionBar.AddPiece(combo);
        });
        
        piece.transform.DORotate(Vector3.zero, 0.2f);
    }
    
    private void HandleTripleMatch() {
        remainingPieces -= 3;
        if (remainingPieces <= 0) {
            WinGame();
        }
    }
    
    private void HandleTrayFull() {
        LoseGame();
    }
    
    private void WinGame() {
        gameActive = false;
        winScreen.SetActive(true);
        endScreen.SetActive(true);
    }

    private void LoseGame() {
        gameActive = false;
        loseScreen.SetActive(true);
        endScreen.SetActive(true);
    }
    
    public void ReshuffleBoard() {
        if (!gameActive || pieceSpawner.IsSpawning) return;
        
        actionBar.ResetBar();
        pieceSpawner.ClearAllPieces();
        
        remainingPieces = pieceSpawner.SpawnPieces(remainingPieces / 3);
    }
    
    public void RestartGame() {
        pieceSpawner.ClearAllPieces();
        StartGame();
    }
}
