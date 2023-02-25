using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedDotTutorial_1
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ParentTypeAttribute : Attribute
    {
        public E_RedPointType Parent { get; set; }
        public ParentTypeAttribute(E_RedPointType parent)
        {
            Parent = parent;
        }
    }
    //public static class EnumExtended
    //{
    //    /// <summary>
    //    /// 用于获取枚举类型的特性的泛型方法
    //    /// </summary>
    //    /// <typeparam name="T">特性类型</typeparam>
    //    /// <param name="e">枚举类型对象</param>
    //    /// <returns></returns>
    //    public static T GetTAttribute<T>(this Enum e) where T : Attribute
    //    {
    //        Type type = e.GetType();
    //        System.Reflection.FieldInfo field = type.GetField(e.ToString());
    //        if (field.IsDefined(typeof(T), true))
    //        {
    //            T attr = (T)field.GetCustomAttributesData(typeof(T));
    //            return attr;
    //        }
    //        return null;
    //    }
    //}
    /// <summary>
    /// 红点路径定义
    /// </summary>
    public enum E_RedPointType
    {
        Invalid = -1,
        Root = 0,
        MailBox = 1,
        [ParentType(MailBox)]
        MailBox_System =  1001,
        MailBox_Team = 1002,
    }

    /// <summary>
    /// 红点系统
    /// </summary>
    public class RedDotSystem
    {
        public RedDotSystem()
        {
            InitRedDotTreeNode();
            Debug.Log("--------------- 初始化 RedDotSystem 完毕 ---------------");
        }

        /// <summary>
        /// 红点数变化通知委托
        /// </summary>
        /// <param name="node"></param>
        public delegate void OnRdCountChange(RedDotNode node);

        /// <summary>
        /// 所有红点的集合
        /// </summary>
        private Dictionary<E_RedPointType, RedDotNode> allRedPointDic = new Dictionary<E_RedPointType, RedDotNode>();

        #region 内部接口

        /// <summary>
        /// 初始化红点树
        /// </summary>
        private void InitRedDotTreeNode()
        {
            AddOneRedPoint(E_RedPointType.Root);
            AddOneRedPoint(E_RedPointType.MailBox, E_RedPointType.Root);
            AddOneRedPoint(E_RedPointType.MailBox_System, E_RedPointType.MailBox);
            AddOneRedPoint(E_RedPointType.MailBox_Team, E_RedPointType.MailBox);
        }
        private void AddOneRedPoint(E_RedPointType child, E_RedPointType parents = E_RedPointType.Invalid)
        {
            RedDotNode childNode = null;
            if (allRedPointDic.ContainsKey(child))
            {
                childNode = allRedPointDic[child];
            }
            else
            {
                childNode = new RedDotNode { redPointType = child };
            }
            allRedPointDic.Add(child, childNode);
            if(parents == E_RedPointType.Invalid)
            {
                return;
            }
            RedDotNode parentsNode = GetRedNode(parents);
            if (parentsNode == null)
            {
                return;
            }
            childNode.SetParent(parentsNode);
            parentsNode.rdChildrenDic.Add(child, childNode);
        }
        /// <summary>
        /// 跟进类型获取红点对象
        /// </summary>
        /// <param name="redPointType"></param>
        /// <returns></returns>
        private RedDotNode GetRedNode(E_RedPointType redPointType)
        {
            if(allRedPointDic.ContainsKey(redPointType))
            {
                return allRedPointDic[redPointType];
            }
            return null;
        }
        #endregion

        #region 外部接口

        /// <summary>
        /// 设置红点数变化的回调
        /// </summary>
        /// <param name="strNode">红点路径，必须是 RedDotDefine </param>
        /// <param name="callBack">回调函数</param>
        public void SetRedDotNodeCallBack(E_RedPointType redPointType, OnRdCountChange callBack)
        {
            RedDotNode node = GetRedNode(redPointType);
            if(node == null)
            {
                return;
            }
            node.countChangeFunc = callBack;
        }

        /// <summary>
        /// 设置红点参数
        /// </summary>
        /// <param name="nodePath">红点路径，必须走 RedDotDefine </param>
        /// <param name="rdCount">红点计数</param>
        public void Set(E_RedPointType redPointType, int rdCount = 1)
        {
            RedDotNode node = GetRedNode(redPointType);
            if (node == null)
            {
                return;
            }
            node.SetRedDotCount(Math.Max(0, rdCount));
        }

        /// <summary>
        /// 获取红点的计数
        /// </summary>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        public int GetRedDotCount(E_RedPointType redPointType)
        {
            RedDotNode node = GetRedNode(redPointType);
            if (node == null)
            {
                return 0;
            }
            return node.RedCount;
        }
        #endregion
    }
}