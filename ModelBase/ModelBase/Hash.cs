﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ModelBase
{
  [Serializable]
  public struct Hash : ICloneable
  {
    public const int Size = 16;
    private readonly byte[] data;

    public Hash(byte[] source)
    {
      data = new byte[Size];
      Buffer.BlockCopy(source, 0, data, 0, Size);
    }

    public Hash(Hash h) : this(h.data) {}

    public Hash(byte[] buf, int offset)
    {
      data = new byte[Size];
      Buffer.BlockCopy(buf, offset, data, 0, Size);
    }

    public Hash(string s)
    {
      data = StringToBytes(s);
    }

    #region ICloneable Members

    public object Clone()
    {
      return new Hash(this);
    }

    #endregion

    public override bool Equals(object obj)
    {
      if (obj == null || !(obj is Hash)) return false;
      var h = (Hash) obj;
      return ByteArrayEquals(data, h.data);
    }

    public override int GetHashCode()
    {
      int hash = 0;
      for (int i = 0; i < data.Length; ++i) hash += data[i];
      return hash;
    }

    public override string ToString()
    {
      return BytesToString(data);
    }

    public static explicit operator byte[](Hash h)
    {
      return h.data;
    }

    public static explicit operator string(Hash h)
    {
      return h.ToString();
    }

    public static explicit operator Hash(byte[] v)
    {
      return new Hash(v);
    }

    public static explicit operator Hash(string v)
    {
      return new Hash(v);
    }

    public static bool operator ==(Hash a, Hash b)
    {
      return ByteArrayEquals(a.data, b.data);
    }

    public static bool operator !=(Hash a, Hash b)
    {
      return !(a == b);
    }




    private static void HashFolderRecursive(DirectoryInfo folder, string path, SortedList<string, FileInfo> entries)
    {
      foreach (DirectoryInfo di in folder.GetDirectories()) HashFolderRecursive(di, path + "/" + di.Name + "/", entries);
      foreach (FileInfo fi in folder.GetFiles()) entries.Add((path + fi.Name), fi);
    }



    public static Hash HashBytes(byte[] data)
    {
      return (Hash) new MD5CryptoServiceProvider().ComputeHash(data);
    }


    public static Hash HashStream(Stream fs)
    {
      return (Hash) new MD5CryptoServiceProvider().ComputeHash(fs);
    }

	public static Hash HashString(string data)
	{
		return (Hash) new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(data));
	}


    #region PRIVATE METHODS

    private static string BytesToString(byte[] bytes)
    {
      var str = new StringBuilder();

      for (int i = 0; i < bytes.Length; i++) str.AppendFormat("{0:x2}", bytes[i]);

      return str.ToString();
    }

    private static byte[] StringToBytes(string s)
    {
      Debug.Assert(s.Length%2 == 0);
      s.ToUpper();
      int destLen = s.Length/2;
      var res = new byte[destLen];
      int i = 0;
      int si = 0;

      while (i < destLen) res[i++] = (byte) ((CharToHexByte(s[si++]) << 4) + CharToHexByte(s[si++]));
      return res;
    }

    private static int CharToHexByte(char x)
    {
      if (x >= '0' && x <= '9') return x - '0';
      else {
        if (x >= 'a' && x <= 'f') return x - 'a' + 10;
        else if (x >= 'A' && x <= 'F') return x - 'A' + 10;
        else throw new ArgumentException("character is not convertible to hex byte");
      }
    }

    private static bool ByteArrayEquals(byte[] b1, byte[] b2)
    {
      if (b1.Length != b2.Length) return false;
      for (int i = 0; i < b1.Length; ++i) if (b1[i] != b2[i]) return false;
      return true;
    }

    private class MyStringCompare : Comparer<string>
    {
      public override int Compare(string x, string y)
      {
        return string.CompareOrdinal(x, y);
      }
    }

    #endregion
  }
}