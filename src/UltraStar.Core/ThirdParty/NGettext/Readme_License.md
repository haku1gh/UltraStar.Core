﻿# NGettext

All files in this folder and sub folders contain the NGettext library in version 0.6.5.

The original source can be found here: https://github.com/VitaliiTsilnyk/NGettext/tree/0.6.5.

The following modifications had been done to the files:
- Namespace had been adapted from NGettext\* to UltraStar.Core.ThirdParty.NGettext\*
- Added a couple of empty XML tags to get rid of Warning CS1591 "Missing XML comment for publicly visible type or member"
- Added a pragma disable for Warning CS3021 "Type or member does not need a CLSCompliant attribute because the assembly
  does not have a CLSCompliant attribute" in file BigEndianBinaryReader.cs

# Original License of NGettext

NGettext is licensed under the MIT license:

The MIT License (MIT)

Copyright (c) 2012 Vitaly Zilnik

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
