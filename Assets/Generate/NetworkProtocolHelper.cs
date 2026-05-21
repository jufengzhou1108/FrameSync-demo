using System.Runtime.CompilerServices;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using System.Collections.Generic;
#pragma warning disable CS8618
namespace Fantasy
{
   public static class NetworkProtocolHelper
   {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<G2C_JoinResponse> C2G_JoinRequest(this Session session, C2G_JoinRequest C2G_JoinRequest_request)
		{
			return (G2C_JoinResponse)await session.Call(C2G_JoinRequest_request);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<G2C_JoinResponse> C2G_JoinRequest(this Session session)
		{
			using var C2G_JoinRequest_request = Fantasy.C2G_JoinRequest.Create();
			return (G2C_JoinResponse)await session.Call(C2G_JoinRequest_request);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void OneFrameCommand(this Session session, OneFrameCommand OneFrameCommand_message)
		{
			session.Send(OneFrameCommand_message);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void OneFrameCommand(this Session session, int frameId, List<Command> commands)
		{
			using var OneFrameCommand_message = Fantasy.OneFrameCommand.Create();
			OneFrameCommand_message.frameId = frameId;
			OneFrameCommand_message.commands = commands;
			session.Send(OneFrameCommand_message);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Command(this Session session, Command Command_message)
		{
			session.Send(Command_message);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Command(this Session session, long clientId, int frameId, int commandType, int x, int y, int z)
		{
			using var Command_message = Fantasy.Command.Create();
			Command_message.clientId = clientId;
			Command_message.frameId = frameId;
			Command_message.commandType = commandType;
			Command_message.x = x;
			Command_message.y = y;
			Command_message.z = z;
			session.Send(Command_message);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void G2C_StartFrameSync(this Session session, G2C_StartFrameSync G2C_StartFrameSync_message)
		{
			session.Send(G2C_StartFrameSync_message);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void G2C_StartFrameSync(this Session session, List<long> clientIdList, List<Position> positionList)
		{
			using var G2C_StartFrameSync_message = Fantasy.G2C_StartFrameSync.Create();
			G2C_StartFrameSync_message.clientIdList = clientIdList;
			G2C_StartFrameSync_message.positionList = positionList;
			session.Send(G2C_StartFrameSync_message);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void C2G_EndSync(this Session session, C2G_EndSync C2G_EndSync_message)
		{
			session.Send(C2G_EndSync_message);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void C2G_EndSync(this Session session)
		{
			using var message = Fantasy.C2G_EndSync.Create();
			session.Send(message);
		}

   }
}