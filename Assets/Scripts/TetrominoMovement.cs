using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoMovement : MonoBehaviour
{
    public Vector3 rotationPoint;
    public float previusTime;
    public float fallTime = .8f;
    public static int width = 10;
    public static int height = 20;
    public float interval = .1f;

    private int _amountLines;
    private static Transform[,] _grid = new Transform[width, height];

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if(!ValidMove())
            {
                transform.position += new Vector3(1, 0, 0);
            }
            else
            {
                SoundManager.Instance.Play(Sounds.SoundName.TetrominoMove);
            }
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
            else
            {
                SoundManager.Instance.Play(Sounds.SoundName.TetrominoMove);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Z))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            }
            else
            {
                SoundManager.Instance.Play(Sounds.SoundName.TetrominoRotate);
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
            else
            {
                SoundManager.Instance.Play(Sounds.SoundName.TetrominoRotate);
            }
        }

        if (Time.time - previusTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                if (GameEnded())
                {
                    SoundManager.Instance.Play(Sounds.SoundName.Loose);

                    FindObjectOfType<LoseScreen>().CloseBlinds();

                    if(GameManager.Instance.score > PlayerPrefs.GetInt("topScore", 0))
                    {
                        PlayerPrefs.SetInt("topScore", GameManager.Instance.score);
                    }
                    this.enabled = false;
                }
                else
                {
                    SoundManager.Instance.Play(Sounds.SoundName.TetrominoLand);
                    
                    FindObjectOfType<Spawn>().NewTetromino();
                    transform.position += new Vector3(0, 1, 0);
                    this.enabled = false;
                    AddToGrid();
                    CheckForLines();
                }
            }

            previusTime = Time.time;
        }
    }

    private void CheckForLines()
    {
        _amountLines = 0;

        for(int i = height - 1; i >= 0; i--)
        {
            if(HasLine(i))
            {
                _amountLines++;
                GameManager.Instance.AddLines(1);
                StartCoroutine(DeleteLine(i));
            }
        }

        if(_amountLines > 0)
        {
            if (_amountLines == 4)
                SoundManager.Instance.Play(Sounds.SoundName.TetrisClear);
            else
                SoundManager.Instance.Play(Sounds.SoundName.Clear);
        }

        GameManager.Instance.AddScore(ScoreAmount(_amountLines));
    }

    public int ScoreAmount(int lines)
    {
        switch(lines)
        {
            case 1:
                return 40;
            case 2:
                return 100;
            case 3:
                return 300;
            case 4:
                return 1200;
            default:
                return 0;
        }
    }

    bool HasLine(int i)
    {
        for(int j = 0; j < width; j++)
        {
            if(_grid[j, i] == null)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            StartCoroutine(DelayForDelete(i, j));
        }

        yield return new WaitForSeconds(interval * (width / 2));
        RowDown(i);
    }

    private IEnumerator DelayForDelete(int i, int j)
    {
        if (j >= width / 2)
        {
            yield return new WaitForSeconds(interval * (j - width / 2));
        }
        else
        {
            yield return new WaitForSeconds(interval * (-j + width / 2));
        }

        Destroy(_grid[j, i].gameObject);
        _grid[j, i] = null;
    }

    private void RowDown(int i)
    {
        for(int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if(_grid[j, y] != null)
                {
                    _grid[j, y - 1] = _grid[j, y];
                    _grid[j, y] = null;
                    _grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    private void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            _grid[roundedX, roundedY] = children;
        }
    }

    bool ValidMove()
    {
        foreach(Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if(roundedX < 0  || roundedX >= width || roundedY < 0 || roundedY > height - 2)
            {
                return false;
            }

            if(_grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }

        return true;
    }

    bool GameEnded()
    {
        foreach (Transform children in transform)
        {
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedY >= height - 2)
            {
                return true;
            }
        }

        return false;
    }
}
