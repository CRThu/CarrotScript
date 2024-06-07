using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Impl.Parser
{
    /// <summary>
    /// 树
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NodeBase<T>
    {
        /// <summary>
        /// 树节点值
        /// </summary>
        public T Value { get; set; }
        /// <summary>
        /// 树的左分支
        /// </summary>
        public NodeBase<T>? Left { get; set; }
        /// <summary>
        /// 树的右分支
        /// </summary>
        public NodeBase<T>? Right { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public NodeBase(T value, NodeBase<T>? left = null, NodeBase<T>? right = null)
        {
            Value = value;
            Left = left;
            Right = right;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        public NodeBase(T value)
        {
            Value = value;
        }
    }
}
