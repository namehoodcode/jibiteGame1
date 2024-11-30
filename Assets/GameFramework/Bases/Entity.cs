using System;
using UnityEngine;

//ʵ�����ݣ�Ѫ�������ٵ�
namespace GameFramework.Bases
{
    public class Entity : MonoBehaviour,IEntity
    {
        protected Rigidbody2D Rb;
        public event Func<float, bool> CommandUpdates;
        public virtual void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
        }

        public virtual void Update()
        {
            CommandUpdates?.Invoke(Time.deltaTime);
        }

    }
}
