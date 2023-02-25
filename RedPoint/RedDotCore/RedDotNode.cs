using System.Collections.Generic;
using UnityEngine;

namespace RedDotTutorial_1
{
    /// <summary>
    /// 红点数据节点
    /// </summary>
    public class RedDotNode
    {
        /// <summary>
        /// 红点类型
        /// </summary>
        public E_RedPointType  redPointType { get; set; }

        /// <summary>
        /// 红点计数
        /// </summary>
        public int RedCount { get; private set; }

        /// <summary>
        /// 父红点
        /// </summary>
        private RedDotNode parent;

        /// <summary>
        /// 发生变化的回调函数
        /// </summary>
        public RedDotSystem.OnRdCountChange countChangeFunc;

        /// <summary>
        /// 子红点的字典表
        /// </summary>
        public Dictionary<E_RedPointType, RedDotNode> rdChildrenDic = new Dictionary<E_RedPointType, RedDotNode>();


        #region 内部接口

        /// <summary>
        /// 重新计算该红点的计数
        /// </summary>
        private void CheckRedDotCount()
        {
            //该红点的计数 = 子红点的计数之和
            int num = 0;
            foreach (RedDotNode node in rdChildrenDic.Values)
                num += node.RedCount;

            //红点计数有变化
            if (num != RedCount)
            {
                RedCount = num;
                NotifyRedDotCountChange();
            }

            parent?.CheckRedDotCount();
        }

        /// <summary>
        /// 红点计数变化，通知
        /// </summary>
        private void NotifyRedDotCountChange()
        {
            countChangeFunc?.Invoke(this);
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 设置红点的数量
        /// </summary>
        /// <param name="rdCount"></param>
        public void SetRedDotCount(int rdCount)
        {
            //只能对非根节点进行设定
            if (rdChildrenDic.Count > 0)
            {
                Debug.LogWarning("不可直接设定根节点的红点数");
                return;
            }

            //设定该红点的计数
            RedCount = rdCount;

            NotifyRedDotCountChange();

            parent?.CheckRedDotCount();
        }
        /// <summary>
        /// 设置父红点
        /// </summary>
        /// <param name="node"></param>
        public void SetParent(RedDotNode node)
        {
            parent = node;
        }

        #endregion
    }
}
