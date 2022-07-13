using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;


public class MultiRecognize : MonoBehaviour
{
    public Text debug1;
    public Text debug2;
    public Text debug3;


    ARTrackedImageManager trackImange;
    //collection -> Dictionary
    // ::> array/list/stack/queue/dictionary
    //    index[    ]/foreach(in)/key:value
    Dictionary<string, GameObject> spawnObj;
    public GameObject[] multi_prefab;//이미지 인식시 사용할 프리펩 객체 정보



    private void Awake()
    {
        //매니저를 얻어옴
        trackImange = this.GetComponent<ARTrackedImageManager>();
        // 해당 이벤트가 발생할 때 호출할 함수를 등록
        //객체를 만들고 => 이름으로 접근 -> Key, data형식의 자료 => collection -> Dictionary
        spawnObj = new Dictionary<string, GameObject>();

        //dictionary에 내가 만든 key, value => 프리펩을 넣겠다. => 내가 만든 프리펩 객체
        foreach (GameObject obj in multi_prefab)
        {
            GameObject temp = Instantiate(obj);
            debug1.text = "검출됨";
            temp.name = obj.name;
            temp.SetActive(false);
            spawnObj.Add(temp.name,temp);//이름, 그 객체 그 자체 => dictionary타입에 넣는다.
        }
        
        /* ==> 위와 같은 문법....
        for (int i = 0; i < multi_prefab.Length; i++)
        {
            GameObject temp = Instantiate(multi_prefab[i]);
        }
        */
        
    }
    private void OnEnable()
    {
        trackImange.trackedImagesChanged += OnTackedImageChange;
    }
    private void OnDisable()
    {
        trackImange.trackedImagesChanged -= OnTackedImageChange;

    }
    void OnTackedImageChange(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage tracked_img in args.added) //찾는 이미지가 있을 때
        {
            //객체를 활성화하고 맞춰서 좌표변경
            UpdateObj(tracked_img);
        }
        foreach (ARTrackedImage tracked_img in args.updated) //찾는 이미지가 갱신 됐을 때
        {

            //객체를 활성화하고 맞춰서 좌표변경
            UpdateObj(tracked_img);


        }
        foreach (ARTrackedImage tracked_img in args.removed)//있다가 찾는 이미지가  없을 때
        {
            debug2.text = "삭제됨";
            //객체를 비활성화 //spawnObj[객체명]
            //if(tracked_img.name=="img1") spawnObj["Cube"].SetActive(false);라고 쓰는 것을 요약
            //if(tracked_img.name=="img2") spawnObj["Sphere"].SetActive(false);라고 쓰는 것을 요약
            spawnObj[tracked_img.referenceImage.name].SetActive(false);
        }


    }
    void UpdateObj(ARTrackedImage img)
    {
        string loadName = img.referenceImage.name;
        debug3.text = "이미지 이름" + loadName;
        spawnObj[loadName].SetActive(true);
        spawnObj[loadName].transform.position = img.transform.position;
        spawnObj[loadName].transform.rotation = img.transform.rotation;

    }

    /*
        //trackImange.trackedImagesChanged += ImageChange;
    void ImageChange(ARTrackedObjectsChangedEventArgs arg)
    { 
        
    }
    */
    //매니저에서 이벤트 발생시 호출할 함수를 등록
    //상황에 맞추어 사용
    //=> 해당되는 이벤트 상황에서 객체를 보여주고 안보여준다
    void Start()
    {

    }

    void Update()
    {
        debug2.text = "검출" + trackImange.trackables.count;
    }
}
