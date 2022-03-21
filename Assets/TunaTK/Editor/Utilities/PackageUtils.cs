// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 2021/10/10 6:04 PM

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using UnityEditor;
using UnityEditor.PackageManager;

using UnityEngine;

namespace TunaTK
{
	public static class PackageUtils
	{
		private static readonly Regex regPackageID = new Regex("^([^@]+)@([^#]+)(#(.+))?$", RegexOptions.Compiled);

		private delegate Dictionary<string, string> DictionaryDelegate(Dictionary<string, object> _dictionary);

		public static Dictionary<string, string> CheckTunacornPackages()
		{
			return FindTunacornPackages("Packages/manifest.json", _jsonDic =>
			{
				Dictionary<string, object> dependencies = _jsonDic["dependencies"] as Dictionary<string, object>;
				Dictionary<string, string> copied = new Dictionary<string, string>();

				foreach(KeyValuePair<string, object> dependency in dependencies!)
				{
					if(dependency.Key.Contains("tunacornstudios"))
						copied.Add(dependency.Key, dependency.Value as string);
				}

				return copied;
			});
		}

		public static void InstallPackage(string _packageName, string _url)
		{
			UpdatePackageJson("Packages/manifest.json", _jsonDic =>
			{
				// Add to dependencies.
				Dictionary<string, object> dependencies = _jsonDic["dependencies"] as Dictionary<string, object>;
				dependencies?.Add(_packageName, _url);
			});
		}

		public static void UninstallPackage(string _packageName)
		{
			UpdatePackageJson("Packages/manifest.json", _jsonDic =>
			{
				// Remove from dependencies.
				Dictionary<string, object> dependencies = _jsonDic["dependencies"] as Dictionary<string, object>;
				dependencies?.Remove(_packageName);

				// Unlock git revision.
				if(_jsonDic.TryGetValue("lock", out object locks))
					(locks as Dictionary<string, object>)?.Remove(_packageName);
			});

			UpdatePackageJson("Packages/packages-lock.json", _jsonDic =>
			{
				// Unlock git revision.
				Dictionary<string, object> dependencies = _jsonDic["dependencies"] as Dictionary<string, object>;
				dependencies?.Remove(_packageName);
			});
		}

		private static void UpdatePackageJson(string _path, Action<Dictionary<string, object>> _action)
		{
			if(!File.Exists(_path))
				return;

			try
			{
				Dictionary<string, object> jsonDic = Json.Deserialize(File.ReadAllText(_path)) as Dictionary<string, object>;

				if(jsonDic != null && _action != null)
					_action(jsonDic);

				// Save manifest.json.
				File.WriteAllText(_path, Json.Serialize(jsonDic, true));

				EditorApplication.delayCall += () =>
				{
				#if UNITY_2020_2_OR_NEWER
					Client.Resolve();
				#else
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
				#endif
				};
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}
		}

		private static Dictionary<string, string> FindTunacornPackages(string _path, DictionaryDelegate _delegate)
		{
			if(!File.Exists(_path))
				return new Dictionary<string, string>();

			try
			{
				// ReSharper disable once UsePatternMatching
				Dictionary<string, object> jsonDic = Json.Deserialize(File.ReadAllText(_path)) as Dictionary<string, object>;

				if(jsonDic != null && _delegate != null)
					return _delegate(jsonDic);
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}

			return new Dictionary<string, string>();
		}

		/// <summary>
		/// Get repo url for package from package id.
		/// </summary>
		/// <param name="_packageId">Package id([packageName]@[repoUrl]#[ref])</param>
		/// <returns>Repo url</returns>
		public static string GetRepoUrl(string _packageId) => string.IsNullOrEmpty(_packageId) ? "" : $"https://gitlab.com/tunacorn-studios/Tuna-TK/{_packageId}.git";

		public static void SplitPackageId(string _packageId, out string _packageName, out string _repoUrl, out string _refName)
		{
			Match m = regPackageID.Match(_packageId);
			_packageName = m.Groups[1].Value;
			_repoUrl = m.Groups[2].Value;
			_refName = m.Groups[4].Value;
		}

		public static class Json
		{
			/// <summary>
			/// Parses the string json into a value
			/// </summary>
			/// <param name="_json">A JSON string.</param>
			/// <returns>An List&lt;object&gt;, a Dictionary&lt;string, object&gt;, a double, an integer,a string, null, true, or false</returns>
			public static object Deserialize(string _json) => _json == null ? null : Parser.Parse(_json);

			private sealed class Parser : IDisposable
			{
				private const string WORD_BREAK = "{}[],:\"";

				private static bool IsWordBreak(char _c) => char.IsWhiteSpace(_c) || WORD_BREAK.IndexOf(_c) != -1;

				private enum Token
				{
					None,
					CurlyOpen,
					CurlyClose,
					SquaredOpen,
					SquaredClose,
					Colon,
					Comma,
					String,
					Number,
					True,
					False,
					Null
				}

				private StringReader json;

				private Parser(string _json) => json = new StringReader(_json);

