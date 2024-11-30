using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.MVC
{
    public class BaseView : MonoBehaviour
    {
        public int ViewId { get;set; }
        public BaseController Controller { get; set; }
        protected Canvas canvas;
        protected Dictionary<string,GameObject> viewObjs = new Dictionary<string,GameObject>();
        private bool _isInit = false;//�Ƿ��ʼ��
        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        protected virtual void OnAwake()
        {

        }

        protected virtual void OnStart()
        {
            
        }

        public void ApplyControllerFunc(int controllerKey,string eventName,params object[] args)
        {
            Controller.ApplyControllerFunc(controllerKey, eventName, args);
        }

        public void ApplyFunc(string eventName,params object[] args)
        {
            Controller.ApplyFunc(eventName, args);
        }

        public virtual void Close(params object[] args)
        {
            SetVisible(false);//���ص�ǰ��ͼ
        }

        public void DestroyView()
        {
            Controller = null;
            Destroy(gameObject);
        }

        public virtual void InitData()
        {
            _isInit = true;
        }

        public virtual void InitUI()
        {

        }

        public bool IsInit => _isInit; //��ͼ�Ƿ��ʼ��

        public bool IsShow => canvas.enabled == true; //�Ƿ���ʾ

        public virtual void Open(params object[] args)
        {

        }

        public void SetVisible(bool value)
        {
            canvas.enabled = value;
        }

        public GameObject Find(string res)
        {
            if (viewObjs.ContainsKey(res))
            {
                return viewObjs[res];
            }
            else
            {
                viewObjs.Add(res, transform.Find(res).gameObject);
                return viewObjs[res];
            }
        }

        public T Find<T>(string res) where T : Component
        {
            return Find(res).GetComponent<T>();
        }
    }
}
