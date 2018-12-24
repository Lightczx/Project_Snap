using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json
{
	public abstract class JsonWriter : IDisposable
	{
		internal enum State
		{
			Start,
			Property,
			ObjectStart,
			Object,
			ArrayStart,
			Array,
			ConstructorStart,
			Constructor,
			Closed,
			Error
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CAutoCompleteAsync_003Ed__1 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public JsonWriter _003C_003E4__this;

			public CancellationToken cancellationToken;

			private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				JsonWriter jsonWriter = _003C_003E4__this;
				try
				{
					ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter;
					switch (num)
					{
					default:
						awaiter = jsonWriter.WriteValueDelimiterAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_007b;
					case 0:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_007b;
					case 1:
						{
							awaiter = _003C_003Eu__1;
							_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
							num = (_003C_003E1__state = -1);
							break;
						}
						IL_007b:
						awaiter.GetResult();
						awaiter = jsonWriter.WriteIndentAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						break;
					}
					awaiter.GetResult();
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CInternalWriteStartAsync_003Ed__20 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public JsonWriter _003C_003E4__this;

			public JsonToken token;

			public CancellationToken cancellationToken;

			public JsonContainerType container;

			private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				JsonWriter jsonWriter = _003C_003E4__this;
				try
				{
					ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter;
					if (num != 0)
					{
						jsonWriter.UpdateScopeWithFinishedValue();
						awaiter = jsonWriter.AutoCompleteAsync(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
					}
					else
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
					}
					awaiter.GetResult();
					jsonWriter.Push(container);
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CWriteTokenAsync_003Ed__30 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public JsonWriter _003C_003E4__this;

			public JsonReader reader;

			public bool writeDateConstructorAsDate;

			public CancellationToken cancellationToken;

			public bool writeComments;

			public bool writeChildren;

			private int _003CinitialDepth_003E5__2;

			private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter _003C_003Eu__1;

			private ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter _003C_003Eu__2;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				JsonWriter jsonWriter = _003C_003E4__this;
				try
				{
					ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter2;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter awaiter;
					bool flag;
					switch (num)
					{
					default:
						_003CinitialDepth_003E5__2 = jsonWriter.CalculateWriteTokenInitialDepth(reader);
						goto IL_0032;
					case 0:
						awaiter2 = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_00d4;
					case 1:
						awaiter2 = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0172;
					case 2:
						{
							awaiter = _003C_003Eu__2;
							_003C_003Eu__2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
							num = (_003C_003E1__state = -1);
							goto IL_021b;
						}
						IL_0032:
						if (writeDateConstructorAsDate && reader.TokenType == JsonToken.StartConstructor && string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
						{
							awaiter2 = jsonWriter.WriteConstructorDateAsync(reader, cancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
							if (!awaiter2.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = awaiter2;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
								return;
							}
							goto IL_00d4;
						}
						if (writeComments || reader.TokenType != JsonToken.Comment)
						{
							awaiter2 = jsonWriter.WriteTokenAsync(reader.TokenType, reader.Value, cancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
							if (!awaiter2.IsCompleted)
							{
								num = (_003C_003E1__state = 1);
								_003C_003Eu__1 = awaiter2;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
								return;
							}
							goto IL_0172;
						}
						goto IL_0179;
						IL_021b:
						flag = awaiter.GetResult();
						goto IL_0224;
						IL_0179:
						flag = ((_003CinitialDepth_003E5__2 - 1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0)) & writeChildren);
						if (flag)
						{
							awaiter = reader.ReadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 2);
								_003C_003Eu__2 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							goto IL_021b;
						}
						goto IL_0224;
						IL_0224:
						if (!flag)
						{
							break;
						}
						goto IL_0032;
						IL_0172:
						awaiter2.GetResult();
						goto IL_0179;
						IL_00d4:
						awaiter2.GetResult();
						goto IL_0179;
					}
					if (_003CinitialDepth_003E5__2 < jsonWriter.CalculateWriteTokenFinalDepth(reader))
					{
						throw JsonWriterException.Create(jsonWriter, "Unexpected end when reading token.", null);
					}
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CWriteTokenSyncReadingAsync_003Ed__31 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public JsonWriter _003C_003E4__this;

			public JsonReader reader;

			public CancellationToken cancellationToken;

			private int _003CinitialDepth_003E5__2;

			private ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				JsonWriter jsonWriter = _003C_003E4__this;
				try
				{
					if (num != 0)
					{
						_003CinitialDepth_003E5__2 = jsonWriter.CalculateWriteTokenInitialDepth(reader);
						goto IL_0026;
					}
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter awaiter = _003C_003Eu__1;
					_003C_003Eu__1 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_0111;
					IL_0119:
					bool flag;
					if (flag)
					{
						goto IL_0026;
					}
					if (_003CinitialDepth_003E5__2 < jsonWriter.CalculateWriteTokenFinalDepth(reader))
					{
						throw JsonWriterException.Create(jsonWriter, "Unexpected end when reading token.", null);
					}
					goto end_IL_000e;
					IL_0026:
					if (reader.TokenType == JsonToken.StartConstructor && string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
					{
						jsonWriter.WriteConstructorDate(reader);
					}
					else
					{
						jsonWriter.WriteToken(reader.TokenType, reader.Value);
					}
					flag = (_003CinitialDepth_003E5__2 - 1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0));
					if (flag)
					{
						awaiter = reader.ReadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0111;
					}
					goto IL_0119;
					IL_0111:
					flag = awaiter.GetResult();
					goto IL_0119;
					end_IL_000e:;
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CWriteConstructorDateAsync_003Ed__32 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public JsonReader reader;

			public CancellationToken cancellationToken;

			public JsonWriter _003C_003E4__this;

			private DateTime _003Cdate_003E5__2;

			private ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter _003C_003Eu__1;

			private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter _003C_003Eu__2;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				JsonWriter jsonWriter = _003C_003E4__this;
				try
				{
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter awaiter2;
					ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter;
					switch (num)
					{
					default:
						awaiter2 = reader.ReadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter2.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
							return;
						}
						goto IL_0088;
					case 0:
						awaiter2 = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0088;
					case 1:
						awaiter2 = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0151;
					case 2:
						{
							awaiter = _003C_003Eu__2;
							_003C_003Eu__2 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
							num = (_003C_003E1__state = -1);
							break;
						}
						IL_0151:
						if (!awaiter2.GetResult())
						{
							throw JsonWriterException.Create(jsonWriter, "Unexpected end when reading date constructor.", null);
						}
						if (reader.TokenType != JsonToken.EndConstructor)
						{
							throw JsonWriterException.Create(jsonWriter, "Unexpected token when reading date constructor. Expected EndConstructor, got " + reader.TokenType, null);
						}
						awaiter = jsonWriter.WriteValueAsync(_003Cdate_003E5__2, cancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 2);
							_003C_003Eu__2 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						break;
						IL_0088:
						if (!awaiter2.GetResult())
						{
							throw JsonWriterException.Create(jsonWriter, "Unexpected end when reading date constructor.", null);
						}
						if (reader.TokenType != JsonToken.Integer)
						{
							throw JsonWriterException.Create(jsonWriter, "Unexpected token when reading date constructor. Expected Integer, got " + reader.TokenType, null);
						}
						_003Cdate_003E5__2 = DateTimeUtils.ConvertJavaScriptTicksToDateTime((long)reader.Value);
						awaiter2 = reader.ReadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter2.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__1 = awaiter2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
							return;
						}
						goto IL_0151;
					}
					awaiter.GetResult();
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003C_003CInternalWriteEndAsync_003Eg__AwaitProperty_007C11_0_003Ed : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public Task task;

			public JsonWriter _003C_003E4__this;

			public CancellationToken CancellationToken;

			public JsonToken token;

			public int LevelsToComplete;

			private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				JsonWriter jsonWriter = _003C_003E4__this;
				try
				{
					ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter;
					switch (num)
					{
					default:
						awaiter = task.ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0081;
					case 0:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0081;
					case 1:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0106;
					case 2:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0176;
					case 3:
						{
							awaiter = _003C_003Eu__1;
							_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
							num = (_003C_003E1__state = -1);
							break;
						}
						IL_010d:
						awaiter = jsonWriter.WriteEndAsync(token, CancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 2);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0176;
						IL_0081:
						awaiter.GetResult();
						if (jsonWriter._formatting == Formatting.Indented && jsonWriter._currentState != State.ObjectStart && jsonWriter._currentState != State.ArrayStart)
						{
							awaiter = jsonWriter.WriteIndentAsync(CancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 1);
								_003C_003Eu__1 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							goto IL_0106;
						}
						goto IL_010d;
						IL_0176:
						awaiter.GetResult();
						jsonWriter.UpdateCurrentState();
						awaiter = jsonWriter._003CInternalWriteEndAsync_003Eg__AwaitRemaining_007C11_3(LevelsToComplete, CancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 3);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						break;
						IL_0106:
						awaiter.GetResult();
						goto IL_010d;
					}
					awaiter.GetResult();
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003C_003CInternalWriteEndAsync_003Eg__AwaitIndent_007C11_1_003Ed : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public Task task;

			public JsonWriter _003C_003E4__this;

			public JsonToken token;

			public CancellationToken CancellationToken;

			public int LevelsToComplete;

			private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				JsonWriter jsonWriter = _003C_003E4__this;
				try
				{
					ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter;
					switch (num)
					{
					default:
						awaiter = task.ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_007d;
					case 0:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_007d;
					case 1:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_00ed;
					case 2:
						{
							awaiter = _003C_003Eu__1;
							_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
							num = (_003C_003E1__state = -1);
							break;
						}
						IL_00ed:
						awaiter.GetResult();
						jsonWriter.UpdateCurrentState();
						awaiter = jsonWriter._003CInternalWriteEndAsync_003Eg__AwaitRemaining_007C11_3(LevelsToComplete, CancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 2);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						break;
						IL_007d:
						awaiter.GetResult();
						awaiter = jsonWriter.WriteEndAsync(token, CancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_00ed;
					}
					awaiter.GetResult();
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003C_003CInternalWriteEndAsync_003Eg__AwaitEnd_007C11_2_003Ed : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public Task task;

			public JsonWriter _003C_003E4__this;

			public int LevelsToComplete;

			public CancellationToken CancellationToken;

			private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				JsonWriter jsonWriter = _003C_003E4__this;
				try
				{
					ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter;
					switch (num)
					{
					default:
						awaiter = task.ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0075;
					case 0:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0075;
					case 1:
						{
							awaiter = _003C_003Eu__1;
							_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
							num = (_003C_003E1__state = -1);
							break;
						}
						IL_0075:
						awaiter.GetResult();
						jsonWriter.UpdateCurrentState();
						awaiter = jsonWriter._003CInternalWriteEndAsync_003Eg__AwaitRemaining_007C11_3(LevelsToComplete, CancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						break;
					}
					awaiter.GetResult();
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003C_003CInternalWriteEndAsync_003Eg__AwaitRemaining_007C11_3_003Ed : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public JsonWriter _003C_003E4__this;

			public CancellationToken CancellationToken;

			public int LevelsToComplete;

			private JsonToken _003Ctoken_003E5__2;

			private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				JsonWriter jsonWriter = _003C_003E4__this;
				try
				{
					ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter;
					switch (num)
					{
					case 0:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_00a3;
					case 1:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0128;
					case 2:
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0195;
					default:
						{
							if (LevelsToComplete-- <= 0)
							{
								break;
							}
							_003Ctoken_003E5__2 = jsonWriter.GetCloseTokenForType(jsonWriter.Pop());
							if (jsonWriter._currentState == State.Property)
							{
								awaiter = jsonWriter.WriteNullAsync(CancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
								if (!awaiter.IsCompleted)
								{
									num = (_003C_003E1__state = 0);
									_003C_003Eu__1 = awaiter;
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
									return;
								}
								goto IL_00a3;
							}
							goto IL_00aa;
						}
						IL_00a3:
						awaiter.GetResult();
						goto IL_00aa;
						IL_00aa:
						if (jsonWriter._formatting == Formatting.Indented && jsonWriter._currentState != State.ObjectStart && jsonWriter._currentState != State.ArrayStart)
						{
							awaiter = jsonWriter.WriteIndentAsync(CancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 1);
								_003C_003Eu__1 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							goto IL_0128;
						}
						goto IL_012f;
						IL_0128:
						awaiter.GetResult();
						goto IL_012f;
						IL_012f:
						awaiter = jsonWriter.WriteEndAsync(_003Ctoken_003E5__2, CancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 2);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
						goto IL_0195;
						IL_0195:
						awaiter.GetResult();
						jsonWriter.UpdateCurrentState();
						goto default;
					}
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		private static readonly State[][] StateArray;

		internal static readonly State[][] StateArrayTempate;

		private List<JsonPosition> _stack;

		private JsonPosition _currentPosition;

		private State _currentState;

		private Formatting _formatting;

		private DateFormatHandling _dateFormatHandling;

		private DateTimeZoneHandling _dateTimeZoneHandling;

		private StringEscapeHandling _stringEscapeHandling;

		private FloatFormatHandling _floatFormatHandling;

		private string _dateFormatString;

		private CultureInfo _culture;

		public bool CloseOutput
		{
			get;
			set;
		}

		public bool AutoCompleteOnClose
		{
			get;
			set;
		}

		protected internal int Top
		{
			get
			{
				List<JsonPosition> stack = _stack;
				int num = (stack != null) ? stack.Count : 0;
				if (Peek() != 0)
				{
					num++;
				}
				return num;
			}
		}

		public WriteState WriteState
		{
			get
			{
				switch (_currentState)
				{
				case State.Error:
					return WriteState.Error;
				case State.Closed:
					return WriteState.Closed;
				case State.ObjectStart:
				case State.Object:
					return WriteState.Object;
				case State.ArrayStart:
				case State.Array:
					return WriteState.Array;
				case State.ConstructorStart:
				case State.Constructor:
					return WriteState.Constructor;
				case State.Property:
					return WriteState.Property;
				case State.Start:
					return WriteState.Start;
				default:
					throw JsonWriterException.Create(this, "Invalid state: " + _currentState, null);
				}
			}
		}

		internal string ContainerPath
		{
			get
			{
				if (_currentPosition.Type == JsonContainerType.None || _stack == null)
				{
					return string.Empty;
				}
				return JsonPosition.BuildPath(_stack, null);
			}
		}

		public string Path
		{
			get
			{
				if (_currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				JsonPosition? currentPosition = (_currentState != State.ArrayStart && _currentState != State.ConstructorStart && _currentState != State.ObjectStart) ? new JsonPosition?(_currentPosition) : null;
				return JsonPosition.BuildPath(_stack, currentPosition);
			}
		}

		public Formatting Formatting
		{
			get
			{
				return _formatting;
			}
			set
			{
				if (value < Formatting.None || value > Formatting.Indented)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_formatting = value;
			}
		}

		public DateFormatHandling DateFormatHandling
		{
			get
			{
				return _dateFormatHandling;
			}
			set
			{
				if (value < DateFormatHandling.IsoDateFormat || value > DateFormatHandling.MicrosoftDateFormat)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_dateFormatHandling = value;
			}
		}

		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return _dateTimeZoneHandling;
			}
			set
			{
				if (value < DateTimeZoneHandling.Local || value > DateTimeZoneHandling.RoundtripKind)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_dateTimeZoneHandling = value;
			}
		}

		public StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return _stringEscapeHandling;
			}
			set
			{
				if (value < StringEscapeHandling.Default || value > StringEscapeHandling.EscapeHtml)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_stringEscapeHandling = value;
				OnStringEscapeHandlingChanged();
			}
		}

		public FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return _floatFormatHandling;
			}
			set
			{
				if (value < FloatFormatHandling.String || value > FloatFormatHandling.DefaultValue)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_floatFormatHandling = value;
			}
		}

		public string DateFormatString
		{
			get
			{
				return _dateFormatString;
			}
			set
			{
				_dateFormatString = value;
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return _culture ?? CultureInfo.InvariantCulture;
			}
			set
			{
				_culture = value;
			}
		}

		internal Task AutoCompleteAsync(JsonToken tokenBeingWritten, CancellationToken cancellationToken)
		{
			State currentState = _currentState;
			State state = StateArray[(int)tokenBeingWritten][(int)currentState];
			if (state == State.Error)
			{
				throw JsonWriterException.Create(this, "Token {0} in state {1} would result in an invalid JSON object.".FormatWith(CultureInfo.InvariantCulture, tokenBeingWritten.ToString(), currentState.ToString()), null);
			}
			_currentState = state;
			if (_formatting == Formatting.Indented)
			{
				switch (currentState)
				{
				case State.Property:
					return WriteIndentSpaceAsync(cancellationToken);
				case State.ArrayStart:
				case State.ConstructorStart:
					return WriteIndentAsync(cancellationToken);
				case State.Array:
				case State.Constructor:
					if (tokenBeingWritten != JsonToken.Comment)
					{
						return AutoCompleteAsync(cancellationToken);
					}
					return WriteIndentAsync(cancellationToken);
				case State.Object:
					switch (tokenBeingWritten)
					{
					case JsonToken.PropertyName:
						return AutoCompleteAsync(cancellationToken);
					default:
						return WriteValueDelimiterAsync(cancellationToken);
					case JsonToken.Comment:
						break;
					}
					break;
				default:
					if (tokenBeingWritten == JsonToken.PropertyName)
					{
						return WriteIndentAsync(cancellationToken);
					}
					break;
				case State.Start:
					break;
				}
			}
			else if (tokenBeingWritten != JsonToken.Comment)
			{
				switch (currentState)
				{
				case State.Object:
				case State.Array:
				case State.Constructor:
					return WriteValueDelimiterAsync(cancellationToken);
				}
			}
			return AsyncUtils.CompletedTask;
		}

		private async Task AutoCompleteAsync(CancellationToken cancellationToken)
		{
			await WriteValueDelimiterAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			await WriteIndentAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public virtual Task CloseAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			Close();
			return AsyncUtils.CompletedTask;
		}

		public virtual Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			Flush();
			return AsyncUtils.CompletedTask;
		}

		protected virtual Task WriteEndAsync(JsonToken token, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteEnd(token);
			return AsyncUtils.CompletedTask;
		}

		protected virtual Task WriteIndentAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteIndent();
			return AsyncUtils.CompletedTask;
		}

		protected virtual Task WriteValueDelimiterAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValueDelimiter();
			return AsyncUtils.CompletedTask;
		}

		protected virtual Task WriteIndentSpaceAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteIndentSpace();
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteRawAsync(string json, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteRaw(json);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteEndAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteEnd();
			return AsyncUtils.CompletedTask;
		}

		internal Task WriteEndInternalAsync(CancellationToken cancellationToken)
		{
			JsonContainerType jsonContainerType = Peek();
			switch (jsonContainerType)
			{
			case JsonContainerType.Object:
				return WriteEndObjectAsync(cancellationToken);
			case JsonContainerType.Array:
				return WriteEndArrayAsync(cancellationToken);
			case JsonContainerType.Constructor:
				return WriteEndConstructorAsync(cancellationToken);
			default:
				if (cancellationToken.IsCancellationRequested)
				{
					return cancellationToken.FromCanceled();
				}
				throw JsonWriterException.Create(this, "Unexpected type when writing end: " + jsonContainerType, null);
			}
		}

		internal Task InternalWriteEndAsync(JsonContainerType type, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			int num = CalculateLevelsToComplete(type);
			while (num-- > 0)
			{
				JsonToken closeTokenForType = GetCloseTokenForType(Pop());
				Task task;
				if (_currentState == State.Property)
				{
					task = WriteNullAsync(cancellationToken);
					if (!task.IsCompletedSucessfully())
					{
						return _003CInternalWriteEndAsync_003Eg__AwaitProperty_007C11_0(task, num, closeTokenForType, cancellationToken);
					}
				}
				if (_formatting == Formatting.Indented && _currentState != State.ObjectStart && _currentState != State.ArrayStart)
				{
					task = WriteIndentAsync(cancellationToken);
					if (!task.IsCompletedSucessfully())
					{
						return _003CInternalWriteEndAsync_003Eg__AwaitIndent_007C11_1(task, num, closeTokenForType, cancellationToken);
					}
				}
				task = WriteEndAsync(closeTokenForType, cancellationToken);
				if (!task.IsCompletedSucessfully())
				{
					return _003CInternalWriteEndAsync_003Eg__AwaitEnd_007C11_2(task, num, cancellationToken);
				}
				UpdateCurrentState();
			}
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteEndArrayAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteEndArray();
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteEndConstructorAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteEndConstructor();
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteEndObjectAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteEndObject();
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteNullAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteNull();
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WritePropertyNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WritePropertyName(name);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WritePropertyNameAsync(string name, bool escape, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WritePropertyName(name, escape);
			return AsyncUtils.CompletedTask;
		}

		internal Task InternalWritePropertyNameAsync(string name, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			_currentPosition.PropertyName = name;
			return AutoCompleteAsync(JsonToken.PropertyName, cancellationToken);
		}

		public virtual Task WriteStartArrayAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteStartArray();
			return AsyncUtils.CompletedTask;
		}

		internal async Task InternalWriteStartAsync(JsonToken token, JsonContainerType container, CancellationToken cancellationToken)
		{
			UpdateScopeWithFinishedValue();
			await AutoCompleteAsync(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Push(container);
		}

		public virtual Task WriteCommentAsync(string text, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteComment(text);
			return AsyncUtils.CompletedTask;
		}

		internal Task InternalWriteCommentAsync(CancellationToken cancellationToken)
		{
			return AutoCompleteAsync(JsonToken.Comment, cancellationToken);
		}

		public virtual Task WriteRawValueAsync(string json, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteRawValue(json);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteStartConstructorAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteStartConstructor(name);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteStartObjectAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteStartObject();
			return AsyncUtils.CompletedTask;
		}

		public Task WriteTokenAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return WriteTokenAsync(reader, writeChildren: true, cancellationToken);
		}

		public Task WriteTokenAsync(JsonReader reader, bool writeChildren, CancellationToken cancellationToken = default(CancellationToken))
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			return WriteTokenAsync(reader, writeChildren, writeDateConstructorAsDate: true, writeComments: true, cancellationToken);
		}

		public Task WriteTokenAsync(JsonToken token, CancellationToken cancellationToken = default(CancellationToken))
		{
			return WriteTokenAsync(token, null, cancellationToken);
		}

		public Task WriteTokenAsync(JsonToken token, object value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				switch (token)
				{
				case JsonToken.None:
					return AsyncUtils.CompletedTask;
				case JsonToken.StartObject:
					return WriteStartObjectAsync(cancellationToken);
				case JsonToken.StartArray:
					return WriteStartArrayAsync(cancellationToken);
				case JsonToken.StartConstructor:
					ValidationUtils.ArgumentNotNull(value, "value");
					return WriteStartConstructorAsync(value.ToString(), cancellationToken);
				case JsonToken.PropertyName:
					ValidationUtils.ArgumentNotNull(value, "value");
					return WritePropertyNameAsync(value.ToString(), cancellationToken);
				case JsonToken.Comment:
					return WriteCommentAsync(value?.ToString(), cancellationToken);
				case JsonToken.Integer:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					object obj;
					if (!((obj = value) is BigInteger))
					{
						return WriteValueAsync(Convert.ToInt64(value, CultureInfo.InvariantCulture), cancellationToken);
					}
					BigInteger bigInteger = (BigInteger)obj;
					return WriteValueAsync(bigInteger, cancellationToken);
				}
				case JsonToken.Float:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					object obj;
					if ((obj = value) is decimal)
					{
						decimal value4 = (decimal)obj;
						return WriteValueAsync(value4, cancellationToken);
					}
					if ((obj = value) is double)
					{
						double value5 = (double)obj;
						return WriteValueAsync(value5, cancellationToken);
					}
					if ((obj = value) is float)
					{
						float value6 = (float)obj;
						return WriteValueAsync(value6, cancellationToken);
					}
					return WriteValueAsync(Convert.ToDouble(value, CultureInfo.InvariantCulture), cancellationToken);
				}
				case JsonToken.String:
					ValidationUtils.ArgumentNotNull(value, "value");
					return WriteValueAsync(value.ToString(), cancellationToken);
				case JsonToken.Boolean:
					ValidationUtils.ArgumentNotNull(value, "value");
					return WriteValueAsync(Convert.ToBoolean(value, CultureInfo.InvariantCulture), cancellationToken);
				case JsonToken.Null:
					return WriteNullAsync(cancellationToken);
				case JsonToken.Undefined:
					return WriteUndefinedAsync(cancellationToken);
				case JsonToken.EndObject:
					return WriteEndObjectAsync(cancellationToken);
				case JsonToken.EndArray:
					return WriteEndArrayAsync(cancellationToken);
				case JsonToken.EndConstructor:
					return WriteEndConstructorAsync(cancellationToken);
				case JsonToken.Date:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					object obj;
					if ((obj = value) is DateTimeOffset)
					{
						DateTimeOffset value3 = (DateTimeOffset)obj;
						return WriteValueAsync(value3, cancellationToken);
					}
					return WriteValueAsync(Convert.ToDateTime(value, CultureInfo.InvariantCulture), cancellationToken);
				}
				case JsonToken.Raw:
					return WriteRawValueAsync(value?.ToString(), cancellationToken);
				case JsonToken.Bytes:
				{
					ValidationUtils.ArgumentNotNull(value, "value");
					object obj;
					if ((obj = value) is Guid)
					{
						Guid value2 = (Guid)obj;
						return WriteValueAsync(value2, cancellationToken);
					}
					return WriteValueAsync((byte[])value, cancellationToken);
				}
				default:
					throw MiscellaneousUtils.CreateArgumentOutOfRangeException("token", token, "Unexpected token type.");
				}
			}
			return cancellationToken.FromCanceled();
		}

		internal virtual async Task WriteTokenAsync(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments, CancellationToken cancellationToken)
		{
			int initialDepth = CalculateWriteTokenInitialDepth(reader);
			bool flag;
			do
			{
				if (writeDateConstructorAsDate && reader.TokenType == JsonToken.StartConstructor && string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
				{
					await WriteConstructorDateAsync(reader, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				else if (writeComments || reader.TokenType != JsonToken.Comment)
				{
					await WriteTokenAsync(reader.TokenType, reader.Value, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				flag = (initialDepth - 1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0) && writeChildren);
				if (flag)
				{
					flag = await reader.ReadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			while (flag);
			if (initialDepth < CalculateWriteTokenFinalDepth(reader))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
			}
		}

		internal async Task WriteTokenSyncReadingAsync(JsonReader reader, CancellationToken cancellationToken)
		{
			int initialDepth = CalculateWriteTokenInitialDepth(reader);
			bool flag;
			do
			{
				if (reader.TokenType == JsonToken.StartConstructor && string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
				{
					WriteConstructorDate(reader);
				}
				else
				{
					WriteToken(reader.TokenType, reader.Value);
				}
				flag = (initialDepth - 1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0));
				if (flag)
				{
					flag = await reader.ReadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			while (flag);
			if (initialDepth < CalculateWriteTokenFinalDepth(reader))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
			}
		}

		private async Task WriteConstructorDateAsync(JsonReader reader, CancellationToken cancellationToken)
		{
			if (!(await reader.ReadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false)))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", null);
			}
			if (reader.TokenType != JsonToken.Integer)
			{
				throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected Integer, got " + reader.TokenType, null);
			}
			DateTime date = DateTimeUtils.ConvertJavaScriptTicksToDateTime((long)reader.Value);
			if (!(await reader.ReadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false)))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", null);
			}
			if (reader.TokenType != JsonToken.EndConstructor)
			{
				throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected EndConstructor, got " + reader.TokenType, null);
			}
			await WriteValueAsync(date, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public virtual Task WriteValueAsync(bool value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(bool? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(byte value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(byte? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(byte[] value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(char value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(char? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(DateTime value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(DateTime? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(DateTimeOffset value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(DateTimeOffset? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(decimal value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(decimal? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(double value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(double? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(float value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(float? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(Guid value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(Guid? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(int value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(int? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(long value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(long? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(object value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(sbyte value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(sbyte? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(short value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(short? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(string value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(TimeSpan value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(TimeSpan? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(uint value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(uint? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(ulong value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(ulong? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteValueAsync(Uri value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(ushort value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(ushort? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteUndefinedAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteUndefined();
			return AsyncUtils.CompletedTask;
		}

		public virtual Task WriteWhitespaceAsync(string ws, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			WriteWhitespace(ws);
			return AsyncUtils.CompletedTask;
		}

		internal Task InternalWriteValueAsync(JsonToken token, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			UpdateScopeWithFinishedValue();
			return AutoCompleteAsync(token, cancellationToken);
		}

		protected Task SetWriteStateAsync(JsonToken token, object value, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				switch (token)
				{
				case JsonToken.StartObject:
					return InternalWriteStartAsync(token, JsonContainerType.Object, cancellationToken);
				case JsonToken.StartArray:
					return InternalWriteStartAsync(token, JsonContainerType.Array, cancellationToken);
				case JsonToken.StartConstructor:
					return InternalWriteStartAsync(token, JsonContainerType.Constructor, cancellationToken);
				case JsonToken.PropertyName:
				{
					string name;
					if ((name = (value as string)) == null)
					{
						throw new ArgumentException("A name is required when setting property name state.", "value");
					}
					return InternalWritePropertyNameAsync(name, cancellationToken);
				}
				case JsonToken.Comment:
					return InternalWriteCommentAsync(cancellationToken);
				case JsonToken.Raw:
					return AsyncUtils.CompletedTask;
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Null:
				case JsonToken.Undefined:
				case JsonToken.Date:
				case JsonToken.Bytes:
					return InternalWriteValueAsync(token, cancellationToken);
				case JsonToken.EndObject:
					return InternalWriteEndAsync(JsonContainerType.Object, cancellationToken);
				case JsonToken.EndArray:
					return InternalWriteEndAsync(JsonContainerType.Array, cancellationToken);
				case JsonToken.EndConstructor:
					return InternalWriteEndAsync(JsonContainerType.Constructor, cancellationToken);
				default:
					throw new ArgumentOutOfRangeException("token");
				}
			}
			return cancellationToken.FromCanceled();
		}

		internal static Task WriteValueAsync(JsonWriter writer, PrimitiveTypeCode typeCode, object value, CancellationToken cancellationToken)
		{
			while (true)
			{
				switch (typeCode)
				{
				case PrimitiveTypeCode.Char:
					return writer.WriteValueAsync((char)value, cancellationToken);
				case PrimitiveTypeCode.CharNullable:
					return writer.WriteValueAsync((value == null) ? null : new char?((char)value), cancellationToken);
				case PrimitiveTypeCode.Boolean:
					return writer.WriteValueAsync((bool)value, cancellationToken);
				case PrimitiveTypeCode.BooleanNullable:
					return writer.WriteValueAsync((value == null) ? null : new bool?((bool)value), cancellationToken);
				case PrimitiveTypeCode.SByte:
					return writer.WriteValueAsync((sbyte)value, cancellationToken);
				case PrimitiveTypeCode.SByteNullable:
					return writer.WriteValueAsync((value == null) ? null : new sbyte?((sbyte)value), cancellationToken);
				case PrimitiveTypeCode.Int16:
					return writer.WriteValueAsync((short)value, cancellationToken);
				case PrimitiveTypeCode.Int16Nullable:
					return writer.WriteValueAsync((value == null) ? null : new short?((short)value), cancellationToken);
				case PrimitiveTypeCode.UInt16:
					return writer.WriteValueAsync((ushort)value, cancellationToken);
				case PrimitiveTypeCode.UInt16Nullable:
					return writer.WriteValueAsync((value == null) ? null : new ushort?((ushort)value), cancellationToken);
				case PrimitiveTypeCode.Int32:
					return writer.WriteValueAsync((int)value, cancellationToken);
				case PrimitiveTypeCode.Int32Nullable:
					return writer.WriteValueAsync((value == null) ? null : new int?((int)value), cancellationToken);
				case PrimitiveTypeCode.Byte:
					return writer.WriteValueAsync((byte)value, cancellationToken);
				case PrimitiveTypeCode.ByteNullable:
					return writer.WriteValueAsync((value == null) ? null : new byte?((byte)value), cancellationToken);
				case PrimitiveTypeCode.UInt32:
					return writer.WriteValueAsync((uint)value, cancellationToken);
				case PrimitiveTypeCode.UInt32Nullable:
					return writer.WriteValueAsync((value == null) ? null : new uint?((uint)value), cancellationToken);
				case PrimitiveTypeCode.Int64:
					return writer.WriteValueAsync((long)value, cancellationToken);
				case PrimitiveTypeCode.Int64Nullable:
					return writer.WriteValueAsync((value == null) ? null : new long?((long)value), cancellationToken);
				case PrimitiveTypeCode.UInt64:
					return writer.WriteValueAsync((ulong)value, cancellationToken);
				case PrimitiveTypeCode.UInt64Nullable:
					return writer.WriteValueAsync((value == null) ? null : new ulong?((ulong)value), cancellationToken);
				case PrimitiveTypeCode.Single:
					return writer.WriteValueAsync((float)value, cancellationToken);
				case PrimitiveTypeCode.SingleNullable:
					return writer.WriteValueAsync((value == null) ? null : new float?((float)value), cancellationToken);
				case PrimitiveTypeCode.Double:
					return writer.WriteValueAsync((double)value, cancellationToken);
				case PrimitiveTypeCode.DoubleNullable:
					return writer.WriteValueAsync((value == null) ? null : new double?((double)value), cancellationToken);
				case PrimitiveTypeCode.DateTime:
					return writer.WriteValueAsync((DateTime)value, cancellationToken);
				case PrimitiveTypeCode.DateTimeNullable:
					return writer.WriteValueAsync((value == null) ? null : new DateTime?((DateTime)value), cancellationToken);
				case PrimitiveTypeCode.DateTimeOffset:
					return writer.WriteValueAsync((DateTimeOffset)value, cancellationToken);
				case PrimitiveTypeCode.DateTimeOffsetNullable:
					return writer.WriteValueAsync((value == null) ? null : new DateTimeOffset?((DateTimeOffset)value), cancellationToken);
				case PrimitiveTypeCode.Decimal:
					return writer.WriteValueAsync((decimal)value, cancellationToken);
				case PrimitiveTypeCode.DecimalNullable:
					return writer.WriteValueAsync((value == null) ? null : new decimal?((decimal)value), cancellationToken);
				case PrimitiveTypeCode.Guid:
					return writer.WriteValueAsync((Guid)value, cancellationToken);
				case PrimitiveTypeCode.GuidNullable:
					return writer.WriteValueAsync((value == null) ? null : new Guid?((Guid)value), cancellationToken);
				case PrimitiveTypeCode.TimeSpan:
					return writer.WriteValueAsync((TimeSpan)value, cancellationToken);
				case PrimitiveTypeCode.TimeSpanNullable:
					return writer.WriteValueAsync((value == null) ? null : new TimeSpan?((TimeSpan)value), cancellationToken);
				case PrimitiveTypeCode.BigInteger:
					return writer.WriteValueAsync((BigInteger)value, cancellationToken);
				case PrimitiveTypeCode.BigIntegerNullable:
					return writer.WriteValueAsync((value == null) ? null : new BigInteger?((BigInteger)value), cancellationToken);
				case PrimitiveTypeCode.Uri:
					return writer.WriteValueAsync((Uri)value, cancellationToken);
				case PrimitiveTypeCode.String:
					return writer.WriteValueAsync((string)value, cancellationToken);
				case PrimitiveTypeCode.Bytes:
					return writer.WriteValueAsync((byte[])value, cancellationToken);
				case PrimitiveTypeCode.DBNull:
					return writer.WriteNullAsync(cancellationToken);
				}
				IConvertible convertible;
				if ((convertible = (value as IConvertible)) == null)
				{
					break;
				}
				ResolveConvertibleValue(convertible, out typeCode, out value);
			}
			if (value == null)
			{
				return writer.WriteNullAsync(cancellationToken);
			}
			throw CreateUnsupportedTypeException(writer, value);
		}

		internal static State[][] BuildStateArray()
		{
			List<State[]> list = StateArrayTempate.ToList();
			State[] item = StateArrayTempate[0];
			State[] item2 = StateArrayTempate[7];
			ulong[] values = EnumUtils.GetEnumValuesAndNames(typeof(JsonToken)).Values;
			foreach (ulong num in values)
			{
				if (list.Count <= (int)num)
				{
					JsonToken jsonToken = (JsonToken)num;
					if ((uint)(jsonToken - 7) <= 5u || (uint)(jsonToken - 16) <= 1u)
					{
						list.Add(item2);
					}
					else
					{
						list.Add(item);
					}
				}
			}
			return list.ToArray();
		}

		static JsonWriter()
		{
			StateArrayTempate = new State[8][]
			{
				new State[10]
				{
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.ObjectStart,
					State.ObjectStart,
					State.Error,
					State.Error,
					State.ObjectStart,
					State.ObjectStart,
					State.ObjectStart,
					State.ObjectStart,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.ArrayStart,
					State.ArrayStart,
					State.Error,
					State.Error,
					State.ArrayStart,
					State.ArrayStart,
					State.ArrayStart,
					State.ArrayStart,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.ConstructorStart,
					State.ConstructorStart,
					State.Error,
					State.Error,
					State.ConstructorStart,
					State.ConstructorStart,
					State.ConstructorStart,
					State.ConstructorStart,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.Property,
					State.Error,
					State.Property,
					State.Property,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.Start,
					State.Property,
					State.ObjectStart,
					State.Object,
					State.ArrayStart,
					State.Array,
					State.Constructor,
					State.Constructor,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.Start,
					State.Property,
					State.ObjectStart,
					State.Object,
					State.ArrayStart,
					State.Array,
					State.Constructor,
					State.Constructor,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.Start,
					State.Object,
					State.Error,
					State.Error,
					State.Array,
					State.Array,
					State.Constructor,
					State.Constructor,
					State.Error,
					State.Error
				}
			};
			StateArray = BuildStateArray();
		}

		internal virtual void OnStringEscapeHandlingChanged()
		{
		}

		protected JsonWriter()
		{
			_currentState = State.Start;
			_formatting = Formatting.None;
			_dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
			CloseOutput = true;
			AutoCompleteOnClose = true;
		}

		internal void UpdateScopeWithFinishedValue()
		{
			if (_currentPosition.HasIndex)
			{
				_currentPosition.Position++;
			}
		}

		private void Push(JsonContainerType value)
		{
			if (_currentPosition.Type != 0)
			{
				if (_stack == null)
				{
					_stack = new List<JsonPosition>();
				}
				_stack.Add(_currentPosition);
			}
			_currentPosition = new JsonPosition(value);
		}

		private JsonContainerType Pop()
		{
			JsonPosition currentPosition = _currentPosition;
			if (_stack != null && _stack.Count > 0)
			{
				_currentPosition = _stack[_stack.Count - 1];
				_stack.RemoveAt(_stack.Count - 1);
			}
			else
			{
				_currentPosition = default(JsonPosition);
			}
			return currentPosition.Type;
		}

		private JsonContainerType Peek()
		{
			return _currentPosition.Type;
		}

		public abstract void Flush();

		public virtual void Close()
		{
			if (AutoCompleteOnClose)
			{
				AutoCompleteAll();
			}
		}

		public virtual void WriteStartObject()
		{
			InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
		}

		public virtual void WriteEndObject()
		{
			InternalWriteEnd(JsonContainerType.Object);
		}

		public virtual void WriteStartArray()
		{
			InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
		}

		public virtual void WriteEndArray()
		{
			InternalWriteEnd(JsonContainerType.Array);
		}

		public virtual void WriteStartConstructor(string name)
		{
			InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
		}

		public virtual void WriteEndConstructor()
		{
			InternalWriteEnd(JsonContainerType.Constructor);
		}

		public virtual void WritePropertyName(string name)
		{
			InternalWritePropertyName(name);
		}

		public virtual void WritePropertyName(string name, bool escape)
		{
			WritePropertyName(name);
		}

		public virtual void WriteEnd()
		{
			WriteEnd(Peek());
		}

		public void WriteToken(JsonReader reader)
		{
			WriteToken(reader, writeChildren: true);
		}

		public void WriteToken(JsonReader reader, bool writeChildren)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			WriteToken(reader, writeChildren, writeDateConstructorAsDate: true, writeComments: true);
		}

		public void WriteToken(JsonToken token, object value)
		{
			switch (token)
			{
			case JsonToken.None:
				break;
			case JsonToken.StartObject:
				WriteStartObject();
				break;
			case JsonToken.StartArray:
				WriteStartArray();
				break;
			case JsonToken.StartConstructor:
				ValidationUtils.ArgumentNotNull(value, "value");
				WriteStartConstructor(value.ToString());
				break;
			case JsonToken.PropertyName:
				ValidationUtils.ArgumentNotNull(value, "value");
				WritePropertyName(value.ToString());
				break;
			case JsonToken.Comment:
				WriteComment(value?.ToString());
				break;
			case JsonToken.Integer:
			{
				ValidationUtils.ArgumentNotNull(value, "value");
				object obj;
				if ((obj = value) is BigInteger)
				{
					BigInteger bigInteger = (BigInteger)obj;
					WriteValue(bigInteger);
				}
				else
				{
					WriteValue(Convert.ToInt64(value, CultureInfo.InvariantCulture));
				}
				break;
			}
			case JsonToken.Float:
			{
				ValidationUtils.ArgumentNotNull(value, "value");
				object obj;
				if ((obj = value) is decimal)
				{
					decimal value3 = (decimal)obj;
					WriteValue(value3);
				}
				else if ((obj = value) is double)
				{
					double value4 = (double)obj;
					WriteValue(value4);
				}
				else if ((obj = value) is float)
				{
					float value5 = (float)obj;
					WriteValue(value5);
				}
				else
				{
					WriteValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
				}
				break;
			}
			case JsonToken.String:
				ValidationUtils.ArgumentNotNull(value, "value");
				WriteValue(value.ToString());
				break;
			case JsonToken.Boolean:
				ValidationUtils.ArgumentNotNull(value, "value");
				WriteValue(Convert.ToBoolean(value, CultureInfo.InvariantCulture));
				break;
			case JsonToken.Null:
				WriteNull();
				break;
			case JsonToken.Undefined:
				WriteUndefined();
				break;
			case JsonToken.EndObject:
				WriteEndObject();
				break;
			case JsonToken.EndArray:
				WriteEndArray();
				break;
			case JsonToken.EndConstructor:
				WriteEndConstructor();
				break;
			case JsonToken.Date:
			{
				ValidationUtils.ArgumentNotNull(value, "value");
				object obj;
				if ((obj = value) is DateTimeOffset)
				{
					DateTimeOffset value6 = (DateTimeOffset)obj;
					WriteValue(value6);
				}
				else
				{
					WriteValue(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
				}
				break;
			}
			case JsonToken.Raw:
				WriteRawValue(value?.ToString());
				break;
			case JsonToken.Bytes:
			{
				ValidationUtils.ArgumentNotNull(value, "value");
				object obj;
				if ((obj = value) is Guid)
				{
					Guid value2 = (Guid)obj;
					WriteValue(value2);
				}
				else
				{
					WriteValue((byte[])value);
				}
				break;
			}
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("token", token, "Unexpected token type.");
			}
		}

		public void WriteToken(JsonToken token)
		{
			WriteToken(token, null);
		}

		internal virtual void WriteToken(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments)
		{
			int num = CalculateWriteTokenInitialDepth(reader);
			do
			{
				if (writeDateConstructorAsDate && reader.TokenType == JsonToken.StartConstructor && string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
				{
					WriteConstructorDate(reader);
				}
				else if (writeComments || reader.TokenType != JsonToken.Comment)
				{
					WriteToken(reader.TokenType, reader.Value);
				}
			}
			while (num - 1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0) && writeChildren && reader.Read());
			if (num < CalculateWriteTokenFinalDepth(reader))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
			}
		}

		private int CalculateWriteTokenInitialDepth(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.None)
			{
				return -1;
			}
			if (!JsonTokenUtils.IsStartToken(tokenType))
			{
				return reader.Depth + 1;
			}
			return reader.Depth;
		}

		private int CalculateWriteTokenFinalDepth(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.None)
			{
				return -1;
			}
			if (!JsonTokenUtils.IsEndToken(tokenType))
			{
				return reader.Depth;
			}
			return reader.Depth - 1;
		}

		private void WriteConstructorDate(JsonReader reader)
		{
			if (!JavaScriptUtils.TryGetDateFromConstructorJson(reader, out DateTime dateTime, out string errorMessage))
			{
				throw JsonWriterException.Create(this, errorMessage, null);
			}
			WriteValue(dateTime);
		}

		private void WriteEnd(JsonContainerType type)
		{
			switch (type)
			{
			case JsonContainerType.Object:
				WriteEndObject();
				break;
			case JsonContainerType.Array:
				WriteEndArray();
				break;
			case JsonContainerType.Constructor:
				WriteEndConstructor();
				break;
			default:
				throw JsonWriterException.Create(this, "Unexpected type when writing end: " + type, null);
			}
		}

		private void AutoCompleteAll()
		{
			while (Top > 0)
			{
				WriteEnd();
			}
		}

		private JsonToken GetCloseTokenForType(JsonContainerType type)
		{
			switch (type)
			{
			case JsonContainerType.Object:
				return JsonToken.EndObject;
			case JsonContainerType.Array:
				return JsonToken.EndArray;
			case JsonContainerType.Constructor:
				return JsonToken.EndConstructor;
			default:
				throw JsonWriterException.Create(this, "No close token for type: " + type, null);
			}
		}

		private void AutoCompleteClose(JsonContainerType type)
		{
			int num = CalculateLevelsToComplete(type);
			for (int i = 0; i < num; i++)
			{
				JsonToken closeTokenForType = GetCloseTokenForType(Pop());
				if (_currentState == State.Property)
				{
					WriteNull();
				}
				if (_formatting == Formatting.Indented && _currentState != State.ObjectStart && _currentState != State.ArrayStart)
				{
					WriteIndent();
				}
				WriteEnd(closeTokenForType);
				UpdateCurrentState();
			}
		}

		private int CalculateLevelsToComplete(JsonContainerType type)
		{
			int num = 0;
			if (_currentPosition.Type == type)
			{
				num = 1;
			}
			else
			{
				int num2 = Top - 2;
				for (int num3 = num2; num3 >= 0; num3--)
				{
					int index = num2 - num3;
					if (_stack[index].Type == type)
					{
						num = num3 + 2;
						break;
					}
				}
			}
			if (num == 0)
			{
				throw JsonWriterException.Create(this, "No token to close.", null);
			}
			return num;
		}

		private void UpdateCurrentState()
		{
			JsonContainerType jsonContainerType = Peek();
			switch (jsonContainerType)
			{
			case JsonContainerType.Object:
				_currentState = State.Object;
				break;
			case JsonContainerType.Array:
				_currentState = State.Array;
				break;
			case JsonContainerType.Constructor:
				_currentState = State.Array;
				break;
			case JsonContainerType.None:
				_currentState = State.Start;
				break;
			default:
				throw JsonWriterException.Create(this, "Unknown JsonType: " + jsonContainerType, null);
			}
		}

		protected virtual void WriteEnd(JsonToken token)
		{
		}

		protected virtual void WriteIndent()
		{
		}

		protected virtual void WriteValueDelimiter()
		{
		}

		protected virtual void WriteIndentSpace()
		{
		}

		internal void AutoComplete(JsonToken tokenBeingWritten)
		{
			State state = StateArray[(int)tokenBeingWritten][(int)_currentState];
			if (state == State.Error)
			{
				throw JsonWriterException.Create(this, "Token {0} in state {1} would result in an invalid JSON object.".FormatWith(CultureInfo.InvariantCulture, tokenBeingWritten.ToString(), _currentState.ToString()), null);
			}
			if ((_currentState == State.Object || _currentState == State.Array || _currentState == State.Constructor) && tokenBeingWritten != JsonToken.Comment)
			{
				WriteValueDelimiter();
			}
			if (_formatting == Formatting.Indented)
			{
				if (_currentState == State.Property)
				{
					WriteIndentSpace();
				}
				if (_currentState == State.Array || _currentState == State.ArrayStart || _currentState == State.Constructor || _currentState == State.ConstructorStart || (tokenBeingWritten == JsonToken.PropertyName && _currentState != 0))
				{
					WriteIndent();
				}
			}
			_currentState = state;
		}

		public virtual void WriteNull()
		{
			InternalWriteValue(JsonToken.Null);
		}

		public virtual void WriteUndefined()
		{
			InternalWriteValue(JsonToken.Undefined);
		}

		public virtual void WriteRaw(string json)
		{
			InternalWriteRaw();
		}

		public virtual void WriteRawValue(string json)
		{
			UpdateScopeWithFinishedValue();
			AutoComplete(JsonToken.Undefined);
			WriteRaw(json);
		}

		public virtual void WriteValue(string value)
		{
			InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(int value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(uint value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		public virtual void WriteValue(long value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(ulong value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		public virtual void WriteValue(float value)
		{
			InternalWriteValue(JsonToken.Float);
		}

		public virtual void WriteValue(double value)
		{
			InternalWriteValue(JsonToken.Float);
		}

		public virtual void WriteValue(bool value)
		{
			InternalWriteValue(JsonToken.Boolean);
		}

		public virtual void WriteValue(short value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(ushort value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		public virtual void WriteValue(char value)
		{
			InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(byte value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		public virtual void WriteValue(decimal value)
		{
			InternalWriteValue(JsonToken.Float);
		}

		public virtual void WriteValue(DateTime value)
		{
			InternalWriteValue(JsonToken.Date);
		}

		public virtual void WriteValue(DateTimeOffset value)
		{
			InternalWriteValue(JsonToken.Date);
		}

		public virtual void WriteValue(Guid value)
		{
			InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(TimeSpan value)
		{
			InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(int? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(uint? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(long? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(ulong? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(float? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(double? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(bool? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(short? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(ushort? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(char? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(byte? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(decimal? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(DateTime? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(DateTimeOffset? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(Guid? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(TimeSpan? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.GetValueOrDefault());
			}
		}

		public virtual void WriteValue(byte[] value)
		{
			if (value == null)
			{
				WriteNull();
			}
			else
			{
				InternalWriteValue(JsonToken.Bytes);
			}
		}

		public virtual void WriteValue(Uri value)
		{
			if (value == null)
			{
				WriteNull();
			}
			else
			{
				InternalWriteValue(JsonToken.String);
			}
		}

		public virtual void WriteValue(object value)
		{
			if (value == null)
			{
				WriteNull();
			}
			else
			{
				if (value is BigInteger)
				{
					throw CreateUnsupportedTypeException(this, value);
				}
				WriteValue(this, ConvertUtils.GetTypeCode(value.GetType()), value);
			}
		}

		public virtual void WriteComment(string text)
		{
			InternalWriteComment();
		}

		public virtual void WriteWhitespace(string ws)
		{
			InternalWriteWhitespace(ws);
		}

		void IDisposable.Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_currentState != State.Closed && disposing)
			{
				Close();
			}
		}

		internal static void WriteValue(JsonWriter writer, PrimitiveTypeCode typeCode, object value)
		{
			while (true)
			{
				switch (typeCode)
				{
				case PrimitiveTypeCode.Char:
					writer.WriteValue((char)value);
					return;
				case PrimitiveTypeCode.CharNullable:
					writer.WriteValue((value == null) ? null : new char?((char)value));
					return;
				case PrimitiveTypeCode.Boolean:
					writer.WriteValue((bool)value);
					return;
				case PrimitiveTypeCode.BooleanNullable:
					writer.WriteValue((value == null) ? null : new bool?((bool)value));
					return;
				case PrimitiveTypeCode.SByte:
					writer.WriteValue((sbyte)value);
					return;
				case PrimitiveTypeCode.SByteNullable:
					writer.WriteValue((value == null) ? null : new sbyte?((sbyte)value));
					return;
				case PrimitiveTypeCode.Int16:
					writer.WriteValue((short)value);
					return;
				case PrimitiveTypeCode.Int16Nullable:
					writer.WriteValue((value == null) ? null : new short?((short)value));
					return;
				case PrimitiveTypeCode.UInt16:
					writer.WriteValue((ushort)value);
					return;
				case PrimitiveTypeCode.UInt16Nullable:
					writer.WriteValue((value == null) ? null : new ushort?((ushort)value));
					return;
				case PrimitiveTypeCode.Int32:
					writer.WriteValue((int)value);
					return;
				case PrimitiveTypeCode.Int32Nullable:
					writer.WriteValue((value == null) ? null : new int?((int)value));
					return;
				case PrimitiveTypeCode.Byte:
					writer.WriteValue((byte)value);
					return;
				case PrimitiveTypeCode.ByteNullable:
					writer.WriteValue((value == null) ? null : new byte?((byte)value));
					return;
				case PrimitiveTypeCode.UInt32:
					writer.WriteValue((uint)value);
					return;
				case PrimitiveTypeCode.UInt32Nullable:
					writer.WriteValue((value == null) ? null : new uint?((uint)value));
					return;
				case PrimitiveTypeCode.Int64:
					writer.WriteValue((long)value);
					return;
				case PrimitiveTypeCode.Int64Nullable:
					writer.WriteValue((value == null) ? null : new long?((long)value));
					return;
				case PrimitiveTypeCode.UInt64:
					writer.WriteValue((ulong)value);
					return;
				case PrimitiveTypeCode.UInt64Nullable:
					writer.WriteValue((value == null) ? null : new ulong?((ulong)value));
					return;
				case PrimitiveTypeCode.Single:
					writer.WriteValue((float)value);
					return;
				case PrimitiveTypeCode.SingleNullable:
					writer.WriteValue((value == null) ? null : new float?((float)value));
					return;
				case PrimitiveTypeCode.Double:
					writer.WriteValue((double)value);
					return;
				case PrimitiveTypeCode.DoubleNullable:
					writer.WriteValue((value == null) ? null : new double?((double)value));
					return;
				case PrimitiveTypeCode.DateTime:
					writer.WriteValue((DateTime)value);
					return;
				case PrimitiveTypeCode.DateTimeNullable:
					writer.WriteValue((value == null) ? null : new DateTime?((DateTime)value));
					return;
				case PrimitiveTypeCode.DateTimeOffset:
					writer.WriteValue((DateTimeOffset)value);
					return;
				case PrimitiveTypeCode.DateTimeOffsetNullable:
					writer.WriteValue((value == null) ? null : new DateTimeOffset?((DateTimeOffset)value));
					return;
				case PrimitiveTypeCode.Decimal:
					writer.WriteValue((decimal)value);
					return;
				case PrimitiveTypeCode.DecimalNullable:
					writer.WriteValue((value == null) ? null : new decimal?((decimal)value));
					return;
				case PrimitiveTypeCode.Guid:
					writer.WriteValue((Guid)value);
					return;
				case PrimitiveTypeCode.GuidNullable:
					writer.WriteValue((value == null) ? null : new Guid?((Guid)value));
					return;
				case PrimitiveTypeCode.TimeSpan:
					writer.WriteValue((TimeSpan)value);
					return;
				case PrimitiveTypeCode.TimeSpanNullable:
					writer.WriteValue((value == null) ? null : new TimeSpan?((TimeSpan)value));
					return;
				case PrimitiveTypeCode.BigInteger:
					writer.WriteValue((BigInteger)value);
					return;
				case PrimitiveTypeCode.BigIntegerNullable:
					writer.WriteValue((value == null) ? null : new BigInteger?((BigInteger)value));
					return;
				case PrimitiveTypeCode.Uri:
					writer.WriteValue((Uri)value);
					return;
				case PrimitiveTypeCode.String:
					writer.WriteValue((string)value);
					return;
				case PrimitiveTypeCode.Bytes:
					writer.WriteValue((byte[])value);
					return;
				case PrimitiveTypeCode.DBNull:
					writer.WriteNull();
					return;
				}
				IConvertible convertible;
				if ((convertible = (value as IConvertible)) == null)
				{
					break;
				}
				ResolveConvertibleValue(convertible, out typeCode, out value);
			}
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			throw CreateUnsupportedTypeException(writer, value);
		}

		private static void ResolveConvertibleValue(IConvertible convertible, out PrimitiveTypeCode typeCode, out object value)
		{
			TypeInformation typeInformation = ConvertUtils.GetTypeInformation(convertible);
			typeCode = ((typeInformation.TypeCode == PrimitiveTypeCode.Object) ? PrimitiveTypeCode.String : typeInformation.TypeCode);
			Type conversionType = (typeInformation.TypeCode == PrimitiveTypeCode.Object) ? typeof(string) : typeInformation.Type;
			value = convertible.ToType(conversionType, CultureInfo.InvariantCulture);
		}

		private static JsonWriterException CreateUnsupportedTypeException(JsonWriter writer, object value)
		{
			return JsonWriterException.Create(writer, "Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()), null);
		}

		protected void SetWriteState(JsonToken token, object value)
		{
			switch (token)
			{
			case JsonToken.StartObject:
				InternalWriteStart(token, JsonContainerType.Object);
				break;
			case JsonToken.StartArray:
				InternalWriteStart(token, JsonContainerType.Array);
				break;
			case JsonToken.StartConstructor:
				InternalWriteStart(token, JsonContainerType.Constructor);
				break;
			case JsonToken.PropertyName:
			{
				string name;
				if ((name = (value as string)) == null)
				{
					throw new ArgumentException("A name is required when setting property name state.", "value");
				}
				InternalWritePropertyName(name);
				break;
			}
			case JsonToken.Comment:
				InternalWriteComment();
				break;
			case JsonToken.Raw:
				InternalWriteRaw();
				break;
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				InternalWriteValue(token);
				break;
			case JsonToken.EndObject:
				InternalWriteEnd(JsonContainerType.Object);
				break;
			case JsonToken.EndArray:
				InternalWriteEnd(JsonContainerType.Array);
				break;
			case JsonToken.EndConstructor:
				InternalWriteEnd(JsonContainerType.Constructor);
				break;
			default:
				throw new ArgumentOutOfRangeException("token");
			}
		}

		internal void InternalWriteEnd(JsonContainerType container)
		{
			AutoCompleteClose(container);
		}

		internal void InternalWritePropertyName(string name)
		{
			_currentPosition.PropertyName = name;
			AutoComplete(JsonToken.PropertyName);
		}

		internal void InternalWriteRaw()
		{
		}

		internal void InternalWriteStart(JsonToken token, JsonContainerType container)
		{
			UpdateScopeWithFinishedValue();
			AutoComplete(token);
			Push(container);
		}

		internal void InternalWriteValue(JsonToken token)
		{
			UpdateScopeWithFinishedValue();
			AutoComplete(token);
		}

		internal void InternalWriteWhitespace(string ws)
		{
			if (ws != null && !StringUtils.IsWhiteSpace(ws))
			{
				throw JsonWriterException.Create(this, "Only white space characters should be used.", null);
			}
		}

		internal void InternalWriteComment()
		{
			AutoComplete(JsonToken.Comment);
		}

		[CompilerGenerated]
		private async Task _003CInternalWriteEndAsync_003Eg__AwaitProperty_007C11_0(Task task, int LevelsToComplete, JsonToken token, CancellationToken CancellationToken)
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
			if (_formatting == Formatting.Indented && _currentState != State.ObjectStart && _currentState != State.ArrayStart)
			{
				await WriteIndentAsync(CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			await WriteEndAsync(token, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			UpdateCurrentState();
			await _003CInternalWriteEndAsync_003Eg__AwaitRemaining_007C11_3(LevelsToComplete, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		[CompilerGenerated]
		private async Task _003CInternalWriteEndAsync_003Eg__AwaitIndent_007C11_1(Task task, int LevelsToComplete, JsonToken token, CancellationToken CancellationToken)
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
			await WriteEndAsync(token, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			UpdateCurrentState();
			await _003CInternalWriteEndAsync_003Eg__AwaitRemaining_007C11_3(LevelsToComplete, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		[CompilerGenerated]
		private async Task _003CInternalWriteEndAsync_003Eg__AwaitEnd_007C11_2(Task task, int LevelsToComplete, CancellationToken CancellationToken)
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
			UpdateCurrentState();
			await _003CInternalWriteEndAsync_003Eg__AwaitRemaining_007C11_3(LevelsToComplete, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		[CompilerGenerated]
		private async Task _003CInternalWriteEndAsync_003Eg__AwaitRemaining_007C11_3(int LevelsToComplete, CancellationToken CancellationToken)
		{
			while (LevelsToComplete-- > 0)
			{
				JsonToken token = GetCloseTokenForType(Pop());
				if (_currentState == State.Property)
				{
					await WriteNullAsync(CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (_formatting == Formatting.Indented && _currentState != State.ObjectStart && _currentState != State.ArrayStart)
				{
					await WriteIndentAsync(CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				await WriteEndAsync(token, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				UpdateCurrentState();
			}
		}
	}
}
