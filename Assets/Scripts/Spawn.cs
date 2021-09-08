using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public List<GameObject> tetrominoes;
    public Transform previewPosition;

    private int _nextTetromino;
    private GameObject _previewTetromino;

    void Start()
    {
        PreviewTetromino();
        NewTetromino();
    }

    public void NewTetromino()
    {
        Instantiate(tetrominoes[_nextTetromino], transform.position, Quaternion.identity);

        GameManager.Instance.AddStatistics(1, _nextTetromino);

        PreviewTetromino();
    }

    public void PreviewTetromino()
    {
        Destroy(_previewTetromino);

        _nextTetromino = Random.Range(0, tetrominoes.Count);
        _previewTetromino = Instantiate(tetrominoes[_nextTetromino], transform.position, Quaternion.identity);

        Destroy(_previewTetromino.GetComponent<TetrominoMovement>());

        _previewTetromino.transform.position = previewPosition.position - _previewTetromino.GetComponent<TetrominoMovement>().rotationPoint;
    }
}
