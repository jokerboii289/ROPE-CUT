using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraPivot;

    CinemachineVirtualCamera camera;
    
    float size;
   
    float heightRatio;

    //when all ring are finished
    public GameObject secondCamera;
   // [SerializeField] Transform position;
    private void Start()
    {
        secondCamera.SetActive(false);
        camera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame    
    private void LateUpdate()
    {
        heightRatio = (.027f / (1 / camera.m_Lens.FieldOfView));
     
        CameraSizeLerp();
        transform.position = Vector3.Lerp(transform.position, new Vector3(cameraPivot.transform.position.x-.35f, cameraPivot.transform.position.y-heightRatio, transform.position.z),3*Time.deltaTime);     
    }

    void CameraSizeLerp()
    {
        //if (size <= 2)
        //    camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, 70, 2 * Time.deltaTime);
        //if (size > 2 && size <= 4)
        //    camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, 80, 2 * Time.deltaTime);
        //if (size > 4 && size <= 6)
        //    camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, 90, 2 * Time.deltaTime);
        //if (size > 6 && size <= 10)
        //    camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, 100, 2 * Time.deltaTime);
        //if (size > 10 && size <= 15)
        //    camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, 110, 2 * Time.deltaTime);
        //if (size > 15)
        //    camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, 120, 2 * Time.deltaTime);

        if (size <= 2)
            camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, 70, .5f * Time.deltaTime);
        if (size > 2 && size <= 4)
            camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, 80, .5f * Time.deltaTime);
        if (size > 4 && size <= 6)
            camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, 90, .5f * Time.deltaTime);
        if (size > 6 && size <= 10)
            camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, 100, .5f * Time.deltaTime);
        if (size > 10 && size <= 15)
            camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, 110, .5f * Time.deltaTime);
        if (size > 15)
            camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, 120, .5f * Time.deltaTime);
    }


    public void GetSize(int size)
    {
        this.size = size;
    }
    
    public IEnumerator RingDisappeared()
    {
        yield return new WaitForSeconds(0f);
        secondCamera.SetActive(true);
    }
}
