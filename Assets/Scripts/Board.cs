using UnityEngine;

public class Board : MonoBehaviour
{
    public void Initialize(int boardSize) 
    {
        var board = transform.Find("Quad");
        board.localScale = new Vector3(boardSize, boardSize, 1);
        board.localPosition = new Vector3(boardSize / 2 - 0.5f, 0, boardSize / 2 - 0.5f);

        board.GetComponent<Renderer>().material.SetFloat("_Tiles", boardSize / 2);

        var camera = Camera.main;
        camera.transform.position = new Vector3(boardSize / 2 - 0.5f, boardSize, boardSize / 2 - 0.5f);
        camera.orthographicSize = boardSize / 2 + 1;
    }
}
