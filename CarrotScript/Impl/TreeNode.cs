using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Impl
{
    /// <summary>
    /// 二叉树
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNode<T>
    {
        /// <summary>
        /// 树节点值
        /// </summary>
        public T Value { get; set; }
        /// <summary>
        /// 树的左分支
        /// </summary>
        public TreeNode<T>? Left { get; set; }
        /// <summary>
        /// 树的右分支
        /// </summary>
        public TreeNode<T>? Right { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public TreeNode(T value, TreeNode<T>? left, TreeNode<T>? right)
        {
            Value = value;
            Left = left;
            Right = right;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        public TreeNode(T value)
        {
            Value = value;
        }
    }
}
