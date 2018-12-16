using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace VsNerdX.Util
{
    public static class TreeHelper
    {
        public static void TraverseNode(object rootNode, Func<Object, bool> shouldExpand, Action<Object> expandableAction, Action<Object> notexpandableAction)
        {
            Stack<object> expandableNodes = new Stack<object>(20);

            if (!shouldExpand(rootNode))
            {
                return;
            }
            expandableNodes.Push(rootNode);

            while (expandableNodes.Count > 0)
            {
                object currentExpandableNode = expandableNodes.Pop();
                expandableAction?.Invoke(currentExpandableNode);

                var notExpandableNodes = new List<Object>();
                var childNodes = GetChildNodes(currentExpandableNode);

                if (childNodes.Count == 0)
                {
                    Object expandedItemsLoaded = null;
                    currentExpandableNode?.InvokeMethod("add_ExpandedItemsLoaded", new Object[]
                    {
                        expandedItemsLoaded = new EventHandler((e, a) =>
                        {
                            TraverseNode(currentExpandableNode, shouldExpand, expandableAction, notexpandableAction);
                            currentExpandableNode?.InvokeMethod("remove_ExpandedItemsLoaded", new Object[] { expandedItemsLoaded });
                        })
                    });

                    Object propertyChanged = null;
                    currentExpandableNode?.InvokeMethod("add_PropertyChanged", new Object[]
                    {
                        propertyChanged = new PropertyChangedEventHandler((e, a) =>
                        {
                            TraverseNode(currentExpandableNode, shouldExpand, expandableAction, notexpandableAction);
                            currentExpandableNode?.InvokeMethod("remove_PropertyChanged", new Object[] { propertyChanged });
                        })
                    });
                }

                foreach (var node in childNodes)
                {
                    if (shouldExpand(node))
                    {
                        expandableNodes.Push(node);
                    }
                    else
                    {
                        notExpandableNodes.Add(node);
                    }
                }

                foreach (var item in notExpandableNodes)
                {
                     notexpandableAction?.Invoke(item);
                }
            }
        }

        public static List<Object> GetChildNodes(this Object item)
        {
            var childNodes = item?.GetValue("ChildNodes");
            return ((IEnumerable) childNodes)?.Cast<Object>().ToList();
        }

        public static void ExpandNode(Object node)
        {
            node.SetProperty("IsExpanded", true);
        }

        public static void CollapseNode(Object node)
        {
            node.SetProperty("IsExpanded", false);
        }

        public static bool IsDirectory(Object item)
        {
            return Directory.Exists(GetCanonicalName(item));
        }

        public static bool IsFile(Object item)
        {
            return File.Exists(GetCanonicalName(item));
        }

        public static string GetCanonicalName(Object item)
        {
            return (string)item
                .GetValue("Item")
                .GetValue("SourceItem")
                .GetValue("CanonicalName");
        }

        public static bool IsExpandable(Object item)
        {
            return (bool?) item.GetValue("IsExpandable") ?? false;
        }

        public static Object GetValue(this Object item, string name)
        {
            return item.GetType().GetProperty(name)?.GetValue(item);
        }

        public static void SetProperty(this Object item, string name, Object value)
        {
            item.GetType().GetProperty(name).SetValue(item, value);
        }

        public static Object InvokeMethod(this Object item, string name, Object[] args)
        {
            return item.GetType().GetMethod(name)?.Invoke(item, args);
        }

    } 
}