				public static object Parse(string _json)
				{
					using(Parser instance = new Parser(_json))
						return instance.ParseValue();
				}

				public void Dispose()
				{
					json.Dispose();
					json = null;
				}

				private Dictionary<string, object> ParseObject()
				{
					Dictionary<string, object> table = new Dictionary<string, object>();

					// ditch opening brace
					json.Read();

					// {
					while(true)
					{
						// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
						switch(NextToken)
						{
							case Token.None:
								return null;
							case Token.Comma:
								continue;
							case Token.CurlyClose:
								return table;
							default:
								// name
								string name = ParseString();
								if(name == null)
									return null;

								// :
								if(NextToken != Token.Colon)
									return null;

								// ditch the colon
								json.Read();

								// value
								table[name] = ParseValue();
								break;
						}
					}
				}

				private List<object> ParseArray()
				{
					List<object> array = new List<object>();

					// ditch opening bracket
					json.Read();

					// [
					bool parsing = true;
					while(parsing)
					{
						Token nextToken = NextToken;

						// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
						switch(nextToken)
						{
							case Token.None:
								return null;
							case Token.Comma:
								continue;
							case Token.SquaredClose:
								parsing = false;
								break;
							default:
								object value = ParseByToken(nextToken);

								array.Add(value);
								break;
						}
					}

					return array;
				}

				private object ParseValue()
				{
					Token nextToken = NextToken;
					return ParseByToken(nextToken);
				}

				private object ParseByToken(Token _token) => _token switch
				{
					Token.String => ParseString(),
					Token.Number => ParseNumber(),
					Token.CurlyOpen => ParseObject(),
					Token.SquaredOpen => ParseArray(),
					Token.True => true,
					Token.False => false,
					Token.Null => null,
					_ => null
				};

				private string ParseString()
				{
					StringBuilder s = new StringBuilder();

					// ditch opening quote
					json.Read();

					bool parsing = true;
					while(parsing)
					{
						if(json.Peek() == -1)
							break;

						char c = NextChar;
						switch(c)
						{
							case '"':
								parsing = false;
								break;
							case '\\':
								if(json.Peek() == -1)
								{
									parsing = false;
									break;
								}

								c = NextChar;
								switch(c)
								{
									case '"':
									case '\\':
									case '/':
										s.Append(c);
										break;
									case 'b':
										s.Append('\b');
										break;
									case 'f':
										s.Append('\f');
										break;
									case 'n':
										s.Append('\n');
										break;
									case 'r':
										s.Append('\r');
										break;
									case 't':
										s.Append('\t');
										break;
									case 'u':
										char[] hex = new char[4];

										for(int i = 0; i < 4; i++)
											hex[i] = NextChar;

										s.Append((char)Convert.ToInt32(new string(hex), 16));
										break;
								}

								break;
							default:
								s.Append(c);
								break;
						}
					}

					return s.ToString();
				}

				private object ParseNumber()
				{
					string number = NextWord;

					if(number.IndexOf('.') == -1)
					{
						long.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out long parsedInt);
						return parsedInt;
					}

					double.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedDouble);
					return parsedDouble;
				}

				private void EatWhitespace()
				{
					while(char.IsWhiteSpace(PeekChar))
					{
						json.Read();

						if(json.Peek() == -1)
						{
							break;
						}
					}
				}

				private char PeekChar => Convert.ToChar(json.Peek());

				private char NextChar => Convert.ToChar(json.Read());

				private string NextWord
				{
					get
					{
						StringBuilder word = new StringBuilder();

						while(!IsWordBreak(PeekChar))
						{
							word.Append(NextChar);

							if(json.Peek() == -1)
							{
								break;
							}
						}

						return word.ToString();
					}
				}

				private Token NextToken
				{
					get
					{
						EatWhitespace();

						if(json.Peek() == -1)
						{
							return Token.None;
						}

						switch(PeekChar)
						{
							case '{':
								return Token.CurlyOpen;
							case '}':
								json.Read();
								return Token.CurlyClose;
							case '[':
								return Token.SquaredOpen;
							case ']':
								json.Read();
								return Token.SquaredClose;
							case ',':
								json.Read();
								return Token.Comma;
							case '"':
								return Token.String;
							case ':':
								return Token.Colon;
							case '0':
							case '1':
							case '2':
							case '3':
							case '4':
							case '5':
							case '6':
							case '7':
							case '8':
							case '9':
							case '-':
								return Token.Number;
						}

						return NextWord switch
						{
							"false" => Token.False,
							"true" => Token.True,
							"null" => Token.Null,
							_ => Token.None
						};
					}
				}
			}

			/// <summary>
			/// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
			/// </summary>
			/// <param name="_obj"></param>
			/// <param name="_pretty">A boolean to indicate whether or not JSON should be prettified, default is false.</param>
			/// <param name="_indentText">A string to ibe used as indentText, default is 2 spaces.</param>
			/// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
			public static string Serialize(object _obj, bool _pretty = false, string _indentText = "  ") => Serializer.Serialize(_obj, _pretty, _indentText);

