using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Quaternion myQuat;
    private Color hover = Color.green;
    private Color normal;
    private Color selected = Color.cyan;
    private Material material = null;
    [SerializeField] private TextMeshPro inputArea = null;
    [SerializeField] private TextMeshPro real = null;
    private void Start()
    {
        myQuat = Quaternion.Euler(transform.eulerAngles);
        material = GetComponent<Renderer>().material;
        normal = material.color;
    }
    private void OnMouseEnter()
    {
        StopCoroutine(nameof(Do_Origin));
        material.color = hover;
    }
    private void OnMouseOver()
    {
        Do_Rotate();
        if (Input.GetMouseButtonDown(0) && real.alpha == 0)
        {
            material.color = selected;
        }
    }
    private void OnMouseExit()
    {
        StartCoroutine(Do_Origin());
        material.color = normal;
    }
    private void Update()
    {
        if (material.color != selected) return;

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                switch (key)
                {
                    case KeyCode.Alpha1:
                    case KeyCode.Keypad1:
                        {
                            inputArea.text = 1.ToString();
                            break;
                        }
                    case KeyCode.Alpha2:
                    case KeyCode.Keypad2:
                        {
                            inputArea.text = 2.ToString();
                            break;
                        }
                    case KeyCode.Alpha3:
                    case KeyCode.Keypad3:
                        {
                            inputArea.text = 3.ToString();
                            break;
                        }
                    case KeyCode.Alpha4:
                    case KeyCode.Keypad4:
                        {
                            inputArea.text = 4.ToString();
                            break;
                        }
                    case KeyCode.Alpha5:
                    case KeyCode.Keypad5:
                        {
                            inputArea.text = 5.ToString();
                            break;
                        }
                    case KeyCode.Alpha6:
                    case KeyCode.Keypad6:
                        {
                            inputArea.text = 6.ToString();
                            break;
                        }
                    case KeyCode.Alpha7:
                    case KeyCode.Keypad7:
                        {
                            inputArea.text = 7.ToString();
                            break;
                        }
                    case KeyCode.Alpha8:
                    case KeyCode.Keypad8:
                        {
                            inputArea.text = 8.ToString();
                            break;
                        }
                    case KeyCode.Alpha9:
                    case KeyCode.Keypad9:
                        {
                            inputArea.text = 9.ToString();
                            break;
                        }
                    case KeyCode.Alpha0:
                    case KeyCode.Keypad0:
                        {
                            inputArea.text = "";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
    }
    private void Do_Rotate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 10f);
    }
    private IEnumerator Do_Origin()
    {
        while (true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, myQuat, Time.deltaTime * 10f);
            yield return new WaitForEndOfFrame();
        }
    }
}
