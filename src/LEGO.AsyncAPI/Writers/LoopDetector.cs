// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using System;
    using System.Collections.Generic;

    internal class LoopDetector
    {
        private readonly Dictionary<Type, Stack<object>> loopStacks = new Dictionary<Type, Stack<object>>();

        /// <summary>
        /// Maintain history of traversals to avoid stack overflows from cycles.
        /// </summary>
        /// <param name="key">Identifier used for current context.</param>
        /// <returns>If method returns false a loop was detected and the key is not added.</returns>
        public bool PushLoop<T>(T key)
        {
            Stack<object> stack;
            if (!this.loopStacks.TryGetValue(typeof(T), out stack))
            {
                stack = new Stack<object>();
                this.loopStacks.Add(typeof(T), stack);
            }

            if (!stack.Contains(key))
            {
                stack.Push(key);
                return true;
            }
            else
            {
                return false;  // Loop detected
            }
        }

        /// <summary>
        /// Exit from the context in cycle detection.
        /// </summary>
        public void PopLoop<T>()
        {
            if (this.loopStacks[typeof(T)].Count > 0)
            {
                this.loopStacks[typeof(T)].Pop();
            }
        }

        public void SaveLoop<T>(T loop)
        {
            if (!this.Loops.ContainsKey(typeof(T)))
            {
                this.Loops[typeof(T)] = new List<object>();
            }

            this.Loops[typeof(T)].Add(loop);
        }

        /// <summary>
        /// Gets list of Loops detected.
        /// </summary>
        /// <value>
        /// The loops.
        /// </value>
        public Dictionary<Type, List<object>> Loops { get; } = new Dictionary<Type, List<object>>();

        /// <summary>
        /// Reset loop tracking stack.
        /// </summary>
        internal void ClearLoop<T>()
        {
            this.loopStacks[typeof(T)].Clear();
        }
    }

}
