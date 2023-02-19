// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/greet.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981
#region Designer generated code

using grpc = global::Grpc.Core;

namespace ShireBankService {
  /// <summary>
  /// The greeting service definition.
  /// </summary>
  public static partial class Bank
  {
    static readonly string __ServiceName = "greet.Bank";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::ShireBankService.OpenAccountRequest> __Marshaller_greet_OpenAccountRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::ShireBankService.OpenAccountRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::ShireBankService.OpenAccountReply> __Marshaller_greet_OpenAccountReply = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::ShireBankService.OpenAccountReply.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::ShireBankService.WithdrawRequest> __Marshaller_greet_WithdrawRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::ShireBankService.WithdrawRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.FloatValue> __Marshaller_google_protobuf_FloatValue = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Google.Protobuf.WellKnownTypes.FloatValue.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::ShireBankService.DepositRequest> __Marshaller_greet_DepositRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::ShireBankService.DepositRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Google.Protobuf.WellKnownTypes.Empty.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.UInt32Value> __Marshaller_google_protobuf_UInt32Value = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Google.Protobuf.WellKnownTypes.UInt32Value.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.StringValue> __Marshaller_google_protobuf_StringValue = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Google.Protobuf.WellKnownTypes.StringValue.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.BoolValue> __Marshaller_google_protobuf_BoolValue = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Google.Protobuf.WellKnownTypes.BoolValue.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::ShireBankService.OpenAccountRequest, global::ShireBankService.OpenAccountReply> __Method_OpenAccount = new grpc::Method<global::ShireBankService.OpenAccountRequest, global::ShireBankService.OpenAccountReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "OpenAccount",
        __Marshaller_greet_OpenAccountRequest,
        __Marshaller_greet_OpenAccountReply);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::ShireBankService.WithdrawRequest, global::Google.Protobuf.WellKnownTypes.FloatValue> __Method_Withdraw = new grpc::Method<global::ShireBankService.WithdrawRequest, global::Google.Protobuf.WellKnownTypes.FloatValue>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Withdraw",
        __Marshaller_greet_WithdrawRequest,
        __Marshaller_google_protobuf_FloatValue);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::ShireBankService.DepositRequest, global::Google.Protobuf.WellKnownTypes.Empty> __Method_Deposit = new grpc::Method<global::ShireBankService.DepositRequest, global::Google.Protobuf.WellKnownTypes.Empty>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Deposit",
        __Marshaller_greet_DepositRequest,
        __Marshaller_google_protobuf_Empty);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.UInt32Value, global::Google.Protobuf.WellKnownTypes.StringValue> __Method_GetHistory = new grpc::Method<global::Google.Protobuf.WellKnownTypes.UInt32Value, global::Google.Protobuf.WellKnownTypes.StringValue>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetHistory",
        __Marshaller_google_protobuf_UInt32Value,
        __Marshaller_google_protobuf_StringValue);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.UInt32Value, global::Google.Protobuf.WellKnownTypes.BoolValue> __Method_CloseAccount = new grpc::Method<global::Google.Protobuf.WellKnownTypes.UInt32Value, global::Google.Protobuf.WellKnownTypes.BoolValue>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CloseAccount",
        __Marshaller_google_protobuf_UInt32Value,
        __Marshaller_google_protobuf_BoolValue);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.StringValue> __Method_StartInspection = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.StringValue>(
        grpc::MethodType.Unary,
        __ServiceName,
        "StartInspection",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_StringValue);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.StringValue> __Method_FinishInspection = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.StringValue>(
        grpc::MethodType.Unary,
        __ServiceName,
        "FinishInspection",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_StringValue);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.StringValue> __Method_GetFullSummary = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.StringValue>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetFullSummary",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_StringValue);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::ShireBankService.GreetReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Bank</summary>
    [grpc::BindServiceMethod(typeof(Bank), "BindService")]
    public abstract partial class BankBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::ShireBankService.OpenAccountReply> OpenAccount(global::ShireBankService.OpenAccountRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Google.Protobuf.WellKnownTypes.FloatValue> Withdraw(global::ShireBankService.WithdrawRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Google.Protobuf.WellKnownTypes.Empty> Deposit(global::ShireBankService.DepositRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Google.Protobuf.WellKnownTypes.StringValue> GetHistory(global::Google.Protobuf.WellKnownTypes.UInt32Value request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Google.Protobuf.WellKnownTypes.BoolValue> CloseAccount(global::Google.Protobuf.WellKnownTypes.UInt32Value request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Google.Protobuf.WellKnownTypes.StringValue> StartInspection(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Google.Protobuf.WellKnownTypes.StringValue> FinishInspection(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Google.Protobuf.WellKnownTypes.StringValue> GetFullSummary(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(BankBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_OpenAccount, serviceImpl.OpenAccount)
          .AddMethod(__Method_Withdraw, serviceImpl.Withdraw)
          .AddMethod(__Method_Deposit, serviceImpl.Deposit)
          .AddMethod(__Method_GetHistory, serviceImpl.GetHistory)
          .AddMethod(__Method_CloseAccount, serviceImpl.CloseAccount)
          .AddMethod(__Method_StartInspection, serviceImpl.StartInspection)
          .AddMethod(__Method_FinishInspection, serviceImpl.FinishInspection)
          .AddMethod(__Method_GetFullSummary, serviceImpl.GetFullSummary).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, BankBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_OpenAccount, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::ShireBankService.OpenAccountRequest, global::ShireBankService.OpenAccountReply>(serviceImpl.OpenAccount));
      serviceBinder.AddMethod(__Method_Withdraw, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::ShireBankService.WithdrawRequest, global::Google.Protobuf.WellKnownTypes.FloatValue>(serviceImpl.Withdraw));
      serviceBinder.AddMethod(__Method_Deposit, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::ShireBankService.DepositRequest, global::Google.Protobuf.WellKnownTypes.Empty>(serviceImpl.Deposit));
      serviceBinder.AddMethod(__Method_GetHistory, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Protobuf.WellKnownTypes.UInt32Value, global::Google.Protobuf.WellKnownTypes.StringValue>(serviceImpl.GetHistory));
      serviceBinder.AddMethod(__Method_CloseAccount, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Protobuf.WellKnownTypes.UInt32Value, global::Google.Protobuf.WellKnownTypes.BoolValue>(serviceImpl.CloseAccount));
      serviceBinder.AddMethod(__Method_StartInspection, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.StringValue>(serviceImpl.StartInspection));
      serviceBinder.AddMethod(__Method_FinishInspection, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.StringValue>(serviceImpl.FinishInspection));
      serviceBinder.AddMethod(__Method_GetFullSummary, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.StringValue>(serviceImpl.GetFullSummary));
    }

  }
}
#endregion