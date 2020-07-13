// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Components.RenderTree
{
    // Special subclass of ArrayBuilder<T> that contains methods optimized for
    // appending render tree frames

    internal class RenderTreeArrayBuilder : ArrayBuilder<RenderTreeFrame>
    {
        public void AppendElement(int sequence, string elementName)
        {
            if (_itemsInUse == _items.Length)
            {
                GrowBuffer(_items.Length * 2);
            }

            ref var item = ref _items[_itemsInUse++];

            item.Sequence = sequence;
            item.FrameType = RenderTreeFrameType.Element;
            item.ElementName = elementName;
        }

        public void AppendText(int sequence, string text)
        {
            if (_itemsInUse == _items.Length)
            {
                GrowBuffer(_items.Length * 2);
            }

            ref var item = ref _items[_itemsInUse++];

            item.Sequence = sequence;
            item.FrameType = RenderTreeFrameType.Text;
            item.TextContent = text;
        }

        public void AppendRegion(int sequence)
        {
            if (_itemsInUse == _items.Length)
            {
                GrowBuffer(_items.Length * 2);
            }

            ref var item = ref _items[_itemsInUse++];

            item.Sequence = sequence;
            item.FrameType = RenderTreeFrameType.Region;
        }
    }
}