			private sealed class Serializer
			{
				private readonly string indentText;
				private readonly bool pretty;
				private readonly StringBuilder builder;

				private Serializer(bool _pretty, string _indentText)
				{
					builder = new StringBuilder();
					pretty = _pretty;
					indentText = _indentText;
				}

				[SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
				internal static string Serialize(object _obj, bool _pretty, string _indentText)
				{
					Serializer instance = new Serializer(_pretty, _indentText);

					instance.SerializeValue(_obj, 0);

					return instance.builder.ToString();
				}

				private void SerializeValue(object _value, int _indent)
				{
					IList asList;
					IDictionary asDict;
					string asStr;

					if(_value == null || _value is Delegate)
					{
						builder.Append("null");
					}
					else if((asStr = _value as string) != null)
					{
						SerializeString(asStr);
					}
					else if(_value is bool boolVal)
					{
						builder.Append(boolVal ? "true" : "false");
					}
					else if((asList = _value as IList) != null)
					{
						SerializeArray(asList, _indent);
					}
					else if((asDict = _value as IDictionary) != null)
					{
						SerializeObject(asDict, _indent);
					}
					else if(_value is char charVal)
					{
						SerializeString(new string(charVal, 1));
					}
					else
					{
						SerializeOther(_value, _indent);
					}
				}

				private void SerializeObject(IDictionary _obj, int _indent)
				{
					if(pretty && _obj.Keys.Count == 0)
					{
						builder.Append("{}");
						return;
					}

					bool first = true;
					string indentLine = null;

					builder.Append('{');
					if(pretty)
					{
						builder.Append('\n');
						indentLine = string.Concat(Enumerable.Repeat(indentText, _indent).ToArray());
					}

					foreach(object e in _obj.Keys)
					{
						if(!first)
						{
							builder.Append(',');
							if(pretty)
								builder.Append('\n');
						}

						if(pretty)
						{
							builder.Append(indentLine);
							builder.Append(indentText);
						}

						SerializeString(e.ToString());
						builder.Append(':');
						if(pretty)
							builder.Append(' ');

						SerializeValue(_obj[e], _indent + 1);

						first = false;
					}

					if(pretty)
					{
						builder.Append('\n');
						builder.Append(indentLine);
					}

					builder.Append('}');
				}

				private void SerializeArray(IList _array, int _indent)
				{
					if(pretty && _array.Count == 0)
					{
						builder.Append("[]");
						return;
					}

					bool first = true;
					string indentLine = null;

					builder.Append('[');
					if(pretty)
					{
						builder.Append('\n');
						indentLine = string.Concat(Enumerable.Repeat(indentText, _indent).ToArray());
					}

					foreach(object obj in _array)
					{
						if(!first)
						{
							builder.Append(',');
							if(pretty)
								builder.Append('\n');
						}

						if(pretty)
						{
							builder.Append(indentLine);
							builder.Append(indentText);
						}

						SerializeValue(obj, _indent + 1);

						first = false;
					}

					if(pretty)
					{
						builder.Append('\n');
						builder.Append(indentLine);
					}

					builder.Append(']');
				}

				private void SerializeString(string _str)
				{
					builder.Append('\"');

					char[] charArray = _str.ToCharArray();
					foreach(char c in charArray)
					{
						switch(c)
						{
							case '"':
								builder.Append("\\\"");
								break;
							case '\\':
								builder.Append("\\\\");
								break;
							case '\b':
								builder.Append("\\b");
								break;
							case '\f':
								builder.Append("\\f");
								break;
							case '\n':
								builder.Append("\\n");
								break;
							case '\r':
								builder.Append("\\r");
								break;
							case '\t':
								builder.Append("\\t");
								break;
							default:
								int codepoint = Convert.ToInt32(c);
								if((codepoint >= 32) && (codepoint <= 126))
								{
									builder.Append(c);
								}
								else
								{
									builder.Append("\\u");
									builder.Append(codepoint.ToString("x4"));
								}

								break;
						}
					}

					builder.Append('\"');
				}

				private void SerializeOther(object _value, int _indent)
				{
					// NOTE: decimals lose precision during serialization.
					// They always have, I'm just letting you know.
					// Previously floats and doubles lost precision too.
					if(_value is float floatVal)
					{
						builder.Append(floatVal.ToString("R", CultureInfo.InvariantCulture));
					}
					else if(_value is int || _value is uint || _value is long || _value is sbyte || _value is byte || _value is short || _value is ushort || _value is ulong)
					{
						builder.Append(_value);
					}
					else if(_value is double || _value is decimal)
					{
						builder.Append(Convert.ToDouble(_value).ToString("R", CultureInfo.InvariantCulture));
					}
					else
					{
						Dictionary<string, object> map = new Dictionary<string, object>();
						List<FieldInfo> fields = _value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).ToList();
						fields.ForEach(_field => map.Add(_field.Name, _field.GetValue(_value)));

						List<PropertyInfo> properties = _value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
						properties.ForEach(_property => map.Add(_property.Name, _property.GetValue(_value, null)));

						SerializeObject(map, _indent);
					}
				}
			}
		}
	}
}