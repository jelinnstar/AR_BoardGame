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
    public GameObject[] multi_prefab;//�̹��� �νĽ� ����� ������ ��ü ����



    private void Awake()
    {
        //�Ŵ����� ����
        trackImange = this.GetComponent<ARTrackedImageManager>();
        // �ش� �̺�Ʈ�� �߻��� �� ȣ���� �Լ��� ���
        //��ü�� ����� => �̸����� ���� -> Key, data������ �ڷ� => collection -> Dictionary
        spawnObj = new Dictionary<string, GameObject>();

        //dictionary�� ���� ���� key, value => �������� �ְڴ�. => ���� ���� ������ ��ü
        foreach (GameObject obj in multi_prefab)
        {
            GameObject temp = Instantiate(obj);
            debug1.text = "�����";
            temp.name = obj.name;
            temp.SetActive(false);
            spawnObj.Add(temp.name,temp);//�̸�, �� ��ü �� ��ü => dictionaryŸ�Կ� �ִ´�.
        }
        
        /* ==> ���� ���� ����....
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
        foreach (ARTrackedImage tracked_img in args.added) //ã�� �̹����� ���� ��
        {
            //��ü�� Ȱ��ȭ�ϰ� ���缭 ��ǥ����
            UpdateObj(tracked_img);
        }
        foreach (ARTrackedImage tracked_img in args.updated) //ã�� �̹����� ���� ���� ��
        {

            //��ü�� Ȱ��ȭ�ϰ� ���缭 ��ǥ����
            UpdateObj(tracked_img);


        }
        foreach (ARTrackedImage tracked_img in args.removed)//�ִٰ� ã�� �̹�����  ���� ��
        {
            debug2.text = "������";
            //��ü�� ��Ȱ��ȭ //spawnObj[��ü��]
            //if(tracked_img.name=="img1") spawnObj["Cube"].SetActive(false);��� ���� ���� ���
            //if(tracked_img.name=="img2") spawnObj["Sphere"].SetActive(false);��� ���� ���� ���
            spawnObj[tracked_img.referenceImage.name].SetActive(false);
        }


    }
    void UpdateObj(ARTrackedImage img)
    {
        string loadName = img.referenceImage.name;
        debug3.text = "�̹��� �̸�" + loadName;
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
    //�Ŵ������� �̺�Ʈ �߻��� ȣ���� �Լ��� ���
    //��Ȳ�� ���߾� ���
    //=> �ش�Ǵ� �̺�Ʈ ��Ȳ���� ��ü�� �����ְ� �Ⱥ����ش�
    void Start()
    {

    }

    void Update()
    {
        debug2.text = "����" + trackImange.trackables.count;
    }
}
