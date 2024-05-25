using System.ComponentModel;
using System.Reflection;

namespace Domain.Extensions
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string value) where T : Enum
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (item.ToString() == value)
                    return item;
            }

            throw new ArgumentException($"String '{value}' do not correspond to a enum value {typeof(T).Name}.");
        }

        private static string GetDescription<T>(this T enumerationValue) where T : Enum
        {
            Type type = enumerationValue.GetType();
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return enumerationValue.ToString();
        }
    }
}
