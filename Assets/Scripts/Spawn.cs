using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public List<GameObject> tetrominoes;
    public Transform previewPosition;

    private GameObject _nextTetromino;
    private GameObject _previewTetromino;

    void Start()
    {
        PreviewTetromino();
        NewTetromino();
    }

    public void NewTetromino()
    {
        Instantiate(_nextTetromino, transform.position, Quaternion.identity);
        PreviewTetromino();
    }

    public void PreviewTetromino()
    {
        Destroy(_previewTetromino);

        _nextTetromino = tetrominoes[Random.Range(0, tetrominoes.Count)];
        _previewTetromino = Instantiate(_nextTetromino, transform.position, Quaternion.identity);

        Destroy(_previewTetromino.GetComponent<TetrominoMovement>());

        _previewTetromino.transform.position = previewPosition.position;
    }
}
