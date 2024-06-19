using UnityEngine;

public class Board : MonoBehaviour
{
    public void SetSize(int boardSize)
    {
        var camera = Camera.main;
        camera.orthographicSize = (boardSize / 2) + 1;
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -.5f);

        transform.localScale = new Vector3(boardSize, 1, boardSize);
    }
}
