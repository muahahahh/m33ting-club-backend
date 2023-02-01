﻿using System.Reflection;
using M33tingClub.Domain.Utilities.Exceptions;

namespace M33tingClub.Domain.Utilities;

public abstract class ValueObject
{
    private List<PropertyInfo> _properties;

    private List<FieldInfo> _fields;

    public static bool operator ==(ValueObject obj1, ValueObject obj2)
    {
        if (Equals(obj1, null))
        {
            if (Equals(obj2, null)) return true;

            return false;
        }

        return obj1.Equals(obj2);
    }

    public static bool operator !=(ValueObject obj1, ValueObject obj2)
    {
        return !(obj1 == obj2);
    }

    public bool Equals(ValueObject obj)
    {
        return Equals(obj as object);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        return GetProperties().All(p => PropertiesAreEqual(obj, p))
               && GetFields().All(f => FieldsAreEqual(obj, f));
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 29;
            foreach (var prop in GetProperties())
            {
                var value = prop.GetValue(this, null);
                hash = HashValue(hash, value);
            }

            foreach (var field in GetFields())
            {
                var value = field.GetValue(this);
                hash = HashValue(hash, value);
            }

            return hash;
        }
    }

    private bool PropertiesAreEqual(object obj, PropertyInfo p)
    {
        return Equals(p.GetValue(this, null), p.GetValue(obj, null));
    }

    private bool FieldsAreEqual(object obj, FieldInfo f)
    {
        return Equals(f.GetValue(this), f.GetValue(obj));
    }

    private IEnumerable<PropertyInfo> GetProperties()
    {
        if (_properties is null)
            _properties = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToList();

        return _properties;
    }

    private IEnumerable<FieldInfo> GetFields()
    {
        if (_fields is null)
            _fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToList();

        return _fields;
    }

    private int HashValue(int seed, object value)
    {
        var currentHash = value?.GetHashCode() ?? 0;

        return seed * 13 + currentHash;
    }

    protected void CheckRule(IRule rule)
    {
        if (rule.IsBroken()) 
            throw new RuleValidationException(rule);
    }
}